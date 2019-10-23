using System;

namespace Faker.Faker.Generators
{
    public abstract class NumGenerator
    {
        public abstract class NumericGenerator: IGenerator
        {
            protected static Random Random = new Random();
            public abstract object Generate();
            public Type GetGenerationType()
            {
                throw new NotImplementedException();
            }
        }
    }
}