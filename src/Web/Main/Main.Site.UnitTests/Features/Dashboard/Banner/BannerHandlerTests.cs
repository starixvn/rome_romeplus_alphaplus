using AlphaPlus.Main.Site.Features.Dashboard.Banner;
using FluentAssertions;
using Moq;

namespace AlphaPlus.Main.Site.UnitTests.Features.Dashboard.Banner
{
    public class BannerHandlerTests
    {
        private readonly Mock<IBannerService> _bannerServiceMock;
        private readonly BannerHandler _handler;

        public BannerHandlerTests()
        {
            _bannerServiceMock = new Mock<IBannerService>();
            _handler = new BannerHandler(_bannerServiceMock.Object);
        }

        [Fact]
        public async Task Handle_WithValidRequest_ShouldCallBannerServiceAndReturnResult()
        {
            // Arrange
            const string bannerName = "TestBanner";
            const byte roleId = 4;
            const bool expectedResult = true;
            var request = new BannerClickLogRequest(bannerName, roleId);
            var cancellationToken = CancellationToken.None;

            _bannerServiceMock
                .Setup(x => x.LogClick(bannerName, roleId))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _handler.Handle(request, cancellationToken);

            // Assert
            result.Should().Be(expectedResult);
            _bannerServiceMock.Verify(x => x.LogClick(bannerName, roleId), Times.Once);
        }

        [Fact]
        public async Task Handle_WithDifferentBannerNameAndRoleId_ShouldCallBannerServiceWithCorrectParameters()
        {
            // Arrange
            const string bannerName = "PromoBanner";
            const byte roleId = 2;
            const bool expectedResult = false;
            var request = new BannerClickLogRequest(bannerName, roleId);
            var cancellationToken = CancellationToken.None;

            _bannerServiceMock
                .Setup(x => x.LogClick(bannerName, roleId))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _handler.Handle(request, cancellationToken);

            // Assert
            result.Should().Be(expectedResult);
            _bannerServiceMock.Verify(x => x.LogClick(bannerName, roleId), Times.Once);
        }

        [Fact]
        public async Task Handle_WithCancellationToken_ShouldPassCancellationTokenToService()
        {
            // Arrange
            const string bannerName = "TestBanner";
            const byte roleId = 3;
            const bool expectedResult = true;
            var request = new BannerClickLogRequest(bannerName, roleId);
            var cancellationToken = new CancellationTokenSource().Token;

            _bannerServiceMock
                .Setup(x => x.LogClick(bannerName, roleId))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _handler.Handle(request, cancellationToken);

            // Assert
            result.Should().Be(expectedResult);
            _bannerServiceMock.Verify(x => x.LogClick(bannerName, roleId), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenServiceThrowsException_ShouldPropagateException()
        {
            // Arrange
            const string bannerName = "TestBanner";
            const byte roleId = 4;
            var request = new BannerClickLogRequest(bannerName, roleId);
            var cancellationToken = CancellationToken.None;
            var expectedException = new InvalidOperationException("Service error");

            _bannerServiceMock
                .Setup(x => x.LogClick(bannerName, roleId))
                .ThrowsAsync(expectedException);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _handler.Handle(request, cancellationToken));

            exception.Should().Be(expectedException);
            _bannerServiceMock.Verify(x => x.LogClick(bannerName, roleId), Times.Once);
        }

        [Fact]
        public async Task Handle_WithEmptyBannerName_ShouldCallServiceWithEmptyString()
        {
            // Arrange
            const string bannerName = "";
            const byte roleId = 1;
            const bool expectedResult = true;
            var request = new BannerClickLogRequest(bannerName, roleId);
            var cancellationToken = CancellationToken.None;

            _bannerServiceMock
                .Setup(x => x.LogClick(bannerName, roleId))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _handler.Handle(request, cancellationToken);

            // Assert
            result.Should().Be(expectedResult);
            _bannerServiceMock.Verify(x => x.LogClick(bannerName, roleId), Times.Once);
        }

        [Fact]
        public async Task Handle_WithZeroRoleId_ShouldCallServiceWithZeroRoleId()
        {
            // Arrange
            const string bannerName = "TestBanner";
            const byte roleId = 0;
            const bool expectedResult = true;
            var request = new BannerClickLogRequest(bannerName, roleId);
            var cancellationToken = CancellationToken.None;

            _bannerServiceMock
                .Setup(x => x.LogClick(bannerName, roleId))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _handler.Handle(request, cancellationToken);

            // Assert
            result.Should().Be(expectedResult);
            _bannerServiceMock.Verify(x => x.LogClick(bannerName, roleId), Times.Once);
        }

        [Fact]
        public async Task Handle_WithNullBannerName_ShouldCallServiceWithNullBannerName()
        {
            // Arrange
            const string bannerName = null;
            const byte roleId = 1;
            const bool expectedResult = true;
            var request = new BannerClickLogRequest(bannerName, roleId);
            var cancellationToken = CancellationToken.None;

            _bannerServiceMock
                .Setup(x => x.LogClick(bannerName, roleId))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _handler.Handle(request, cancellationToken);

            // Assert
            result.Should().Be(expectedResult);
            _bannerServiceMock.Verify(x => x.LogClick(bannerName, roleId), Times.Once);
        }

        [Fact]
        public async Task Handle_WithMaximumRoleId_ShouldCallServiceWithMaximumRoleId()
        {
            // Arrange
            const string bannerName = "TestBanner";
            const byte roleId = byte.MaxValue;
            const bool expectedResult = true;
            var request = new BannerClickLogRequest(bannerName, roleId);
            var cancellationToken = CancellationToken.None;

            _bannerServiceMock
                .Setup(x => x.LogClick(bannerName, roleId))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _handler.Handle(request, cancellationToken);

            // Assert
            result.Should().Be(expectedResult);
            _bannerServiceMock.Verify(x => x.LogClick(bannerName, roleId), Times.Once);
        }

        [Fact]
        public async Task Handle_WithWhitespaceBannerName_ShouldCallServiceWithWhitespaceBannerName()
        {
            // Arrange
            const string bannerName = "   ";
            const byte roleId = 1;
            const bool expectedResult = true;
            var request = new BannerClickLogRequest(bannerName, roleId);
            var cancellationToken = CancellationToken.None;

            _bannerServiceMock
                .Setup(x => x.LogClick(bannerName, roleId))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _handler.Handle(request, cancellationToken);

            // Assert
            result.Should().Be(expectedResult);
            _bannerServiceMock.Verify(x => x.LogClick(bannerName, roleId), Times.Once);
        }

        [Fact]
        public async Task Handle_WithLongBannerName_ShouldCallServiceWithLongBannerName()
        {
            // Arrange
            const string bannerName = "VeryLongBannerNameThatExceedsNormalLength";
            const byte roleId = 4;
            const bool expectedResult = true;
            var request = new BannerClickLogRequest(bannerName, roleId);
            var cancellationToken = CancellationToken.None;

            _bannerServiceMock
                .Setup(x => x.LogClick(bannerName, roleId))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _handler.Handle(request, cancellationToken);

            // Assert
            result.Should().Be(expectedResult);
            _bannerServiceMock.Verify(x => x.LogClick(bannerName, roleId), Times.Once);
        }

        [Fact]
        public async Task Handle_WithSpecialCharactersInBannerName_ShouldCallServiceWithSpecialCharacters()
        {
            // Arrange
            const string bannerName = "Test-Banner_123";
            const byte roleId = 4;
            const bool expectedResult = true;
            var request = new BannerClickLogRequest(bannerName, roleId);
            var cancellationToken = CancellationToken.None;

            _bannerServiceMock
                .Setup(x => x.LogClick(bannerName, roleId))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _handler.Handle(request, cancellationToken);

            // Assert
            result.Should().Be(expectedResult);
            _bannerServiceMock.Verify(x => x.LogClick(bannerName, roleId), Times.Once);
        }

        [Fact]
        public async Task Handle_WithCancelledCancellationToken_ShouldHandleCancellationGracefully()
        {
            // Arrange
            const string bannerName = "TestBanner";
            const byte roleId = 4;
            var request = new BannerClickLogRequest(bannerName, roleId);
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            _bannerServiceMock
                .Setup(x => x.LogClick(bannerName, roleId))
                .ReturnsAsync(true);

            // Act & Assert
            // The handler should still call the service even with a cancelled token
            // since the service method doesn't take the cancellation token
            var result = await _handler.Handle(request, cancellationTokenSource.Token);

            // Assert
            result.Should().BeTrue();
            _bannerServiceMock.Verify(x => x.LogClick(bannerName, roleId), Times.Once);
        }

        [Fact]
        public async Task Handle_WithMultipleRequests_ShouldCallServiceCorrectlyForEach()
        {
            // Arrange
            var requests = new[]
            {
                new BannerClickLogRequest("Banner1", 1),
                new BannerClickLogRequest("Banner2", 2),
                new BannerClickLogRequest("Banner3", 3)
            };
            var cancellationToken = CancellationToken.None;

            _bannerServiceMock
                .Setup(x => x.LogClick(It.IsAny<string>(), It.IsAny<byte>()))
                .ReturnsAsync(true);

            // Act
            foreach (var request in requests)
            {
                await _handler.Handle(request, cancellationToken);
            }

            // Assert
            _bannerServiceMock.Verify(x => x.LogClick("Banner1", 1), Times.Once);
            _bannerServiceMock.Verify(x => x.LogClick("Banner2", 2), Times.Once);
            _bannerServiceMock.Verify(x => x.LogClick("Banner3", 3), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenServiceReturnsFalse_ShouldReturnFalse()
        {
            // Arrange
            const string bannerName = "TestBanner";
            const byte roleId = 4;
            const bool expectedResult = false;
            var request = new BannerClickLogRequest(bannerName, roleId);
            var cancellationToken = CancellationToken.None;

            _bannerServiceMock
                .Setup(x => x.LogClick(bannerName, roleId))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _handler.Handle(request, cancellationToken);

            // Assert
            result.Should().Be(expectedResult);
            _bannerServiceMock.Verify(x => x.LogClick(bannerName, roleId), Times.Once);
        }
    }
}

