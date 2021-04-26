using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vaquinha.Domain;
using Vaquinha.Domain.Entities;
using Vaquinha.Domain.ViewModels;

namespace Vaquinha.Service
{
    public class DonationService : IDonationService
    {
        private readonly IMapper _mapper;
        private readonly IDonationRepository _donationRepository;
        private readonly IDomainNotificationService _domainNotificationService;

        public DonationService(IMapper mapper,
                             IDonationRepository donationRepository,
                             IDomainNotificationService domainNotificationService)
        {
            _mapper = mapper;
            _donationRepository = donationRepository;
            _domainNotificationService = domainNotificationService;
        }

        public async Task RealizarDonationAsync(DonationViewModel model)
        {
            var entity = _mapper.Map<DonationViewModel, Donation>(model);

            entity.AtualizarDataCompra();

            if (entity.Valido())
            {
                await _donationRepository.AdicionarAsync(entity);
                return;
            }

            _domainNotificationService.Adicionar(entity);
        }

        public async Task<IEnumerable<DoadorViewModel>> RecuperarDoadoresAsync(int pageIndex = 0)
        {
            var doadores = await _donationRepository.RecuperarDoadoesAsync(pageIndex);
            return _mapper.Map<IEnumerable<Donation>, IEnumerable<DoadorViewModel>>(doadores);
        }
    }
}