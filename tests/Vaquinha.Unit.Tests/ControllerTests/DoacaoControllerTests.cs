using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using NToastNotify;
using Vaquinha.Domain;
using Vaquinha.Domain.Entities;
using Vaquinha.Domain.ViewModels;
using Vaquinha.MVC.Controllers;
using Vaquinha.Service;
using Vaquinha.Tests.Common.Fixtures;
using Xunit;

namespace Vaquinha.Unit.Tests.ControllerTests
{
    [Collection(nameof(DonationFixtureCollection))]
    public class DonationControllerTests : IClassFixture<DonationFixture>,
                                        IClassFixture<AddressFixture>,
                                        IClassFixture<CreditCardFixture>
    {
        private readonly Mock<IDonationRepository> _donationRepository = new Mock<IDonationRepository>();        
        private readonly Mock<GloballAppConfig> _globallAppConfig = new Mock<GloballAppConfig>();

        private readonly DonationFixture _donationFixture;
        private readonly AddressFixture _addressFixture;
        private readonly CreditCardFixture _creditCardFixture;

        private DoacoesController _donationController;
        private readonly IDonationService _donationService;

        private Mock<IMapper> _mapper;
        private Mock<IPaymentService> _polenService = new Mock<IPaymentService>();
        private Mock<ILogger<DoacoesController>> _logger = new Mock<ILogger<DoacoesController>>();

        private IDomainNotificationService _domainNotificationService = new DomainNotificationService();

        private Mock<IToastNotification> _toastNotification = new Mock<IToastNotification>();

        private readonly Donation _donationValida;
        private readonly DonationViewModel _donationModelValida;

        public DonationControllerTests(DonationFixture donationFixture, AddressFixture addressFixture, CreditCardFixture creditCardFixture)
        {
            _donationFixture = donationFixture;
            _addressFixture = addressFixture;
            _creditCardFixture = creditCardFixture;

            _mapper = new Mock<IMapper>();

            _donationValida = donationFixture.ValidDonation();
            _donationValida.AdicionarAddressCobranca(addressFixture.ValidAddress());
            _donationValida.AdicionarFormaPagamento(creditCardFixture.ValidCreditCard());

            _donationModelValida = donationFixture.DonationModelValida();
            _donationModelValida.AddressCobranca = addressFixture.AddressModelValido();
            _donationModelValida.FormaPagamento = creditCardFixture.CreditCardModelValido();

            _mapper.Setup(a => a.Map<DonationViewModel, Donation>(_donationModelValida)).Returns(_donationValida);

            _donationService = new DonationService(_mapper.Object, _donationRepository.Object, _domainNotificationService);
        }

        #region HTTPPOST

        [Trait("DonationController", "DonationController_Adicionar_RetornaDadosComSucesso")]
        [Fact]
        public void DonationController_Adicionar_RetornaDadosComSucesso()
        {
            // Arrange            
            _donationController = new DoacoesController(_donationService, _domainNotificationService, _toastNotification.Object);

            // Act
            var retorno = _donationController.Create(_donationModelValida);

            _mapper.Verify(a => a.Map<DonationViewModel, Donation>(_donationModelValida), Times.Once);
            _toastNotification.Verify(a => a.AddSuccessToastMessage(It.IsAny<string>(), It.IsAny<LibraryOptions>()), Times.Once);

            retorno.Should().BeOfType<RedirectToActionResult>();

            ((RedirectToActionResult)retorno).ActionName.Should().Be("Index");
            ((RedirectToActionResult)retorno).ControllerName.Should().Be("Home");
        }

        [Trait("DonationController", "DonationController_AdicionarDadosInvalidos_BadRequest")]
        [Fact]
        public void DonationController_AdicionarDadosInvalidos_BadRequest()
        {
            // Arrange          
            var donation = _donationFixture.DonationInvalida();
            var donationModelInvalida = new DonationViewModel();
            _mapper.Setup(a => a.Map<DonationViewModel, Donation>(donationModelInvalida)).Returns(donation);

            _donationController = new DoacoesController(_donationService, _domainNotificationService, _toastNotification.Object);

            // Act
            var retorno = _donationController.Create(donationModelInvalida);

            // Assert                   
            retorno.Should().BeOfType<ViewResult>();

            _mapper.Verify(a => a.Map<DonationViewModel, Donation>(donationModelInvalida), Times.Once);
            _donationRepository.Verify(a => a.AdicionarAsync(donation), Times.Never);
            _toastNotification.Verify(a => a.AddErrorToastMessage(It.IsAny<string>(), It.IsAny<LibraryOptions>()), Times.Once);

            var viewResult = ((ViewResult)retorno);

            viewResult.Model.Should().BeOfType<DonationViewModel>();

            ((DonationViewModel)viewResult.Model).Should().Be(donationModelInvalida);
        }

        #endregion
    }
}

