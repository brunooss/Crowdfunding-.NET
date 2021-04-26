using System.Collections.Generic;
using System.Threading.Tasks;
using Vaquinha.Domain.ViewModels;

namespace Vaquinha.Domain
{
    public interface IDonationService
    {
        Task RealizarDonationAsync(DonationViewModel model);
        Task<IEnumerable<DoadorViewModel>> RecuperarDoadoresAsync(int pageIndex = 0);
    }
}