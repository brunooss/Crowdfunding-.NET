using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Vaquinha.Tests.Common.Fixtures;

namespace Vaquinha.Unit.Tests.DomainTests
{
    [Collection(nameof(CreditCardFixtureCollection))]
    public class CreditCardTests: IClassFixture<CreditCardFixture>
    {
        private readonly CreditCardFixture _fixture;

        public CreditCardTests(CreditCardFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        [Trait("CreditCard", "CreditCard_CorretamentePreenchido_ValidCreditCard")]
        public void CreditCard_CorretamentePreenchido_ValidCreditCard()
        {
            // Arrange
            var creditCard = _fixture.ValidCreditCard();

            // Act
            var valido = creditCard.Valido();

            // Assert
            valido.Should().BeTrue(because: "os campos foram preenchidos corretamente");
            creditCard.ErrorMessages.Should().BeEmpty();
        }

        [Fact]
        [Trait("CreditCard", "CreditCard_NenhumDadoPreenchido_CreditCardInvalido")]
        public void CreditCard_NenhumDadoPreenchido_CreditCardInvalido()
        {
            // Arrange
            var creditCard = _fixture.CreditCardVazio();

            // Act
            var valido = creditCard.Valido();

            // Assert
            valido.Should().BeFalse(because: "deve possuir erros de preenchimento");
            creditCard.ErrorMessages.Should().HaveCount(4, because: "nenhum dos 4 campos obrigatórios foi informado ou estão incorretos.");

            creditCard.ErrorMessages.Should().Contain("O campo Nome Titular deve ser preenchido", because: "o campo Nome Titular é obrigatório e não foi preenchido.");
            creditCard.ErrorMessages.Should().Contain("O campo Número de cartão de crédito deve ser preenchido", because: "o campo Número de cartão de crédito é obrigatório e não foi preenchido.");
            creditCard.ErrorMessages.Should().Contain("O campo CVV deve ser preenchido", because: "o campo CVV é obrigatório e não foi preenchido.");
            creditCard.ErrorMessages.Should().Contain("O campo Validade deve ser preenchido", because: "o campo Data de Vencimento do cartão de crédito é obrigatório e não foi preenchido.");
        }

        [Fact]
        [Trait("CreditCard", "CreditCard_NumeroValidadeCVVInvalido_CreditCardInvalido")]
        public void CreditCard_NumeroValidadeCVVInvalido_CreditCardInvalido()
        {
            // Arrange
            var creditCard = _fixture.CreditCardNumeroValidadeCVVInvalido();

            // Act
            var valido = creditCard.Valido();

            // Assert
            valido.Should().BeFalse(because: "deve possuir erros de validação");
            creditCard.ErrorMessages.Should().HaveCount(3, because: "nenhum dos 3 campos obrigatórios foi informado ou estão incorretos.");

            creditCard.ErrorMessages.Should().Contain("Campo Número de cartão de crédito inválido", because: "o campo Nome Titular é obrigatório e não foi preenchido.");
            creditCard.ErrorMessages.Should().Contain("Campo CVV inválido", because: "o campo Nome Titular é obrigatório e não foi preenchido.");
            creditCard.ErrorMessages.Should().Contain("Campo Data de Vencimento do cartão de crédito inválido", because: "o campo Nome Titular é obrigatório e não foi preenchido.");
        }

        [Fact]
        [Trait("CreditCard", "CreditCard_ValidadeExpirada_CreditCardInvalido")]
        public void CreditCard_ValidadeExpirada_CreditCardInvalido()
        {
            // Arrange
            var creditCard = _fixture.CreditCardValidadeExpirada();

            // Act
            var valido = creditCard.Valido();

            // Assert
            valido.Should().BeFalse(because: "deve possuir erros de validação");
            creditCard.ErrorMessages.Should().HaveCount(1, because: "data de vencimento expirada");

            creditCard.ErrorMessages.Should().Contain("Cartão de Crédito com data expirada", because: "o campo Data de Vencimento do cartão de crédito está expirado.");
        }

        [Fact]
        [Trait("CreditCard", "CreditCard_NomeTitularMaxLength_CreditCardInvalido")]
        public void CreditCard_NomeTitularMaxLength_CreditCardInvalido()
        {
            // Arrange
            var creditCard = _fixture.CreditCardNomeTitularMaxLengthInvalido();

            // Act
            var valido = creditCard.Valido();

            // Assert
            valido.Should().BeFalse(because: "tamanho máximo de campos atingidos");
            creditCard.ErrorMessages.Should().HaveCount(1, because: "o preenchimento de 1 campo ultrapassou tamanho máximo permitido.");

            creditCard.ErrorMessages.Should().Contain("O campo Nome Titular deve possuir no máximo 150 caracteres", because: "o campo Nome Titular é obrigatório e não foi preenchido.");
        }

    }
}
