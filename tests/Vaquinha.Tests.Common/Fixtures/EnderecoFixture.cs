using Bogus;
using Bogus.DataSets;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Vaquinha.Domain.Entities;
using Vaquinha.Domain.ViewModels;

namespace Vaquinha.Tests.Common.Fixtures
{
    [CollectionDefinition(nameof(AddressFixtureCollection))]
    public class AddressFixtureCollection : ICollectionFixture<AddressFixture>
    {
    }
    public class AddressFixture
    {
        public AddressViewModel AddressModelValido()
        {
            var address = new Faker().Address;

            var faker = new Faker<AddressViewModel>("pt_BR");

            faker.RuleFor(c => c.CEP, (f, c) => "14800-700");
            faker.RuleFor(c => c.Cidade, (f, c) => address.City());
            faker.RuleFor(c => c.Estado, (f, c) => address.StateAbbr());
            faker.RuleFor(c => c.TextoAddress, (f, c) => address.StreetAddress());            

            return faker.Generate();
        }

        public Domain.Entities.Address ValidAddress()
        {
            var address = new Faker("pt_BR").Address;
            
            var faker = new Faker<Domain.Entities.Address>("pt_BR");

            faker.CustomInstantiator(f =>
                 new Domain.Entities.Address(Guid.NewGuid(), "14800-000", address.StreetAddress(false), string.Empty, address.City(), address.StateAbbr(), "16995811385", "100A"));

            return faker.Generate();
        }

        public Domain.Entities.Address EmptyAddress()
        {
            return new Domain.Entities.Address(Guid.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
        }

        public Domain.Entities.Address AddressCneNumberStateInvalid()
        {
            var address = new Faker("pt_BR").Address;

            var faker = new Faker<Domain.Entities.Address>("pt_BR");

            faker.CustomInstantiator(f =>
                 new Domain.Entities.Address(Guid.NewGuid(), "14800-0000", address.StreetAddress(false), string.Empty, address.City(), address.State(), "169958113859", "2005"));

            return faker.Generate();
        }

        public Domain.Entities.Address AddressMaxLength()
        {
            const string MORE_THAN_250_CHARACTERS_TEXT = "AHIUDHASHOIFJOASJPFPOKAPFOKPKQPOFKOPQKWPOFEMMVIMWPOVPOQWPMVPMQOPIPQMJEOIPFMOIQOIFMCOKQMEWVMOPMQEOMVOPMWQOEMVOWMEOMVOIQMOIVMQEHISUAHDUIHASIUHDIHASIUHDUIHIAUSHIDUHAIUSDQWMFMPEQPOGFMPWEMGVWEM CQPWEM,CPQWPMCEOWIMVOEWOINMMFWOIEMFOIMOIOWEMFOIEWMFOIWEMFOWEOIMF";

            var address = new Faker("pt_BR").Address;

            var faker = new Faker<Domain.Entities.Address>("pt_BR");

            faker.CustomInstantiator(f =>
                 new Domain.Entities.Address(Guid.NewGuid(), "14800-000", MORE_THAN_250_CHARACTERS_TEXT, MORE_THAN_250_CHARACTERS_TEXT, MORE_THAN_250_CHARACTERS_TEXT, address.StateAbbr(), "16995811385", "1234567"));

            return faker.Generate();
        }

    }
}
