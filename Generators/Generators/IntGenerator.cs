using System;
using Faker.Faker.Generators;

namespace Generators.Generators
{
    public class IntGenerator : IGenerator
    {
        private static readonly Random Random = new Random();
        public object Generate()
        {
            return Random.Next();
        }

        public Type GetGenerationType()
        {
            return typeof(int);
        }
    }
}