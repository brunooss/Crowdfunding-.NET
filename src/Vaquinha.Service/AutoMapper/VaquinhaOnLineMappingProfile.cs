using AutoMapper;
using System;
using Vaquinha.Domain.Entities;
using Vaquinha.Domain.ViewModels;

namespace Vaquinha.Service.AutoMapper
{
    public class VaquinhaOnLineMappingProfile : Profile
    {
        public VaquinhaOnLineMappingProfile()
        {   
            CreateMap<Pessoa, PessoaViewModel>();
            CreateMap<Donation, DonationViewModel>();
            CreateMap<Address, AddressViewModel>();
            CreateMap<Causa, CausaViewModel>();
            CreateMap<CreditCard, CreditCardViewModel>();

            CreateMap<Donation, DoadorViewModel>()
                .ForMember(dest => dest.Nome, m => m.MapFrom(src => src.DadosPessoais.Nome))
                .ForMember(dest => dest.Anonima, m => m.MapFrom(src => src.DadosPessoais.Anonima))
                .ForMember(dest => dest.MensagemApoio, m => m.MapFrom(src => src.DadosPessoais.MensagemApoio))
                .ForMember(dest => dest.Valor, m => m.MapFrom(src => src.Valor))             
                .ForMember(dest => dest.DataHora, m => m.MapFrom(src => src.DataHora));

            CreateMap<PessoaViewModel, Pessoa>()
                .ConstructUsing(src => new Pessoa(Guid.NewGuid(), src.Nome, src.Email, src.Anonima, src.MensagemApoio));

            CreateMap<CreditCardViewModel, CreditCard>()
                .ConstructUsing(src => new CreditCard(src.NomeTitular, src.NumeroCreditCard, src.Validade, src.CVV));

            CreateMap<CausaViewModel, Causa>()
                .ConstructUsing(src => new Causa(Guid.NewGuid(), src.Nome, src.Cidade, src.Estado));

            CreateMap<AddressViewModel, Address>()
                .ConstructUsing(src => new Address(Guid.NewGuid(), src.CEP, src.TextoAddress, src.Complemento, src.Cidade, src.Estado, src.Telefone, src.Numero));

            CreateMap<DonationViewModel, Donation>()
                .ForCtorParam("valor", opt => opt.MapFrom(src => src.Valor))
                .ForCtorParam("dadosPessoais", opt => opt.MapFrom(src => src.DadosPessoais))
                .ForCtorParam("formaPagamento", opt => opt.MapFrom(src => src.FormaPagamento))
                .ForCtorParam("addressCobranca", opt => opt.MapFrom(src => src.AddressCobranca));
        }
    }
}