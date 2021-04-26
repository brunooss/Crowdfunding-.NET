using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Vaquinha.Domain;

namespace Vaquinha.MVC.Controllers
{
    public class DoadoresController : Controller
    {
        private readonly IDonationService _donationService;

        public DoadoresController(IDonationService donationService)
        {
            _donationService = donationService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _donationService.RecuperarDoadoresAsync());
        }
    }
}