using Bogus;
using Bogus.DataSets;
using System;
using Xunit;
using Vaquinha.Domain.Entities;
using Vaquinha.Domain.ViewModels;

namespace Vaquinha.Tests.Common.Fixtures
{
    [CollectionDefinition(nameof(DonationFixtureCollection))]
    public class DonationFixtureCollection : ICollectionFixture<DonationFixture>, ICollectionFixture<AddressFixture>, ICollectionFixture<CreditCardFixture>
    {
    }

    public class DonationFixture
    {
        public DonationViewModel DonationModelValida()
        {
            var faker = new Faker<DonationViewModel>("pt_BR");

            const int MIN_VALUE = 1;
            const int MAX_VALUE = 500;
            const int DECIMALS = 2;

            faker.RuleFor(c => c.Valor, (f, c) => f.Finance.Amount(MIN_VALUE, MAX_VALUE, DECIMALS));
            
            var retorno = faker.Generate();

            retorno.DadosPessoais = PessoaModelValida();

            return retorno;
        }

        public Donation ValidDonation(bool emailInvalido = false, double? valor = 5, bool maxLenghField = false)
        {            
            var faker = new Faker<Donation>("pt_BR");

            const int MIN_VALUE = 1;
            const int MAX_VALUE = 500;
            const int DECIMALS = 2;

            faker.CustomInstantiator(f => new Donation(Guid.Empty, Guid.Empty, Guid.Empty, valor ?? (double)f.Finance.Amount(MIN_VALUE, MAX_VALUE, DECIMALS), 
                                                        PessoaValida(emailInvalido, maxLenghField), null, null));

            return faker.Generate();
        }

        public DonationViewModel DonationModelInvalidaValida()
        {
            return new DonationViewModel();
        }

        public Donation DonationInvalida(bool donationAnonima = false)
        {
            var pessoaInvalida = new Pessoa(Guid.Empty, string.Empty, string.Empty, donationAnonima, string.Empty);
            return new Donation(Guid.Empty, Guid.Empty, Guid.Empty, 0, pessoaInvalida, null, null );
        }

        public Pessoa PessoaValida(bool emailInvalido = false,bool maxLenghField = false)
        {            
            var pessoa = new Faker().Person;

            var faker = new Faker<Pessoa>("pt_BR");

            faker.CustomInstantiator(f =>
                 new Pessoa(Guid.NewGuid(), pessoa.FullName, string.Empty, false, maxLenghField ? f.Lorem.Sentence(501) : f.Lorem.Sentence(30)))
                .RuleFor(c => c.Email, (f, c) => emailInvalido ? "EMAIL_INVALIDO" : f.Internet.Email(c.Nome.ToLower(), c.Nome.ToLower()));

            return faker.Generate();
        }

        public PessoaViewModel PessoaModelValida(bool emailInvalido = false)
        {
            var genero = new Faker().PickRandom<Name.Gender>();

            var faker = new Faker<PessoaViewModel>("pt_BR");

            faker.RuleFor(a => a.Nome, (f, c) => f.Name.FirstName(genero));
            faker.RuleFor(a => a.Email, (f, c) => emailInvalido ? "EMAIL_INVALIDO" : f.Internet.Email(c.Nome.ToLower(), c.Nome.ToLower()));

            return faker.Generate();
        }
    }
}