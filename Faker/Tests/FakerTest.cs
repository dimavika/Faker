using System;
using Faker.DTO;
using NUnit.Framework;

namespace Faker.Tests
{
    public class Tests
    {
        private bool _bool;
        private byte _byte;
        private char _char;
        private double _double;
        private float _float;
        private int _int;
        private ulong _long;
        private short _short;

        private DateTime _dateTime;
        private int[] _ints;
        private MyEnum _myEnum;
        private string _string;
        
        public class  Class1
        {
            
            private Class1()
            {
            }
        }
        
        private readonly Faker.Faker _faker = new Faker.Faker();

        [Test]
        public void RecursionControl()
        {
            var value = _faker.Create<CycleDep1>();
            Assert.NotNull(value);
            Assert.NotNull(value.CycleDep2);
            Assert.NotNull(value.CycleDep2.CycleDep3);
            Assert.NotNull(value.CycleDep2.CycleDep3.CycleDep2);
            Assert.Null(value.CycleDep2.CycleDep3.CycleDep2.CycleDep3.CycleDep2);
            
        }

        [Test]
        public void Test1()
        {
            var value = _faker.Create<Class1>();
            Assert.Null(value);
        }

        [Test]
        public void BoolNotStandardGeneration()
        {
            var value = _faker.Create<bool>();
            Assert.AreNotEqual(_bool, value);
        }
        
        [Test]
        public void ByteNotStandardGeneration()
        {
            var value = _faker.Create<byte>();
            Assert.AreNotEqual(_byte, value);
        }
        
        [Test]
        public void CharNotStandardGeneration()
        {
            var value = _faker.Create<char>();
            Assert.AreNotEqual(_char, value);
        }
        
        [Test]
        public void DoubleNotStandardGeneration()
        {
            var value = _faker.Create<double>();
            Assert.AreNotEqual(_double, value);
        }
        
        [Test]
        public void FloatNotStandardGeneration()
        {
            var value = _faker.Create<float>();
            Assert.AreNotEqual(_float, value);
        }
        
        [Test]
        public void IntNotStandardGeneration()
        {
            var value = _faker.Create<int>();
            Assert.AreNotEqual(_int, value);
        }
        
        [Test]
        public void LongNotStandardGeneration()
        {
            var value = _faker.Create<ulong>();
            Assert.AreNotEqual(_long, value);
        }

        [Test]
        public void ShortNotStandardGeneration()
        {
            var value = _faker.Create<short>();
            Assert.AreNotEqual(_short, value);
        }

        [Test]
        public void DateTimeNotStandardGeneration()
        {
            var value = _faker.Create<DateTime>();
            Assert.AreNotEqual(_dateTime, value);
        }
        
        [Test]
        public void ArrayNotStandardGeneration()
        {
            var value = _faker.Create<int[]>();
            Assert.AreNotEqual(_ints, value);
            foreach (var variable in value)
            {
                Console.WriteLine(variable);
            }
        }
        
        [Test]
        public void EnumNotStandardGeneration()
        {
            var value = _faker.Create<MyEnum>();
            Assert.AreNotEqual(_myEnum, value);
        }
        
        [Test]
        public void StringNotStandardGeneration()
        {
            var value = _faker.Create<string>();
            Assert.AreNotEqual(_string, value);
        }

        [Test]
        public void GenerationThroughConstructor()
        {
            var o1 = new ConstructorDto();
            var value = _faker.Create<ConstructorDto>();
            Assert.AreNotEqual(01, value);
        }

        [Test]
        public void GenerationThroughProperties()
        {
            var o1 = new PropertiesDto();
            var value = _faker.Create<PropertiesDto>();
            Assert.AreNotEqual(01, value);
        }

        [Test]
        public void GenerationThroughFields()
        {
            var o1 = new FieldsDto();
            var value = _faker.Create<FieldsDto>();
            Assert.AreNotEqual(01, value);
        }
        
        [Test]
        public void GenerationDtoWithDto()
        {
            var o1 = new ConstructorDto();
            var value = _faker.Create<DtoWithInnerDto>();
            Assert.AreNotEqual(null, value._constructorDto);
            Assert.AreNotEqual(01, value._constructorDto);
        }

        struct MyStruct
        {
            public int Integer { get; set; }
        }

        [Test]
        public void StructTst()
        {
            var @struct = _faker.Create<MyStruct>();
            Assert.AreNotEqual(@struct.Integer, 0);
        }
    }
}