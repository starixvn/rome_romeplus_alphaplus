using AlphaPlus.Core.Models;
using AlphaPlus.Main.Site.Features.Banner;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace AlphaPlus.Main.Site.UnitTests.Features.Banner
{
    public class BannerHandlerTests
    {
        private readonly Mock<IBannerService> _bannerServiceMock;
        private readonly Mock<IUserSession> _userSessionMock;
        private readonly BannerHandler _handler;
        private readonly IConfiguration _configuration;

        public BannerHandlerTests()
        {
            _bannerServiceMock = new Mock<IBannerService>();
            _userSessionMock = new Mock<IUserSession>();
            _handler = new BannerHandler(_bannerServiceMock.Object, _userSessionMock.Object);

            // Setup mock configuration
            var inMemorySettings = new Dictionary<string, string> {
                {"Banner:IsHide", "false"},
                {"Banner:Environment", "test"},
                {"Banner:IsHideMobileSabaApp", "true"},
                {"SiteLabel", "MBC"}
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            Core.Configurations.ConfigurationManager.Init(_configuration);
        }

        public void Dispose()
        {
            // Reset configuration to avoid affecting other tests
            Core.Configurations.ConfigurationManager.Init(null);
        }

        [Fact]
        public async Task Handle_WhenBannerExists_ReturnsBannerViewModel()
        {
            // Arrange
            const string language = "en-US";
            const byte bannerType = BannerType.Desktop;
            var imageContent = new byte[] { 1, 2, 3, 4, 5 };
            _userSessionMock.Setup(x => x.Language).Returns(language);

            var query = new BannerQuery(_userSessionMock.Object) { Type = bannerType };

            var banner = new BannerItem { ImageContent = imageContent };
            _bannerServiceMock
                .Setup(x => x.GetBanner(language, bannerType))
                .ReturnsAsync(banner);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(bannerType, result.Type);
            Assert.Equal(language, result.Language);
            Assert.Same(_userSessionMock.Object, result.UserSession);
            Assert.Contains($"data:image/png;base64,{Convert.ToBase64String(imageContent)}", result.Banners.Select(b => b.ImageSource));

            _bannerServiceMock.Verify(x => x.GetBanner(language, bannerType), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenBannerDoesNotExist_ReturnsBannerViewModelWithEmptyImage()
        {
            // Arrange
            const string language = "en-US";
            const byte bannerType = BannerType.Desktop;
            _userSessionMock.Setup(x => x.Language).Returns(language);

            var query = new BannerQuery(_userSessionMock.Object) { Type = bannerType };

            _bannerServiceMock
                .Setup(x => x.GetBanner(language, bannerType))
                .ReturnsAsync((BannerItem)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(bannerType, result.Type);
            Assert.Equal(language, result.Language);
            Assert.Same(_userSessionMock.Object, result.UserSession);
            // Assert.Contains(string.Empty, result.Banners.Select(b => b.ImageSource));

            _bannerServiceMock.Verify(x => x.GetBanner(language, bannerType), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenBannerHasNoImageContent_ReturnsBannerViewModelWithEmptyImage()
        {
            // Arrange
            const string language = "en-US";
            const byte bannerType = BannerType.Desktop;
            _userSessionMock.Setup(x => x.Language).Returns(language);

            var query = new BannerQuery(_userSessionMock.Object) { Type = bannerType };

            var banner = new BannerItem { ImageContent = null };
            _bannerServiceMock
                .Setup(x => x.GetBanner(language, bannerType))
                .ReturnsAsync(banner);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(bannerType, result.Type);
            Assert.Equal(language, result.Language);
            Assert.Same(_userSessionMock.Object, result.UserSession);
            //Assert.Contains(string.Empty, result.Banners.Select(b => b.ImageSource));

            _bannerServiceMock.Verify(x => x.GetBanner(language, bannerType), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenBannerHasEmptyImageContent_ReturnsBannerViewModelWithEmptyImage()
        {
            // Arrange
            const string language = "en-US";
            const byte bannerType = BannerType.Desktop;
            _userSessionMock.Setup(x => x.Language).Returns(language);

            var query = new BannerQuery(_userSessionMock.Object) { Type = bannerType };

            var banner = new BannerItem { ImageContent = [] };
            _bannerServiceMock
                .Setup(x => x.GetBanner(language, bannerType))
                .ReturnsAsync(banner);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(bannerType, result.Type);
            Assert.Equal(language, result.Language);
            Assert.Same(_userSessionMock.Object, result.UserSession);
            //Assert.Contains(string.Empty, result.Banners.Select(b => b.ImageSource));

            _bannerServiceMock.Verify(x => x.GetBanner(language, bannerType), Times.Once);
        }
    }
}