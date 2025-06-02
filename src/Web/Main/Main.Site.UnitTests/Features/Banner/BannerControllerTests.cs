using AlphaPlus.Core.Models;
using AlphaPlus.Main.Site.Features.Banner;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

namespace AlphaPlus.Main.Site.UnitTests.Features.Banner
{
    public class BannerControllerTests : IAsyncDisposable
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly BannerController _controller;
        private readonly IConfiguration _configuration;

        public BannerControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new BannerController(_mediatorMock.Object);

            // Setup mock configuration
            var inMemorySettings = new Dictionary<string, string> {
                {"Banner:IsHide", "false"},
                {"Banner:Environment", "test"},
                {"Banner:IsHideMobileSabaApp", "false"},
                {"SiteLabel", "MBC"}
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            Core.Configurations.ConfigurationManager.Init(_configuration);
        }

        ValueTask IAsyncDisposable.DisposeAsync()
        {
            // Reset configuration to avoid affecting other tests
            Core.Configurations.ConfigurationManager.Init(null);
            return ValueTask.CompletedTask;
        }

        [Fact]
        public async Task Index_WhenIsProtectedMode_ReturnsPartialViewWithNullModel()
        {
            // Arrange
            var _userSessionMock = new Mock<IUserSession>();
            _userSessionMock.Setup(x => x.Language).Returns("en-US");
            _userSessionMock.Setup(x => x.IsProtectedMode).Returns(true);
            var query = new BannerQuery(_userSessionMock.Object);

            // Act
            var result = await _controller.Index(query);

            // Assert
            var partialViewResult = Assert.IsType<PartialViewResult>(result);
            Assert.Null(partialViewResult.Model);
            _mediatorMock.Verify(m => m.Send(It.IsAny<BannerQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Index_WhenNotProtectedMode_ReturnsPartialViewWithModel()
        {
            // Arrange
            var _userSessionMock = new Mock<IUserSession>();
            _userSessionMock.Setup(x => x.Language).Returns("en-US");
            _userSessionMock.Setup(x => x.IsProtectedMode).Returns(false);
            var query = new BannerQuery(_userSessionMock.Object);
            var expectedModel = new BannerViewModel(BannerType.Desktop, "en-US", string.Empty, _userSessionMock.Object);
            _mediatorMock.Setup(m => m.Send(query, default))
                        .ReturnsAsync(expectedModel);

            // Act
            var result = await _controller.Index(query);

            // Assert
            var partialViewResult = Assert.IsType<PartialViewResult>(result);
            Assert.Same(expectedModel, partialViewResult.Model);
            _mediatorMock.Verify(m => m.Send(query, It.IsAny<CancellationToken>()), Times.Once);
        }


    }
}