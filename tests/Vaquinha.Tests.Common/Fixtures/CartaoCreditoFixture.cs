using Bogus;
using Xunit;
using Vaquinha.Domain.Entities;
using Vaquinha.Domain.ViewModels;

namespace Vaquinha.Tests.Common.Fixtures
{
    [CollectionDefinition(nameof(CreditCardFixtureCollection))]
    public class CreditCardFixtureCollection : ICollectionFixture<CreditCardFixture>
    {
    }
    public class CreditCardFixture
    {
        public CreditCardViewModel CreditCardModelValido()
        {
            var creditCard = new Faker().Finance;

            var faker = new Faker<CreditCardViewModel>("pt_BR");

            faker.RuleFor(c => c.CVV, (f, c) => creditCard.CreditCardCvv());
            faker.RuleFor(c => c.NomeTitular, (f, c) => f.Person.FullName);
            faker.RuleFor(c => c.NumeroCreditCard, (f, c) => creditCard.CreditCardNumber());
            faker.RuleFor(c => c.Validade, (f, c) => "06/28");

            return faker.Generate();
        }

        public CreditCard ValidCreditCard()
        {
            var creditCard = new Faker("pt_BR").Finance;
            var pessoa = new Faker("pt_BR").Person;

            var faker = new Faker<CreditCard>("pt_BR");

            faker.CustomInstantiator(f =>
                 new CreditCard(pessoa.FullName, creditCard.CreditCardNumber(), "06/28", creditCard.CreditCardCvv()));

            return faker.Generate();
        }

        public CreditCard CreditCardVazio()
        {
            return new CreditCard(string.Empty, string.Empty, string.Empty, string.Empty);
        }

        public CreditCard CreditCardNumeroValidadeCVVInvalido()
        {
            var creditCard = new Faker("pt_BR").Finance;
            var pessoa = new Faker("pt_BR").Person;

            var faker = new Faker<CreditCard>("pt_BR");

            faker.CustomInstantiator(f =>
                 new CreditCard(pessoa.FullName, "21125684", "14/25", "312q"));

            return faker.Generate();
        }

        public CreditCard CreditCardValidadeExpirada()
        {
            var creditCard = new Faker("pt_BR").Finance;
            var pessoa = new Faker("pt_BR").Person;

            var faker = new Faker<CreditCard>("pt_BR");

            faker.CustomInstantiator(f =>
                 new CreditCard(pessoa.FullName, creditCard.CreditCardNumber(), "06/19", creditCard.CreditCardCvv()));

            return faker.Generate();
        }

        public CreditCard CreditCardNomeTitularMaxLengthInvalido()
        {
            const string TEXTO_COM_MAIS_DE_150_CARACTERES = "AHIUDHASHOIFJOASJPFPOKAPFOKPKQPOFKOPQKWPOFEMMVIMWPOVPOQWPMVPMQOPIPQMJEOIPFMOIQOIFMCOKQMEWVMOPMQEOMVOPMWQOEMVOWMEOMVOIQMOIVMQEHISUAHDUIHASIUHDIHASIUHDUIHIAUSHIDUHAIUSDQWMFMPEQPOGFMPWEMGVWEM";
            var creditCard = new Faker("pt_BR").Finance;

            var faker = new Faker<CreditCard>("pt_BR");

            faker.CustomInstantiator(f =>
                 new CreditCard(TEXTO_COM_MAIS_DE_150_CARACTERES, creditCard.CreditCardNumber(), "06/28", creditCard.CreditCardCvv()));

            return faker.Generate();
        }

    }
}
