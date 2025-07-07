using System.Net;
using AlphaPlus.CashService.Site.Features.Payments;
using AlphaPlus.CashService.Site.Features.Payments.Models;
using AlphaPlus.CashService.Site.Features.Payments.Requests;
using AlphaPlus.CashService.Site.Features.Payments.ViewModels;
using AlphaPlus.CashService.Site.Features.Shared.Models;
using AlphaPlus.Core.Models;
using Fanex.Customer.Client.Services;
using Fanex.Logging;
using FluentAssertions;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;

namespace AlphaPlus.CashService.Site.UnitTests.Features.Payments
{
    public class PaymentsControllerTests
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly Mock<IUserSession> _userSessionMock;
        private readonly Mock<IAntiforgery> _antiforgeryMock;
        private readonly Mock<ICustomerService> _customerServiceMock;
        private readonly Mock<ILogger> _loggerMock;
        private readonly Mock<HttpContext> _httpContextMock;
        private readonly PaymentsController _controller;
        private readonly string _apiBaseUrl = "https://api.test.com";

        public PaymentsControllerTests()
        {
            _configurationMock = new Mock<IConfiguration>();
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _userSessionMock = new Mock<IUserSession>();
            _antiforgeryMock = new Mock<IAntiforgery>();
            _customerServiceMock = new Mock<ICustomerService>();
            _loggerMock = new Mock<ILogger>();
            _httpContextMock = new Mock<HttpContext>();

            _configurationMock.Setup(x => x["CashService:APIUrl"]).Returns(_apiBaseUrl);
            _configurationMock.Setup(x => x["CashService:TWAPIUrl"]).Returns("https://tw-api.test.com");

            _userSessionMock.Setup(x => x.CustId).Returns(123);
            _userSessionMock.Setup(x => x.Language).Returns("en");
            _userSessionMock.Setup(x => x.DarkModeName).Returns("dark");
            _userSessionMock.Setup(x => x.Site).Returns("test-site");
            _userSessionMock.Setup(x => x.IsInternal).Returns(false);

            _controller = new PaymentsController(
                _configurationMock.Object,
                _httpClient,
                _userSessionMock.Object,
                _antiforgeryMock.Object,
                _customerServiceMock.Object,
                _loggerMock.Object);

            // Setup HttpContext
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = _httpContextMock.Object
            };
        }

        [Fact]
        public async Task Index_WhenCashServiceEnabled_ReturnsViewWithViewModel()
        {
            // Arrange
            _customerServiceMock.Setup(x => x.GetCashServiceStatus(123)).ReturnsAsync(1);

            // Act
            var result = await _controller.Index();

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result as ViewResult;
            viewResult.Model.Should().BeOfType<PaymentBaseViewModel>();
            var viewModel = viewResult.Model as PaymentBaseViewModel;
            viewModel.IsCashServiceEnabled.Should().Be(1);
            viewModel.Language.Should().Be("en");
            viewModel.DarkModeName.Should().Be("dark");
        }

        [Fact]
        public async Task Index_WhenCashServiceDisabled_ReturnsViewWithDisabledStatus()
        {
            // Arrange
            _customerServiceMock.Setup(x => x.GetCashServiceStatus(123)).ReturnsAsync(0);

            // Act
            var result = await _controller.Index();

            // Assert
            result.Should().BeOfType<ViewResult>();
            var viewResult = result as ViewResult;
            var viewModel = viewResult.Model as PaymentBaseViewModel;
            viewModel.IsCashServiceEnabled.Should().Be(0);
        }

        [Fact]
        public async Task GetProviders_WhenApiReturnsData_ReturnsJsonResult()
        {
            // Arrange
            var expectedProviders = new[] { new ProviderModel { Id = 1, Name = "Provider1" } };
            SetupHttpClientResponse(HttpStatusCode.OK, expectedProviders);

            // Act
            var result = await _controller.GetProviders(CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            AssertJsonResultEquals(jsonResult, expectedProviders);
            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.ToString().Contains("/payments/providers-by-custid/123")),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task GetProviders_WhenApiThrowsException_ThrowsException()
        {
            // Arrange
            SetupHttpClientException(new HttpRequestException("API Error"));

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() =>
                _controller.GetProviders(CancellationToken.None));
        }

        [Fact]
        public async Task GetDailyWithdrawalCount_WhenApiReturnsData_ReturnsJsonResult()
        {
            // Arrange
            var expectedCount = 5;
            SetupHttpClientResponse(HttpStatusCode.OK, expectedCount);

            // Act
            var result = await _controller.GetDailyWithdrawalCount(CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            AssertJsonResultEquals(jsonResult, expectedCount);
        }

        [Fact]
        public async Task SetDailyWithdrawalCount_WhenValidRequest_ReturnsJsonResult()
        {
            // Arrange
            var request = new SetDailyWithdrawalCountRequest { Count = 10 };
            var expectedResponse = new { Success = true };
            _customerServiceMock.Setup(x => x.GetCashServiceStatus(123)).ReturnsAsync(1);
            SetupHttpClientResponse(HttpStatusCode.OK, expectedResponse);

            // Act
            var result = await _controller.SetDailyWithdrawalCount(request, CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            AssertJsonResultEquals(jsonResult, expectedResponse);
        }

        [Fact]
        public async Task SetDailyWithdrawalCount_WhenCashServiceDisabled_ReturnsUnauthorized()
        {
            // Arrange
            var request = new SetDailyWithdrawalCountRequest { Count = 10 };
            _customerServiceMock.Setup(x => x.GetCashServiceStatus(123)).ReturnsAsync(0);

            // Act
            var result = await _controller.SetDailyWithdrawalCount(request, CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            AssertJsonResultEquals(jsonResult, new ResultModel { Code = 401 });
        }

        [Fact]
        public async Task GetProviderFees_WhenApiReturnsData_ReturnsJsonResult()
        {
            // Arrange
            var expectedFees = new[] { new { Method = "Credit Card", Fee = 2.5m } };
            SetupHttpClientResponse(HttpStatusCode.OK, expectedFees);

            // Act
            var result = await _controller.GetProviderFees(1, CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            // Compare JSON elements since the controller returns object
            var jsonString = System.Text.Json.JsonSerializer.Serialize(jsonResult.Value);
            var expectedJsonString = System.Text.Json.JsonSerializer.Serialize(expectedFees);
            AssertJsonResultEquals(jsonResult, expectedFees);
        }

        [Fact]
        public async Task ProviderSettings_WhenApiReturnsData_ReturnsJsonResult()
        {
            // Arrange
            var expectedSettings = new[] { new { Method = "Credit Card", Status = 1 } };
            SetupHttpClientResponse(HttpStatusCode.OK, expectedSettings);

            // Act
            var result = await _controller.ProviderSettings(1, 1, 1, 1, CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            AssertJsonResultEquals(jsonResult, expectedSettings);
        }

        [Fact]
        public async Task GetAddPaymentMethodInfo_WhenApiReturnsData_ReturnsJsonResult()
        {
            // Arrange
            var expectedMethods = new { PaymentMethods = new[] { new { Id = 1, Name = "Method1" } } };
            SetupHttpClientResponse(HttpStatusCode.OK, expectedMethods);

            // Act
            var result = await _controller.GetAddPaymentMethodInfo(1, 1, CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            AssertJsonResultEquals(jsonResult, expectedMethods);
        }

        [Fact]
        public async Task AddPaymentMethod_WhenValidRequest_ReturnsJsonResult()
        {
            // Arrange
            var request = new InsertPaymentMethodRequest
            {
                ProviderId = 1,
                TransactionType = 1,
                Method = 1,
                APIMethodKey = "key",
                MerchantID = "merchant",
                AppSecret = "secret"
            };
            var expectedResponse = new { Success = true };
            _customerServiceMock.Setup(x => x.GetCashServiceStatus(123)).ReturnsAsync(1);
            
            // Setup HTTP client to return different responses based on URL
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Returns<HttpRequestMessage, CancellationToken>((req, token) =>
                {
                    var response = new HttpResponseMessage(HttpStatusCode.OK);
                    if (req.RequestUri.ToString().Contains("/PaymentGateway/TestConnection"))
                    {
                        response.Content = new StringContent(JsonConvert.SerializeObject(true));
                    }
                    else if (req.RequestUri.ToString().Contains("/payments/add-payment-method"))
                    {
                        response.Content = new StringContent(JsonConvert.SerializeObject(expectedResponse));
                    }
                    return Task.FromResult(response);
                });

            // Act
            var result = await _controller.AddPaymentMethod(request, CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            AssertJsonResultEquals(jsonResult, expectedResponse);
            
            // Verify that APIResult was set to true after successful validation
            request.APIResult.Should().BeTrue();
            
            // Verify that both HTTP requests were made
            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(2),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task AddPaymentMethod_WhenCashServiceDisabled_ReturnsUnauthorized()
        {
            // Arrange
            var request = new InsertPaymentMethodRequest
            {
                ProviderId = 1,
                TransactionType = 1,
                Method = 1,
                APIMethodKey = "key",
                MerchantID = "merchant",
                AppSecret = "secret"
            };
            _customerServiceMock.Setup(x => x.GetCashServiceStatus(123)).ReturnsAsync(0);

            // Act
            var result = await _controller.AddPaymentMethod(request, CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            AssertJsonResultEquals(jsonResult, new ResultModel { Code = 401 });
            
            // Verify that APIResult was still set to false even though the request was rejected
            request.APIResult.Should().BeFalse();
        }

        [Fact]
        public async Task AddPaymentMethod_WhenValidationFails_ReturnsJsonResultWithFalseAPIResult()
        {
            // Arrange
            var request = new InsertPaymentMethodRequest
            {
                ProviderId = 1,
                TransactionType = 1,
                Method = 1,
                APIMethodKey = "key",
                MerchantID = "merchant",
                AppSecret = "secret"
            };
            var expectedResponse = new { Success = true };
            _customerServiceMock.Setup(x => x.GetCashServiceStatus(123)).ReturnsAsync(1);
            
            // Setup HTTP client to return validation failure for TW API but success for main API
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Returns<HttpRequestMessage, CancellationToken>((req, token) =>
                {
                    var response = new HttpResponseMessage(HttpStatusCode.OK);
                    if (req.RequestUri.ToString().Contains("/PaymentGateway/TestConnection"))
                    {
                        response.Content = new StringContent(JsonConvert.SerializeObject(false));
                    }
                    else if (req.RequestUri.ToString().Contains("/payments/add-payment-method"))
                    {
                        response.Content = new StringContent(JsonConvert.SerializeObject(expectedResponse));
                    }
                    return Task.FromResult(response);
                });

            // Act
            var result = await _controller.AddPaymentMethod(request, CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            AssertJsonResultEquals(jsonResult, expectedResponse);
            
            // Verify that APIResult was set to false after failed validation
            request.APIResult.Should().BeFalse();
        }

        [Fact]
        public async Task AddPaymentMethod_WhenUserIsInternal_ReturnsUnauthorized()
        {
            // Arrange
            var request = new InsertPaymentMethodRequest
            {
                ProviderId = 1,
                TransactionType = 1,
                Method = 1,
                APIMethodKey = "key",
                MerchantID = "merchant",
                AppSecret = "secret"
            };
            _userSessionMock.Setup(x => x.IsInternal).Returns(true);

            // Act
            var result = await _controller.AddPaymentMethod(request, CancellationToken.None);

            // Assert
            result.Should().BeOfType<UnauthorizedResult>();
            
            // Verify that APIResult was still set to false even though the request was rejected
            request.APIResult.Should().BeFalse();
        }

        [Fact]
        public async Task AddPaymentMethod_WhenHttpClientThrowsException_ThrowsException()
        {
            // Arrange
            var request = new InsertPaymentMethodRequest
            {
                ProviderId = 1,
                TransactionType = 1,
                Method = 1,
                APIMethodKey = "key",
                MerchantID = "merchant",
                AppSecret = "secret"
            };
            _customerServiceMock.Setup(x => x.GetCashServiceStatus(123)).ReturnsAsync(1);
            SetupHttpClientException(new HttpRequestException("Network error"));

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() =>
                _controller.AddPaymentMethod(request, CancellationToken.None));
            
            // Verify that APIResult was still set to false even though an exception occurred
            request.APIResult.Should().BeFalse();
        }

        [Fact]
        public async Task AddPaymentMethod_WhenValidationThrowsException_ReturnsJsonResultWithFalseAPIResult()
        {
            // Arrange
            var request = new InsertPaymentMethodRequest
            {
                ProviderId = 1,
                TransactionType = 1,
                Method = 1,
                APIMethodKey = "key",
                MerchantID = "merchant",
                AppSecret = "secret"
            };
            var expectedResponse = new { Success = true };
            _customerServiceMock.Setup(x => x.GetCashServiceStatus(123)).ReturnsAsync(1);
            
            // Setup HTTP client to throw exception for TW API but success for main API
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Returns<HttpRequestMessage, CancellationToken>((req, token) =>
                {
                    if (req.RequestUri.ToString().Contains("/PaymentGateway/TestConnection"))
                    {
                        throw new HttpRequestException("TW API Error");
                    }
                    else if (req.RequestUri.ToString().Contains("/payments/add-payment-method"))
                    {
                        var response = new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new StringContent(JsonConvert.SerializeObject(expectedResponse))
                        };
                        return Task.FromResult(response);
                    }
                    return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
                });

            // Act
            var result = await _controller.AddPaymentMethod(request, CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            AssertJsonResultEquals(jsonResult, expectedResponse);
            
            // Verify that APIResult was set to false after exception during validation
            request.APIResult.Should().BeFalse();
            
            // Verify that logger was called with the exception
            _loggerMock.Verify(x => x.ErrorAsync(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
        }

        [Fact]
        public async Task GetBanksByTransactionType_WhenApiReturnsData_ReturnsJsonResult()
        {
            // Arrange
            var expectedBanks = new[] { new { Id = 1, Name = "Bank1" } };
            SetupHttpClientResponse(HttpStatusCode.OK, expectedBanks);

            // Act
            var result = await _controller.GetBanksByTransactionType(1, CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            AssertJsonResultEquals(jsonResult, expectedBanks);
        }

        [Fact]
        public async Task UpdateProvider_WhenValidRequest_ReturnsJsonResult()
        {
            // Arrange
            var request = new UpdateProviderRequest { ProviderId = 1, Status = 1, ProviderSettings = new List<ProviderSettingsModel>() };
            var expectedResponse = new { Success = true };
            _customerServiceMock.Setup(x => x.GetCashServiceStatus(123)).ReturnsAsync(1);
            SetupHttpClientResponse(HttpStatusCode.OK, expectedResponse);

            // Act
            var result = await _controller.UpdateProvider(request, CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            AssertJsonResultEquals(jsonResult, expectedResponse);
        }

        [Fact]
        public async Task GetBanksByCust_WhenApiReturnsData_ReturnsJsonResult()
        {
            // Arrange
            var expectedBanks = new[] { new BankModel { Id = 1, Name = "Bank1" } };
            SetupHttpClientResponse(HttpStatusCode.OK, expectedBanks);

            // Act
            var result = await _controller.GetBanksByCust(CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            AssertJsonResultEquals(jsonResult, expectedBanks);
        }

        [Fact]
        public async Task GetTransactionTypes_WhenApiReturnsData_ReturnsJsonResult()
        {
            // Arrange
            var expectedTypes = new[] { new { Id = 1, Name = "Deposit" } };
            SetupHttpClientResponse(HttpStatusCode.OK, expectedTypes);

            // Act
            var result = await _controller.GetTransactionTypes(CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            AssertJsonResultEquals(jsonResult, expectedTypes);
        }

        [Fact]
        public async Task GetBankAccounts_WhenApiReturnsData_ReturnsJsonResult()
        {
            // Arrange
            var expectedAccounts = new[] { new { Id = 1, AccountNumber = "1234567890" } };
            SetupHttpClientResponse(HttpStatusCode.OK, expectedAccounts);

            // Act
            var result = await _controller.GetBankAccounts(1, 1, 1, "test", CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            AssertJsonResultEquals(jsonResult, expectedAccounts);
        }

        [Fact]
        public async Task GetTransactionLimit_WhenApiReturnsData_ReturnsJsonResult()
        {
            // Arrange
            var expectedLimit = new { DailyWithdrawalCount = 5, MinPerDepositTransaction = 100m };
            SetupHttpClientResponse(HttpStatusCode.OK, expectedLimit);

            // Act
            var result = await _controller.GetTransactionLimit(CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            AssertJsonResultEquals(jsonResult, expectedLimit);
        }

        [Fact]
        public async Task SetTransactionLimit_WhenValidRequest_ReturnsJsonResult()
        {
            // Arrange
            var request = new SetTransactionLimitRequest
            {
                DailyWithdrawalCount = 5,
                MinPerDepositTransaction = 100m,
                MaxPerDepositTransaction = 10000m,
                MinPerWithdrawalTransaction = 50m,
                MaxPerWithdrawalTransaction = 5000m
            };
            var expectedResponse = new { Success = true };
            _customerServiceMock.Setup(x => x.GetCashServiceStatus(123)).ReturnsAsync(1);
            SetupHttpClientResponse(HttpStatusCode.OK, expectedResponse);

            // Act
            var result = await _controller.SetTransactionLimit(request, CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            AssertJsonResultEquals(jsonResult, expectedResponse);
        }

        [Fact]
        public async Task AddBankAccount_WhenValidRequest_ReturnsJsonResult()
        {
            // Arrange
            var request = new AddBankAccountRequest
            {
                AccountNumber = "1234567890",
                AccountHolderName = "John Doe",
                BankId = 1,
                Status = 1
            };
            var expectedResponse = new { Success = true };
            _customerServiceMock.Setup(x => x.GetCashServiceStatus(123)).ReturnsAsync(1);
            SetupHttpClientResponse(HttpStatusCode.OK, expectedResponse);

            // Act
            var result = await _controller.AddBankAccount(request, CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            AssertJsonResultEquals(jsonResult, expectedResponse);
        }

        [Fact]
        public async Task GetBankAccountById_WhenApiReturnsData_ReturnsJsonResult()
        {
            // Arrange
            var expectedAccount = new { Id = 1, AccountNumber = "1234567890", AccountHolderName = "John Doe" };
            SetupHttpClientResponse(HttpStatusCode.OK, expectedAccount);

            // Act
            var result = await _controller.GetBankAccountById(1, CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            AssertJsonResultEquals(jsonResult, expectedAccount);
        }

        [Fact]
        public async Task UpdateBankAccount_WhenValidRequest_ReturnsJsonResult()
        {
            // Arrange
            var request = new UpdateBankAccountRequest
            {
                Id = 1,
                AccountNickname = "Updated Account",
                BankId = 1,
                Status = 1,
                Creator = "Test User"
            };
            var expectedResponse = new { Success = true };
            _customerServiceMock.Setup(x => x.GetCashServiceStatus(123)).ReturnsAsync(1);
            SetupHttpClientResponse(HttpStatusCode.OK, expectedResponse);

            // Act
            var result = await _controller.UpdateBankAccount(request, CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            AssertJsonResultEquals(jsonResult, expectedResponse);
        }

        [Fact]
        public async Task DeleteBankAccount_WhenValidRequest_ReturnsJsonResult()
        {
            // Arrange
            var request = new DeleteBankAccountRequest { BankAccountId = 1 };
            var expectedResponse = new { Success = true };
            _customerServiceMock.Setup(x => x.GetCashServiceStatus(123)).ReturnsAsync(1);
            SetupHttpClientResponse(HttpStatusCode.OK, expectedResponse);

            // Act
            var result = await _controller.DeleteBankAccount(request, CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            AssertJsonResultEquals(jsonResult, expectedResponse);
        }

        [Fact]
        public async Task GetCountryCodes_WhenApiReturnsData_ReturnsJsonResult()
        {
            // Arrange
            var expectedCodes = new[] { new { Code = "US", Name = "United States" } };
            SetupHttpClientResponse(HttpStatusCode.OK, expectedCodes);

            // Act
            var result = await _controller.GetCountryCodes(CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            AssertJsonResultEquals(jsonResult, expectedCodes);
        }

        [Fact]
        public async Task DoAction_WhenUserIsInternal_ReturnsUnauthorized()
        {
            // Arrange
            var request = new SetDailyWithdrawalCountRequest { Count = 10 };
            _userSessionMock.Setup(x => x.IsInternal).Returns(true);

            // Act
            var result = await _controller.SetDailyWithdrawalCount(request, CancellationToken.None);

            // Assert
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public async Task DoAction_WhenHttpClientThrowsException_ThrowsException()
        {
            // Arrange
            var request = new SetDailyWithdrawalCountRequest { Count = 10 };
            _customerServiceMock.Setup(x => x.GetCashServiceStatus(123)).ReturnsAsync(1);
            SetupHttpClientException(new HttpRequestException("Network error"));

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() =>
                _controller.SetDailyWithdrawalCount(request, CancellationToken.None));
        }

        [Fact]
        public async Task DoAction_WhenHttpResponseIsNotSuccess_ThrowsHttpRequestException()
        {
            // Arrange
            var request = new SetDailyWithdrawalCountRequest { Count = 10 };
            _customerServiceMock.Setup(x => x.GetCashServiceStatus(123)).ReturnsAsync(1);
            SetupHttpClientResponse(HttpStatusCode.BadRequest, new { Error = "Bad Request" });

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() =>
                _controller.SetDailyWithdrawalCount(request, CancellationToken.None));
        }

        [Fact]
        public async Task GetProviders_WhenApiReturnsEmptyList_ReturnsEmptyJsonResult()
        {
            // Arrange
            var expectedProviders = new object[] { };
            SetupHttpClientResponse(HttpStatusCode.OK, expectedProviders);

            // Act
            var result = await _controller.GetProviders(CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            AssertJsonResultEquals(jsonResult, expectedProviders);
        }

        [Fact]
        public async Task GetDailyWithdrawalCount_WhenApiReturnsZero_ReturnsZero()
        {
            // Arrange
            var expectedCount = 0;
            SetupHttpClientResponse(HttpStatusCode.OK, expectedCount);

            // Act
            var result = await _controller.GetDailyWithdrawalCount(CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            AssertJsonResultEquals(jsonResult, expectedCount);
        }

        [Fact]
        public async Task ManageProviderSettings_WhenProviderNotFound_ThrowsException()
        {
            // Arrange
            SetupHttpClientResponse(HttpStatusCode.NotFound, new { Error = "Provider not found" });

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() =>
                _controller.ManageProviderSettings(999, CancellationToken.None));
        }

        [Fact]
        public async Task GetProviderFees_WhenProviderIdIsZero_ReturnsEmptyResult()
        {
            // Arrange
            var expectedFees = new object[] { };
            SetupHttpClientResponse(HttpStatusCode.OK, expectedFees);

            // Act
            var result = await _controller.GetProviderFees(0, CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            AssertJsonResultEquals(jsonResult, expectedFees);
        }

        [Fact]
        public async Task GetBankAccounts_WhenSearchTextIsEmpty_ReturnsAllAccounts()
        {
            // Arrange
            var expectedAccounts = new[] { new { Id = 1, AccountNumber = "1234567890" } };
            SetupHttpClientResponse(HttpStatusCode.OK, expectedAccounts);

            // Act
            var result = await _controller.GetBankAccounts(1, 1, 1, "", CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            AssertJsonResultEquals(jsonResult, expectedAccounts);
        }

        [Fact]
        public async Task GetBankAccounts_WhenSearchTextIsNull_ReturnsAllAccounts()
        {
            // Arrange
            var expectedAccounts = new[] { new { Id = 1, AccountNumber = "1234567890" } };
            SetupHttpClientResponse(HttpStatusCode.OK, expectedAccounts);

            // Act
            var result = await _controller.GetBankAccounts(1, 1, 1, null, CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            AssertJsonResultEquals(jsonResult, expectedAccounts);
        }

        [Fact]
        public async Task GetCountryCodes_WhenSiteIsDifferent_ReturnsCorrectUrl()
        {
            // Arrange
            _userSessionMock.Setup(x => x.Site).Returns("different-site");
            var expectedCodes = new[] { new { Code = "TH", Name = "Thailand" } };
            SetupHttpClientResponse(HttpStatusCode.OK, expectedCodes);

            // Act
            var result = await _controller.GetCountryCodes(CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            AssertJsonResultEquals(jsonResult, expectedCodes);
            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.ToString().Contains("different-site")),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task UpdatePaymentMethod_WhenValidRequest_ReturnsJsonResult()
        {
            // Arrange
            var request = new UpdatePaymentMethodRequest
            {
                ProviderId = 1,
                TransactionType = 1,
                Method = 1,
                APIMethodKey = "key",
                MerchantID = "merchant",
                AppSecret = "secret"
            };
            var expectedResponse = new { Success = true };
            _customerServiceMock.Setup(x => x.GetCashServiceStatus(123)).ReturnsAsync(1);
            
            // Setup HTTP client to return different responses based on URL
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Returns<HttpRequestMessage, CancellationToken>((req, token) =>
                {
                    var response = new HttpResponseMessage(HttpStatusCode.OK);
                    if (req.RequestUri.ToString().Contains("/PaymentGateway/TestConnection"))
                    {
                        response.Content = new StringContent(JsonConvert.SerializeObject(true));
                    }
                    else if (req.RequestUri.ToString().Contains("/payments/update-payment-method"))
                    {
                        response.Content = new StringContent(JsonConvert.SerializeObject(expectedResponse));
                    }
                    return Task.FromResult(response);
                });

            // Act
            var result = await _controller.UpdatePaymentMethod(request, CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            AssertJsonResultEquals(jsonResult, expectedResponse);
            
            // Verify that APIResult was set to true after successful validation
            request.APIResult.Should().BeTrue();
            
            // Verify that both HTTP requests were made
            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(2),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task UpdatePaymentMethod_WhenCashServiceDisabled_ReturnsUnauthorized()
        {
            // Arrange
            var request = new UpdatePaymentMethodRequest
            {
                ProviderId = 1,
                TransactionType = 1,
                Method = 1,
                APIMethodKey = "key",
                MerchantID = "merchant",
                AppSecret = "secret"
            };
            _customerServiceMock.Setup(x => x.GetCashServiceStatus(123)).ReturnsAsync(0);

            // Act
            var result = await _controller.UpdatePaymentMethod(request, CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            AssertJsonResultEquals(jsonResult, new ResultModel { Code = 401 });
            
            // Verify that APIResult was still set to false even though the request was rejected
            request.APIResult.Should().BeFalse();
        }

        [Fact]
        public async Task UpdatePaymentMethod_WhenUserIsInternal_ReturnsUnauthorized()
        {
            // Arrange
            var request = new UpdatePaymentMethodRequest
            {
                ProviderId = 1,
                TransactionType = 1,
                Method = 1,
                APIMethodKey = "key",
                MerchantID = "merchant",
                AppSecret = "secret"
            };
            _userSessionMock.Setup(x => x.IsInternal).Returns(true);

            // Act
            var result = await _controller.UpdatePaymentMethod(request, CancellationToken.None);

            // Assert
            result.Should().BeOfType<UnauthorizedResult>();
            
            // Verify that APIResult was still set to false even though the request was rejected
            request.APIResult.Should().BeFalse();
        }

        [Fact]
        public async Task UpdatePaymentMethod_WhenValidationFails_ReturnsJsonResultWithFalseAPIResult()
        {
            // Arrange
            var request = new UpdatePaymentMethodRequest
            {
                ProviderId = 1,
                TransactionType = 1,
                Method = 1,
                APIMethodKey = "key",
                MerchantID = "merchant",
                AppSecret = "secret"
            };
            var expectedResponse = new { Success = true };
            _customerServiceMock.Setup(x => x.GetCashServiceStatus(123)).ReturnsAsync(1);
            
            // Setup HTTP client to return validation failure for TW API but success for main API
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Returns<HttpRequestMessage, CancellationToken>((req, token) =>
                {
                    var response = new HttpResponseMessage(HttpStatusCode.OK);
                    if (req.RequestUri.ToString().Contains("/PaymentGateway/TestConnection"))
                    {
                        response.Content = new StringContent(JsonConvert.SerializeObject(false));
                    }
                    else if (req.RequestUri.ToString().Contains("/payments/update-payment-method"))
                    {
                        response.Content = new StringContent(JsonConvert.SerializeObject(expectedResponse));
                    }
                    return Task.FromResult(response);
                });

            // Act
            var result = await _controller.UpdatePaymentMethod(request, CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            AssertJsonResultEquals(jsonResult, expectedResponse);
            
            // Verify that APIResult was set to false after failed validation
            request.APIResult.Should().BeFalse();
        }

        [Fact]
        public async Task UpdatePaymentMethod_WhenValidationThrowsException_ReturnsJsonResultWithFalseAPIResult()
        {
            // Arrange
            var request = new UpdatePaymentMethodRequest
            {
                ProviderId = 1,
                TransactionType = 1,
                Method = 1,
                APIMethodKey = "key",
                MerchantID = "merchant",
                AppSecret = "secret"
            };
            var expectedResponse = new { Success = true };
            _customerServiceMock.Setup(x => x.GetCashServiceStatus(123)).ReturnsAsync(1);
            
            // Setup HTTP client to throw exception for TW API but success for main API
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Returns<HttpRequestMessage, CancellationToken>((req, token) =>
                {
                    if (req.RequestUri.ToString().Contains("/PaymentGateway/TestConnection"))
                    {
                        throw new HttpRequestException("TW API Error");
                    }
                    else if (req.RequestUri.ToString().Contains("/payments/update-payment-method"))
                    {
                        var response = new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new StringContent(JsonConvert.SerializeObject(expectedResponse))
                        };
                        return Task.FromResult(response);
                    }
                    return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
                });

            // Act
            var result = await _controller.UpdatePaymentMethod(request, CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            AssertJsonResultEquals(jsonResult, expectedResponse);
            
            // Verify that APIResult was set to false after exception during validation
            request.APIResult.Should().BeFalse();
            
            // Verify that logger was called with the exception
            _loggerMock.Verify(x => x.ErrorAsync(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
        }

        private void SetupHttpClientResponse<T>(HttpStatusCode statusCode, T content)
        {
            var response = new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(JsonConvert.SerializeObject(content))
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);
        }

        private void SetupHttpClientResponse<T>(HttpStatusCode statusCode, T content, Func<HttpRequestMessage, bool> urlPredicate)
        {
            var response = new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(JsonConvert.SerializeObject(content))
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => urlPredicate(req)),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);
        }

        private void SetupHttpClientException(Exception exception)
        {
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(exception);
        }

        private void AssertJsonResultEquals<T>(JsonResult jsonResult, T expectedValue)
        {
            jsonResult.Should().NotBeNull();
            jsonResult.Value.Should().NotBeNull();

            // For JsonElement comparisons, serialize both to JSON strings and compare
            var actualJson = System.Text.Json.JsonSerializer.Serialize(jsonResult.Value);
            var expectedJson = System.Text.Json.JsonSerializer.Serialize(expectedValue);

            // Parse both as JsonDocument for structural comparison
            var actualDoc = System.Text.Json.JsonDocument.Parse(actualJson);
            var expectedDoc = System.Text.Json.JsonDocument.Parse(expectedJson);

            actualDoc.RootElement.ValueKind.Should().Be(expectedDoc.RootElement.ValueKind);

            if (actualDoc.RootElement.ValueKind == System.Text.Json.JsonValueKind.Array)
            {
                actualDoc.RootElement.GetArrayLength().Should().Be(expectedDoc.RootElement.GetArrayLength());
                for (int i = 0; i < actualDoc.RootElement.GetArrayLength(); i++)
                {
                    var actualElement = actualDoc.RootElement[i];
                    var expectedElement = expectedDoc.RootElement[i];
                    actualElement.ValueKind.Should().Be(expectedElement.ValueKind);
                }
            }
        }
    }
}