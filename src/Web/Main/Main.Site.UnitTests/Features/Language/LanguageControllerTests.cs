using AlphaPlus.Core.Models;
using AlphaPlus.Main.Site.Features.Language;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

namespace AlphaPlus.Main.Site.UnitTests.Features.Language
{
    public class LanguageControllerTests : IAsyncDisposable
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IUserSession> _userSessionMock;
        private readonly LanguageController _controller;
        private readonly IConfiguration _configuration;

        public LanguageControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _userSessionMock = new Mock<IUserSession>();
            _controller = new LanguageController(_mediatorMock.Object, _userSessionMock.Object);

            // Setup mock configuration to prevent AppSettings exceptions
            var inMemorySettings = new Dictionary<string, string>
            {
                ["SiteLabel"] = "MBC",
                ["SiteLabel789y"] = "789y",
                ["IsDisabledContactUsLink"] = "false",
                ["Banner:IsHide"] = "false",
                ["Banner:Environment"] = "test",
                ["Banner:IsHideMobileSabaApp"] = "false",
                ["EnableUMAnnouncementPopup"] = "false",
                ["UMAnnouncementPopup:ServerCacheInMinutes"] = "30",
                ["UMAnnouncementPopup:ClientIntervalInMinutes"] = "5",
                ["LiveChatLicenseID"] = "test-license",
                ["skypeId"] = "test-skype",
                ["letstalkid"] = "test-letstalk",
                ["letstalkidNew"] = "test-letstalk-new",
                ["letstalkurl"] = "https://test.com",
                ["letstalkurlNew"] = "https://test-new.com",
                ["DNReCheckin:IntervalInSeconds"] = "60",
                ["Recheckin:IntervalMiliseconds"] = "30000",
                ["JsVersion"] = "1.0.0",
                ["ApiKey:Gateway"] = "https://api.test.com",
                ["QRCode:Hide"] = "false",
                ["OfferAutoTransfer.Currencies"] = "USD,EUR",
                ["Dashboard:BannerOfferedSites"] = "MBC,789y",
                ["WhiteLabel:OfferedSites"] = "MBC,789y",
                ["EndWhiteLabelOffer"] = "2024-12-31",
                ["WinningThresholdSurveySites"] = "MBC,789y",
                ["WinningThresholdSurveyExcludedSites"] = "",
                ["WinningThresholdSurveyExcludedLines"] = "",
                ["DevicesUseCustomScrollbar"] = "device1,device2",
                ["TrackingConnectionCustIds"] = "1,2,3"
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            // Initialize ConfigurationManager with mock configuration
            AlphaPlus.Core.Configurations.ConfigurationManager.Init(_configuration);

            // Setup default HttpContext with host header
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Host"] = "test.example.com";
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
        }

        public ValueTask DisposeAsync()
        {
            // Reset configuration to avoid affecting other tests
            AlphaPlus.Core.Configurations.ConfigurationManager.Init(null);
            return ValueTask.CompletedTask;
        }

        [Fact]
        public async Task Change_WhenDisplayLanguageIsTrueAndUserIsInternal_ReturnsNullJson()
        {
            // Arrange
            _userSessionMock.Setup(x => x.IsInternal).Returns(true);

            // Act
            var result = await _controller.Change("en", true);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Change_WhenDisplayLanguageIsFalse_SetsCookiesAndReturnsViewModel()
        {
            // Arrange
            const string expectedLanguageCode = "en";
            _userSessionMock.Setup(x => x.IsInternal).Returns(false);
            _mediatorMock.Setup(x => x.Send(
                It.IsAny<ChangeLanguageCommand>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedLanguageCode);

            // Act
            var result = await _controller.Change(expectedLanguageCode, false);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedLanguageCode, result.Value);
            _mediatorMock.Verify(x => x.Send(
                It.Is<ChangeLanguageCommand>(cmd =>
                    cmd.LanguageCode == expectedLanguageCode),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Change_WhenDisplayLanguageIsNull_SetsCookiesAndReturnsViewModel()
        {
            // Arrange
            const string expectedLanguageCode = "en";
            _userSessionMock.Setup(x => x.IsInternal).Returns(false);
            _mediatorMock.Setup(x => x.Send(
                It.IsAny<ChangeLanguageCommand>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedLanguageCode);

            // Act
            var result = await _controller.Change(expectedLanguageCode, null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedLanguageCode, result.Value);
            _mediatorMock.Verify(x => x.Send(
                It.Is<ChangeLanguageCommand>(cmd =>
                    cmd.LanguageCode == expectedLanguageCode),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}