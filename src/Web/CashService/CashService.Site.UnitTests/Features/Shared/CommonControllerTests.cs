using System.Net;
using AlphaPlus.CashService.Site.Features.Shared;
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
            var expectedChecklist = new { 
                IsComplete = true, 
                Items = new[] { "Item1", "Item2" } 
            };
            SetupHttpClientResponse(HttpStatusCode.OK, expectedChecklist);

            // Act
            var result = await _controller.GetSetupChecklist(CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            AssertJsonResultEquals(jsonResult, expectedChecklist);
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
            var expectedChecklist = new { IsComplete = false, Items = new string[0] };
            SetupHttpClientResponse(HttpStatusCode.OK, expectedChecklist);

            // Act
            var result = await _controller.GetSetupChecklist(CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            AssertJsonResultEquals(jsonResult, expectedChecklist);
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
            var expectedChecklist = new { IsComplete = true };
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

            var expectedChecklist = new { IsComplete = true };
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
        public async Task GetSetupChecklist_WhenApiReturnsComplexObject_ReturnsCorrectJsonResult()
        {
            // Arrange
            var expectedChecklist = new
            {
                IsComplete = false,
                Items = new[]
                {
                    new { Id = 1, Name = "Profile Setup", IsCompleted = true },
                    new { Id = 2, Name = "Payment Method", IsCompleted = false },
                    new { Id = 3, Name = "Verification", IsCompleted = true }
                },
                TotalItems = 3,
                CompletedItems = 2
            };
            SetupHttpClientResponse(HttpStatusCode.OK, expectedChecklist);

            // Act
            var result = await _controller.GetSetupChecklist(CancellationToken.None);

            // Assert
            result.Should().BeOfType<JsonResult>();
            var jsonResult = result as JsonResult;
            AssertJsonResultEquals(jsonResult, expectedChecklist);
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

        private void AssertJsonResultEquals<T>(JsonResult jsonResult, T expectedValue)
        {
            jsonResult.Should().NotBeNull();
            jsonResult.Value.Should().NotBeNull();
            
            // Compare JSON strings to handle anonymous types and complex objects
            var actualJson = System.Text.Json.JsonSerializer.Serialize(jsonResult.Value);
            var expectedJson = System.Text.Json.JsonSerializer.Serialize(expectedValue);
            actualJson.Should().Be(expectedJson);
        }
    }
} 