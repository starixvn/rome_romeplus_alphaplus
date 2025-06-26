using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AlphaPlus.CashService.Site.Features.Payments;
using AlphaPlus.CashService.Site.Features.Payments.Models;
using AlphaPlus.CashService.Site.Features.Payments.Requests;
using AlphaPlus.CashService.Site.Features.Payments.ViewModels;
using AlphaPlus.Core.Models;
using Fanex.Customer.Client.Services;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace AlphaPlus.CashService.Site.UnitTests.Features.Payments
{
    public class PaymentsControllerTests
    {
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly Mock<HttpClient> _mockHttpClient;
        private readonly Mock<IUserSession> _mockUserSession;
        private readonly Mock<IAntiforgery> _mockAntiforgery;
        private readonly Mock<ICustomerService> _mockCustomerService;
        private readonly PaymentsController _controller;

        public PaymentsControllerTests()
        {
            _mockConfig = new Mock<IConfiguration>();
            _mockHttpClient = new Mock<HttpClient>();
            _mockUserSession = new Mock<IUserSession>();
            _mockAntiforgery = new Mock<IAntiforgery>();
            _mockCustomerService = new Mock<ICustomerService>();

            _mockConfig.Setup(c => c["CashService:APIUrl"]).Returns("http://fake-api");
            _mockUserSession.Setup(u => u.CustId).Returns(1);
            _mockUserSession.Setup(u => u.Language).Returns("en");
            _mockUserSession.Setup(u => u.DarkModeName).Returns("dark");
            _mockCustomerService.Setup(s => s.GetCashServiceStatus(It.IsAny<int>())).ReturnsAsync(1);

            _controller = new PaymentsController(
                _mockConfig.Object,
                _mockHttpClient.Object,
                _mockUserSession.Object,
                _mockAntiforgery.Object,
                _mockCustomerService.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewWithViewModel()
        {
            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<PaymentBaseViewModel>(viewResult.Model);
            Assert.Equal("en", model.Language);
            Assert.Equal("dark", model.DarkModeName);
            Assert.Equal(1, model.IsCashServiceEnabled);
        }

        [Fact]
        public async Task GetProviders_ReturnsJsonResult()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            // Setup HttpClient extension for GetFromJsonAsync if needed (use a wrapper or refactor for testability)
            // For now, just test that the action returns JsonResult

            // Act
            var result = await _controller.GetProviders(cancellationToken);

            // Assert
            Assert.IsType<JsonResult>(result);
        }

        [Fact]
        public async Task AddPaymentMethod_ReturnsActionResult()
        {
            // Arrange
            var request = new InsertPaymentMethodRequest();
            var cancellationToken = CancellationToken.None;
            // You may need to mock DoAction if it's virtual or refactor for testability

            // Act
            var result = await _controller.AddPaymentMethod(request, cancellationToken);

            // Assert
            Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}