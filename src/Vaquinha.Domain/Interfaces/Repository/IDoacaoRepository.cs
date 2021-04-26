using System.Collections.Generic;
using System.Threading.Tasks;
using Vaquinha.Domain.Entities;

namespace Vaquinha.Domain
{
    public interface IDonationRepository
    {
        Task AdicionarAsync(Donation model);
        Task<IEnumerable<Donation>> RecuperarDoadoesAsync(int pageIndex = 0);
    }
}