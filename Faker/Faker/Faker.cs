using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Faker.Faker.Generators;

namespace Faker.Faker
{
    public class Faker
    {
        private readonly Dictionary<Type, IGenerator> _generators;
        private readonly string _pluginPath = Path.Combine(Directory.GetCurrentDirectory(), "Plugins");
        private int index = -1;
        private List<Type> _type;
        
        public Faker()
        {
            _generators = new Dictionary<Type, IGenerator>();
            _type = new List<Type>();
            LoadGeneratorsFromDirectory();
        }

        private void LoadGeneratorsFromDirectory()
        {
            if (_generators == null) return;
            
            var pluginDirectory = new DirectoryInfo(_pluginPath);
            if (!pluginDirectory.Exists)
            {
                pluginDirectory.Create();
                return;
            }

            var pluginFiles = Directory.GetFiles(pluginDirectory.FullName,"*.dll");

            foreach (var pluginFile in pluginFiles)
            {
                var assembly = Assembly.LoadFrom(pluginFile);
                LoadGeneratorsFromAssembly(assembly);  
            }
        }

        private void LoadGeneratorsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetTypes().Where(type => typeof(IGenerator).IsAssignableFrom(type));
            foreach (var type in types)
            {
                
                if (!type.IsClass) continue;
                var generatorPlugin = (IGenerator) assembly.CreateInstance(type.FullName);
                if (generatorPlugin != null)
                {
                    _generators.Add(generatorPlugin.GetGenerationType(), generatorPlugin);
                }
            }
        }
        
        public T Create<T>()
        {
            var type = typeof(T);
            if (type.IsAbstract) return default;
            if (_generators.TryGetValue(type, out var generator))
            {
                return (T) generator.Generate();
            }
            if (type.IsEnum)
            {
                return (T) new EnumGenerator<T>().Generate();
            }
            if (type.IsArray)
            {
                return (T) new ArrayGenerator<T>().Generate();
            }
            if (type == typeof(string))
            {
                return default;
            } 
            if (type.IsValueType) 
            { 
                return CreateStruct<T>();
            }
            if (type.IsClass || type.IsValueType)
            {
                if (_type.Contains(type))
                {
                    if (_type.IndexOf(type) == index)
                    {
                        index = -1;
                        return default;
                    }
                    if (index == -1)
                    {
                        index = _type.IndexOf(type);
                    }
                }
                _type.Add(type);
                var instance = (T) CreateByConstructor(type);
                if (instance == null)
                {
                    _type.Remove(type);
                    return default;
                }
                FillObject(instance);
                _type.Remove(type);

                return instance;
            }

            return default;
        }

        private object Create(Type type)
        {
            var create = typeof(Faker).GetMethod("Create");
            return create == null ? null : create.MakeGenericMethod(type).Invoke(this, null);
        }

        private object CreateByConstructor(Type type)
        {
            var constructors = type.GetConstructors();
            var constructor = GetConstructorWithMaxParam(constructors);
            if (constructor == null) return null;

            var parametersInfo = constructor.GetParameters();

            return constructor.Invoke(parametersInfo.Select(parameter => Create(parameter.ParameterType)).ToArray());
        }

        private static ConstructorInfo GetConstructorWithMaxParam(IReadOnlyList<ConstructorInfo> constructors)
        {
            if (constructors == null || constructors.Count <= 0) return null;
            
            var constructorInfo = constructors[0];
            foreach (var constructor in constructors)
            {
                if (constructor.GetParameters().Length > constructorInfo.GetParameters().Length)
                {
                    constructorInfo = constructor;
                }
            }
            return constructorInfo;
        }
        
        private void FillObject( object instance)
        {
            var type = instance.GetType();
            var fields = new List<FieldInfo>(type.GetFields());
            foreach (var field in fields)
            {
                if (field.IsLiteral) continue;
                var fieldType = field.FieldType;
                var value = Create(fieldType);
                field.SetValue(instance, value);
            }
            var properties = new List<PropertyInfo>(type.GetProperties());
            foreach (var property in properties)
            {
                if (!property.CanWrite) continue;
                var propertyType = property.PropertyType;
                var value = Create(propertyType);
                property.SetValue(instance, value);
            }
        }
        private T CreateStruct<T>()
        {
            Type type = typeof(T);
            object o = Activator.CreateInstance(type);
            FillObject(o);
            return (T)o;
        }
    }
}