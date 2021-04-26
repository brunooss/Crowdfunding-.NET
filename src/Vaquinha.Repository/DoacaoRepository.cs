using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vaquinha.Domain;
using Vaquinha.Domain.Entities;
using Vaquinha.Repository.Context;

namespace Vaquinha.Repository
{
    public class DonationRepository : IDonationRepository
    {
        private readonly ILogger<DonationRepository> _logger;
        private readonly GloballAppConfig _globalSettings;
        private readonly VaquinhaOnlineDBContext _vaquinhaOnlineDBContext;

        public DonationRepository(GloballAppConfig globalSettings,
                                VaquinhaOnlineDBContext vaquinhaDbContext,
                                ILogger<DonationRepository> logger)
        {
            _globalSettings = globalSettings;
            _vaquinhaOnlineDBContext = vaquinhaDbContext;
            _logger = logger;
        }

        public async Task AdicionarAsync(Donation model)
        {
            await _vaquinhaOnlineDBContext.Doacoes.AddAsync(model);
            await _vaquinhaOnlineDBContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Donation>> RecuperarDoadoesAsync(int pageIndex = 0)
        {
            return await _vaquinhaOnlineDBContext.Doacoes.Include("DadosPessoais").ToListAsync();
        }
    }
}