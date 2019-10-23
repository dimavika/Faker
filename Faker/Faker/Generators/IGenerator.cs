using System;

namespace Faker.Faker.Generators
{
    public interface IGenerator
    {
        object Generate();

        Type GetGenerationType();
    }
}