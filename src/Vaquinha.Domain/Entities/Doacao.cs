using FluentValidation;
using System;
using Vaquinha.Domain.Base;

namespace Vaquinha.Domain.Entities
{
    public class Donation : Entity
    {
        private Donation() { }

        public Donation(Guid id, Guid dadosPessoaisId, Guid addressCobrancaId, double valor,
                      Pessoa dadosPessoais, CreditCard formaPagamento, Address addressCobranca)
        {
            Id = id;
            DataHora = DateTime.Now;

            DadosPessoaisId = dadosPessoaisId;
            AddressCobrancaId = addressCobrancaId;

            Valor = valor;

            DadosPessoais = dadosPessoais;
            FormaPagamento = formaPagamento;
            AddressCobranca = addressCobranca;
        }

        public double Valor { get; private set; }

        public Guid DadosPessoaisId { get; private set; }
        public Guid AddressCobrancaId { get; private set; }

        public DateTime DataHora { get; private set; }

        public Pessoa DadosPessoais { get; private set; }
        public Address AddressCobranca { get; private set; }
        public CreditCard FormaPagamento { get; private set; }

        public void AtualizarDataCompra()
        {
            DataHora = DateTime.Now;
        }

        public void AdicionarPessoa(Pessoa pessoa)
        {
            DadosPessoais = pessoa;
        }

        public void AdicionarAddressCobranca(Address address) {
            AddressCobranca = address;
        }
        public void AdicionarFormaPagamento(CreditCard formaPagamento) {
            FormaPagamento = formaPagamento;
        }

        public override bool Valido()
        {
            ValidationResult = new ValidDonationcao().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ValidDonationcao : AbstractValidator<Donation>
    {
        public ValidDonationcao()
        {
            RuleFor(a => a.Valor)
                .GreaterThanOrEqualTo(5).WithMessage("Valor mínimo de doação é de R$ 5,00")
                .LessThanOrEqualTo(4500).WithMessage("Valor máximo para a doação é de R$4.500,00");

            RuleFor(a => a.DadosPessoais).NotNull().WithMessage("Os Dados Pessoais são obrigatórios").SetValidator(new PessoaValidacao());
            RuleFor(a => a.AddressCobranca).NotNull().WithMessage("O Endereço de Cobrança é obtigatório.").SetValidator(new AddressValidacao());
            RuleFor(a => a.FormaPagamento).NotNull().WithMessage("A Forma de Pagamento é obtigatória.").SetValidator(new CreditCardValidacao());
        }
    }
}