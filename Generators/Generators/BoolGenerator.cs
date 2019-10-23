using System;
using Faker.Faker.Generators;

namespace Generators.Generators
{
    public class BoolGenerator : IGenerator
    {
        public object Generate()
        {
            return true;
        }

        public Type GetGenerationType()
        {
            return typeof(bool);
        }
    }
}