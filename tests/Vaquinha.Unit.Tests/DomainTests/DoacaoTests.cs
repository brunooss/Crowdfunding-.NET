using FluentAssertions;
using Xunit;
using Vaquinha.Tests.Common.Fixtures;

namespace Vaquinha.Unit.Tests.DomainTests
{
    [Collection(nameof(DonationFixtureCollection))]    
    public class DonationTests: IClassFixture<DonationFixture>, 
                              IClassFixture<AddressFixture>, 
                              IClassFixture<CreditCardFixture>
    {
        private readonly DonationFixture _donationFixture;
        private readonly AddressFixture _addressFixture;
        private readonly CreditCardFixture _creditCardFixture;

        public DonationTests(DonationFixture donationFixture, AddressFixture addressFixture, CreditCardFixture creditCardFixture)
        {
            _donationFixture = donationFixture;
            _addressFixture = addressFixture;
            _creditCardFixture = creditCardFixture;
        }

        [Fact]
        [Trait("Donation", "Donation_CorretamentePreenchidos_ValidDonation")]
        public void Donation_CorretamentePreenchidos_ValidDonation()
        {           
            // Arrange
            var donation = _donationFixture.ValidDonation();
            donation.AdicionarAddressCobranca(_addressFixture.ValidAddress());
            donation.AdicionarFormaPagamento(_creditCardFixture.ValidCreditCard());

            // Act
            var valido = donation.Valido();

            // Assert
            valido.Should().BeTrue(because: "os campos foram preenchidos corretamente");
            donation.ErrorMessages.Should().BeEmpty();
        }

        [Fact]
        [Trait("Donation", "Donation_DadosPessoaisInvalidos_DonationInvalida")]
        public void Donation_DadosPessoaisInvalidos_DonationInvalida()
        {
            // Arrange
            const bool EMAIL_INVALIDO = true;
            var donation = _donationFixture.ValidDonation(EMAIL_INVALIDO);
            donation.AdicionarAddressCobranca(_addressFixture.ValidAddress());
            donation.AdicionarFormaPagamento(_creditCardFixture.ValidCreditCard());

            // Act
            var valido = donation.Valido();

            // Assert
            valido.Should().BeFalse(because: "o campo email está inválido");
            donation.ErrorMessages.Should().Contain("O campo Email é inválido.");
            donation.ErrorMessages.Should().HaveCount(1, because: "somente o campo email está inválido.");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        [InlineData(-5)]
        [InlineData(-10.20)]
        [InlineData(-55.4)]
        [InlineData(-0.1)]
        [Trait("Donation", "Donation_ValoresDonationMenorIgualZero_DonationInvalida")]
        public void Donation_ValoresDonationMenorIgualZero_DonationInvalida(double valorDonation)
        {
            // Arrange            
            var donation = _donationFixture.ValidDonation(false, valorDonation);
            donation.AdicionarAddressCobranca(_addressFixture.ValidAddress());
            donation.AdicionarFormaPagamento(_creditCardFixture.ValidCreditCard());

            // Act
            var valido = donation.Valido();

            // Assert
            valido.Should().BeFalse(because: "o campo Valor está inválido");
            donation.ErrorMessages.Should().Contain("Valor mínimo de doação é de R$ 5,00");
            donation.ErrorMessages.Should().HaveCount(1, because: "somente o campo Valor está inválido.");
        }

        [Theory]
        [InlineData(25000)]
        [InlineData(5500.50)]
        [InlineData(5000.1)]
        [InlineData(4505)]
        [InlineData(4500.1)]
        [Trait("Donation", "Donation_ValoresDonationMaiorLimite_DonationInvalida")]
        public void Donation_ValoresDonationMaiorLimite_DonationInvalida(double valorDonation)
        {
            // Arrange
            const bool EXCEDER_MAX_VALOR_DOACAO = true;
            var donation = _donationFixture.ValidDonation(false, valorDonation);
            donation.AdicionarAddressCobranca(_addressFixture.ValidAddress());
            donation.AdicionarFormaPagamento(_creditCardFixture.ValidCreditCard());

            // Act
            var valido = donation.Valido();

            // Assert
            valido.Should().BeFalse(because: "o campo Valor está inválido");
            donation.ErrorMessages.Should().Contain("Valor máximo para a doação é de R$4.500,00");
            donation.ErrorMessages.Should().HaveCount(1, because: "somente o campo Valor está inválido.");
        }

        [Fact]
        [Trait("Donation", "Donation_MensagemApoioMaxlenghtExecido_DonationInvalida")]
        public void Donation_MensagemApoioMaxlenghtExecido_DonationInvalida()
        {
            // Arrange
            const bool EXCEDER_MAX_LENTH_MENSAGEM_APOIO = true;
            var donation = _donationFixture.ValidDonation(false, null, EXCEDER_MAX_LENTH_MENSAGEM_APOIO);
            donation.AdicionarAddressCobranca(_addressFixture.ValidAddress());
            donation.AdicionarFormaPagamento(_creditCardFixture.ValidCreditCard());

            // Act
            var valido = donation.Valido();

            // Assert
            valido.Should().BeFalse(because: "O campo Mensagem de Apoio possui mais caracteres do que o permitido");
            donation.ErrorMessages.Should().HaveCount(1, because: "somente o campo Mensagem deApoio está inválido.");
            donation.ErrorMessages.Should().Contain("O campo Mensagem de Apoio deve possuir no máximo 500 caracteres.");
        }

        [Fact]
        [Trait("Donation", "Donation_DadosNaoInformados_DonationInvalida")]
        public void Donation_DadosNaoInformados_DonationInvalida()
        {
            // Arrange
            var donation = _donationFixture.DonationInvalida(false);
            donation.AdicionarAddressCobranca(_addressFixture.ValidAddress());
            donation.AdicionarFormaPagamento(_creditCardFixture.ValidCreditCard());

            // Act
            var valido = donation.Valido();

            // Assert
            valido.Should().BeFalse(because: "os campos da doação nao foram informados");

            donation.ErrorMessages.Should().HaveCount(3, because: "Os 3 campos obrigatórios da doação não foram preenchidos");

            donation.ErrorMessages.Should().Contain("Valor mínimo de doação é de R$ 5,00", because: "valor mínimo de doação nao foi atingido.");
            donation.ErrorMessages.Should().Contain("O campo Nome é obrigatório.", because: "o campo Nome não foi informado.");
            donation.ErrorMessages.Should().Contain("O campo Email é obrigatório.", because: "o campo Email não foi informado.");            
        }

        [Fact]
        [Trait("Donation", "Donation_DadosNaoInformadosDonationAnonima_DonationInvalida")]
        public void Donation_DadosNaoInformadosDonationAnonima_DonationInvalida()
        {
            // Arrange
            var donation = _donationFixture.DonationInvalida(true);
            donation.AdicionarAddressCobranca(_addressFixture.ValidAddress());
            donation.AdicionarFormaPagamento(_creditCardFixture.ValidCreditCard());

            // Act
            var valido = donation.Valido();

            // Assert
            valido.Should().BeFalse(because: "os campos da doação nao foram informados");

            donation.ErrorMessages.Should().HaveCount(2, because: "Os 2 campos obrigatórios da doação não foram preenchidos");

            donation.ErrorMessages.Should().Contain("Valor mínimo de doação é de R$ 5,00", because: "valor mínimo de doação nao foi atingido.");            
            donation.ErrorMessages.Should().Contain("O campo Email é obrigatório.", because: "o campo Email não foi informado.");            
        }

    }
}
