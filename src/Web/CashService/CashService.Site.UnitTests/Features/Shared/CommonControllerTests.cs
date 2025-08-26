using System.Net;
using AlphaPlus.CashService.Site.Features.Shared;
using AlphaPlus.CashService.Site.Features.Shared.Models;
using AlphaPlus.Core.Models;
using Fanex.Customer.Client.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using Xunit;

namespace AlphaPlus.CashService.Site.UnitTests.Features.Shared
{
    public class CommonControllerTests
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly Mock<IUserSession> _userSessionMock;
        private readonly Mock<IAntiforgery> _antiforgeryMock;
        private readonly Mock<ICustomerService> _customerServiceMock;
        private readonly Mock<HttpContext> _httpContextMock;
        private readonly CommonController _controller;
        private readonly string _apiBaseUrl = "https://api.test.com";

        public CommonControllerTests()
        {
            _configurationMock = new Mock<IConfiguration>();
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _userSessionMock = new Mock<IUserSession>();
            _antiforgeryMock = new Mock<IAntiforgery>();
            _customerServiceMock = new Mock<ICustomerService>();
            _httpContextMock = new Mock<HttpContext>();

            _configurationMock.Setup(x => x["CashService:APIUrl"]).Returns(_apiBaseUrl);

            _userSessionMock.Setup(x => x.CustId).Returns(123);
            _userSessionMock.Setup(x => x.IsInternal).Returns(false);
            _userSessionMock.Setup(x => x.IsSubAcc).Returns(false);
            _userSessionMock.Setup(x => x.SubPageAccess).Returns(new PageAccess { CashService = true });

            _controller = new CommonController(
                _configurationMock.Object,
                _httpClient,
                _userSessionMock.Object,
                _antiforgeryMock.Object,
                _customerServiceMock.Object);

            // Setup HttpContext
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = _httpContextMock.Object
            };
        }

        [Fact]
        public async Task GetSetupChecklist_WhenApiReturnsData_ReturnsJsonResult()
        {
            // Arrange
            var expectedChecklist = new SetupCheckListModel
            {
                HasDepositMethods = true,
                HasWithdrawalMethods = true,
                HasLineID = false
            };
            SetupHttpClientResponse(HttpStatusCode.OK, expectedChecklist);

            // Act
            var result = await _controller.GetSetupChecklist(CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            var resultValue = jsonResult.Value as SetupCheckListModel;
            
            resultValue.Should().NotBeNull();
            resultValue.HasDepositMethods.Should().Be(expectedChecklist.HasDepositMethods);
            resultValue.HasWithdrawalMethods.Should().Be(expectedChecklist.HasWithdrawalMethods);
            resultValue.HasLineID.Should().Be(expectedChecklist.HasLineID);
            resultValue.IsCompleteAll.Should().Be(expectedChecklist.IsCompleteAll);
            resultValue.IsCanEditCashService.Should().BeTrue();

            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => 
                    req.Method == HttpMethod.Get && 
                    req.RequestUri.ToString() == $"{_apiBaseUrl}/common/setup-checklist/123"),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task GetSetupChecklist_WhenApiReturnsEmptyData_ReturnsEmptyJsonResult()
        {
            // Arrange
            var expectedChecklist = new SetupCheckListModel
            {
                HasDepositMethods = false,
                HasWithdrawalMethods = false,
                HasLineID = false
            };
            SetupHttpClientResponse(HttpStatusCode.OK, expectedChecklist);

            // Act
            var result = await _controller.GetSetupChecklist(CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            var resultValue = jsonResult.Value as SetupCheckListModel;
            
            resultValue.Should().NotBeNull();
            resultValue.HasDepositMethods.Should().BeFalse();
            resultValue.HasWithdrawalMethods.Should().BeFalse();
            resultValue.HasLineID.Should().BeFalse();
            resultValue.IsCompleteAll.Should().BeFalse();
            resultValue.IsCanEditCashService.Should().BeTrue();
        }

        [Fact]
        public async Task GetSetupChecklist_WhenApiThrowsException_ThrowsException()
        {
            // Arrange
            SetupHttpClientException(new HttpRequestException("API Error"));

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() =>
                _controller.GetSetupChecklist(CancellationToken.None));
        }

        [Fact]
        public async Task GetSetupChecklist_WhenApiReturnsErrorStatusCode_ThrowsHttpRequestException()
        {
            // Arrange
            SetupHttpClientResponse(HttpStatusCode.InternalServerError, new { Error = "Server Error" });

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() =>
                _controller.GetSetupChecklist(CancellationToken.None));
        }

        [Fact]
        public async Task GetSetupChecklist_WhenApiReturnsNotFound_ThrowsHttpRequestException()
        {
            // Arrange
            SetupHttpClientResponse(HttpStatusCode.NotFound, new { Error = "Not Found" });

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() =>
                _controller.GetSetupChecklist(CancellationToken.None));
        }

        [Fact]
        public async Task GetSetupChecklist_WhenApiReturnsBadRequest_ThrowsHttpRequestException()
        {
            // Arrange
            SetupHttpClientResponse(HttpStatusCode.BadRequest, new { Error = "Bad Request" });

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() =>
                _controller.GetSetupChecklist(CancellationToken.None));
        }

        [Fact]
        public async Task GetSetupChecklist_WhenCancellationTokenIsCancelled_ThrowsOperationCanceledException()
        {
            // Arrange
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _controller.GetSetupChecklist(cancellationTokenSource.Token));
        }

        [Fact]
        public async Task GetSetupChecklist_WhenUserSessionHasDifferentCustId_UsesCorrectCustId()
        {
            // Arrange
            _userSessionMock.Setup(x => x.CustId).Returns(456);
            var expectedChecklist = new SetupCheckListModel { HasDepositMethods = true };
            SetupHttpClientResponse(HttpStatusCode.OK, expectedChecklist);

            // Act
            var result = await _controller.GetSetupChecklist(CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => 
                    req.RequestUri.ToString() == $"{_apiBaseUrl}/common/setup-checklist/456"),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task GetSetupChecklist_WhenApiBaseUrlIsDifferent_UsesCorrectUrl()
        {
            // Arrange
            var differentApiUrl = "https://different-api.test.com";
            _configurationMock.Setup(x => x["CashService:APIUrl"]).Returns(differentApiUrl);
            
            var controller = new CommonController(
                _configurationMock.Object,
                _httpClient,
                _userSessionMock.Object,
                _antiforgeryMock.Object,
                _customerServiceMock.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = _httpContextMock.Object
            };

            var expectedChecklist = new SetupCheckListModel { HasDepositMethods = true };
            SetupHttpClientResponse(HttpStatusCode.OK, expectedChecklist);

            // Act
            var result = await controller.GetSetupChecklist(CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => 
                    req.RequestUri.ToString() == $"{differentApiUrl}/common/setup-checklist/123"),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task GetSetupChecklist_WhenApiReturnsCompleteSetup_ReturnsCorrectJsonResult()
        {
            // Arrange
            var expectedChecklist = new SetupCheckListModel
            {
                HasDepositMethods = true,
                HasWithdrawalMethods = true,
                HasLineID = true
            };
            SetupHttpClientResponse(HttpStatusCode.OK, expectedChecklist);

            // Act
            var result = await _controller.GetSetupChecklist(CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            var resultValue = jsonResult.Value as SetupCheckListModel;
            
            resultValue.Should().NotBeNull();
            resultValue.HasDepositMethods.Should().BeTrue();
            resultValue.HasWithdrawalMethods.Should().BeTrue();
            resultValue.HasLineID.Should().BeTrue();
            resultValue.IsCompleteAll.Should().BeTrue();
            resultValue.IsCanEditCashService.Should().BeTrue();
        }

        [Fact]
        public async Task GetSetupChecklist_WhenUserIsInternal_IsCanEditCashServiceShouldBeTrue()
        {
            // Arrange
            _userSessionMock.Setup(x => x.IsInternal).Returns(true);
            var expectedChecklist = new SetupCheckListModel
            {
                HasDepositMethods = true,
                HasWithdrawalMethods = true,
                HasLineID = true
            };
            SetupHttpClientResponse(HttpStatusCode.OK, expectedChecklist);

            // Act
            var result = await _controller.GetSetupChecklist(CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            var resultValue = jsonResult.Value as SetupCheckListModel;
            
            resultValue.IsCanEditCashService.Should().BeTrue();
        }

        [Fact]
        public async Task GetSetupChecklist_WhenUserIsSubAccWithoutCashServiceAccess_IsCanEditCashServiceShouldBeFalse()
        {
            // Arrange
            _userSessionMock.Setup(x => x.IsSubAcc).Returns(true);
            _userSessionMock.Setup(x => x.SubPageAccess).Returns(new PageAccess { CashService = false });
            var expectedChecklist = new SetupCheckListModel
            {
                HasDepositMethods = true,
                HasWithdrawalMethods = true,
                HasLineID = true
            };
            SetupHttpClientResponse(HttpStatusCode.OK, expectedChecklist);

            // Act
            var result = await _controller.GetSetupChecklist(CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            var resultValue = jsonResult.Value as SetupCheckListModel;
            
            resultValue.IsCanEditCashService.Should().BeFalse();
        }

        [Fact]
        public async Task GetSetupChecklist_WhenUserIsSubAccWithCashServiceAccess_IsCanEditCashServiceShouldBeTrue()
        {
            // Arrange
            _userSessionMock.Setup(x => x.IsSubAcc).Returns(true);
            _userSessionMock.Setup(x => x.SubPageAccess).Returns(new PageAccess { CashService = true });
            var expectedChecklist = new SetupCheckListModel
            {
                HasDepositMethods = true,
                HasWithdrawalMethods = true,
                HasLineID = true
            };
            SetupHttpClientResponse(HttpStatusCode.OK, expectedChecklist);

            // Act
            var result = await _controller.GetSetupChecklist(CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            var resultValue = jsonResult.Value as SetupCheckListModel;
            
            resultValue.IsCanEditCashService.Should().BeTrue();
        }

        [Fact]
        public async Task GetSetupChecklist_WhenApiReturnsPartialSetup_ReturnsCorrectJsonResult()
        {
            // Arrange
            var expectedChecklist = new SetupCheckListModel
            {
                HasDepositMethods = true,
                HasWithdrawalMethods = false,
                HasLineID = true
            };
            SetupHttpClientResponse(HttpStatusCode.OK, expectedChecklist);

            // Act
            var result = await _controller.GetSetupChecklist(CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            var resultValue = jsonResult.Value as SetupCheckListModel;
            
            resultValue.Should().NotBeNull();
            resultValue.HasDepositMethods.Should().BeTrue();
            resultValue.HasWithdrawalMethods.Should().BeFalse();
            resultValue.HasLineID.Should().BeTrue();
            resultValue.IsCompleteAll.Should().BeFalse();
            resultValue.IsCanEditCashService.Should().BeTrue();
        }

        [Fact]
        public async Task GetSetupChecklist_WhenApiReturnsUnauthorized_ThrowsHttpRequestException()
        {
            // Arrange
            SetupHttpClientResponse(HttpStatusCode.Unauthorized, new { Error = "Unauthorized" });

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() =>
                _controller.GetSetupChecklist(CancellationToken.None));
        }

        [Fact]
        public async Task GetSetupChecklist_WhenApiReturnsForbidden_ThrowsHttpRequestException()
        {
            // Arrange
            SetupHttpClientResponse(HttpStatusCode.Forbidden, new { Error = "Forbidden" });

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() =>
                _controller.GetSetupChecklist(CancellationToken.None));
        }

        [Fact]
        public async Task GetSetupChecklist_WhenApiReturnsServiceUnavailable_ThrowsHttpRequestException()
        {
            // Arrange
            SetupHttpClientResponse(HttpStatusCode.ServiceUnavailable, new { Error = "Service Unavailable" });

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() =>
                _controller.GetSetupChecklist(CancellationToken.None));
        }

        [Fact]
        public async Task GetSetupChecklist_WhenApiReturnsGatewayTimeout_ThrowsHttpRequestException()
        {
            // Arrange
            SetupHttpClientResponse(HttpStatusCode.GatewayTimeout, new { Error = "Gateway Timeout" });

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() =>
                _controller.GetSetupChecklist(CancellationToken.None));
        }

        [Fact]
        public async Task GetSetupChecklist_WhenApiReturnsInvalidJson_ThrowsJsonException()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Invalid JSON content")
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act & Assert
            await Assert.ThrowsAsync<System.Text.Json.JsonException>(() =>
                _controller.GetSetupChecklist(CancellationToken.None));
        }

        [Fact]
        public async Task GetSetupChecklist_WhenApiReturnsNullContent_ReturnNull()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("null")
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var result = await _controller.GetSetupChecklist(CancellationToken.None);

            // Assert       
            Assert.Null((result as JsonResult).Value);
        }

        [Fact]
        public async Task GetSetupChecklist_WhenConfigurationIsNull_ThrowsNullReferenceException()
        {
            // Arrange
            _configurationMock.Setup(x => x["CashService:APIUrl"]).Returns((string)null);
            
            var controller = new CommonController(
                _configurationMock.Object,
                _httpClient,
                _userSessionMock.Object,
                _antiforgeryMock.Object,
                _customerServiceMock.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = _httpContextMock.Object
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                controller.GetSetupChecklist(CancellationToken.None));
        }

        [Fact]
        public async Task GetSetupChecklist_WhenConfigurationIsEmpty_ThrowsInvalidOperationException()
        {
            // Arrange
            _configurationMock.Setup(x => x["CashService:APIUrl"]).Returns(string.Empty);
            
            var controller = new CommonController(
                _configurationMock.Object,
                _httpClient,
                _userSessionMock.Object,
                _antiforgeryMock.Object,
                _customerServiceMock.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = _httpContextMock.Object
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                controller.GetSetupChecklist(CancellationToken.None));
        }

        [Theory]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        [InlineData(true, true, false)]
        [InlineData(true, false, true)]
        [InlineData(false, true, true)]
        public async Task GetSetupChecklist_WhenApiReturnsMixedSetup_ReturnsCorrectJsonResult(
            bool hasDepositMethods, bool hasWithdrawalMethods, bool hasLineID)
        {
            // Arrange
            var expectedChecklist = new SetupCheckListModel
            {
                HasDepositMethods = hasDepositMethods,
                HasWithdrawalMethods = hasWithdrawalMethods,
                HasLineID = hasLineID
            };
            SetupHttpClientResponse(HttpStatusCode.OK, expectedChecklist);

            // Act
            var result = await _controller.GetSetupChecklist(CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            var resultValue = jsonResult.Value as SetupCheckListModel;
            
            resultValue.Should().NotBeNull();
            resultValue.HasDepositMethods.Should().Be(hasDepositMethods);
            resultValue.HasWithdrawalMethods.Should().Be(hasWithdrawalMethods);
            resultValue.HasLineID.Should().Be(hasLineID);
            resultValue.IsCompleteAll.Should().Be(hasDepositMethods && hasWithdrawalMethods && hasLineID);
            resultValue.IsCanEditCashService.Should().BeTrue();
        }

        private void SetupHttpClientResponse<T>(HttpStatusCode statusCode, T content)
        {
            var response = new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(content))
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);
        }

        private void SetupHttpClientException(Exception exception)
        {
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(exception);
        }
    }
} 