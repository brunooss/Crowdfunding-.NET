namespace Vaquinha.Domain.ViewModels
{
    public class DonationViewModel
    {
        public decimal Valor { get; set; }

        public PessoaViewModel DadosPessoais { get; set; }
        public AddressViewModel AddressCobranca { get; set; }
        public CreditCardViewModel FormaPagamento { get; set; }
    }
}