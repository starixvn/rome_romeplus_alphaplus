using AlphaPlus.Main.Site.Features.Dashboard.Banner;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

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
    }
}

