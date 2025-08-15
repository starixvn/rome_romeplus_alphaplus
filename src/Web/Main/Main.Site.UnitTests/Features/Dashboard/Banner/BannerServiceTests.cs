using AlphaPlus.Main.Site.Features.Dashboard.Banner;
using Fanex.Data.Repository;
using FluentAssertions;
using Moq;

namespace AlphaPlus.Main.Site.UnitTests.Features.Dashboard.Banner
{
    public class BannerServiceTests
    {
        private readonly Mock<IDynamicRepository> _dynamicRepositoryMock;
        private readonly BannerService _bannerService;

        public BannerServiceTests()
        {
            _dynamicRepositoryMock = new Mock<IDynamicRepository>();
            _bannerService = new BannerService(_dynamicRepositoryMock.Object);
        }

        [Fact]
        public async Task LogClick_WithValidParameters_ShouldExecuteCommandAndReturnTrue()
        {
            // Arrange
            const string bannerName = "TestBanner";
            const byte roleId = 4;

            _dynamicRepositoryMock
                .Setup(x => x.ExecuteAsync(It.IsAny<BannerClickLogCommand>(), string.Empty))
                .ReturnsAsync(1);

            // Act
            var result = await _bannerService.LogClick(bannerName, roleId);

            // Assert
            result.Should().BeTrue();
            _dynamicRepositoryMock.Verify(x => x.ExecuteAsync(
                It.Is<BannerClickLogCommand>(cmd =>
                    cmd.PageUrl == $"DashboardBanner/{bannerName}" &&
                    cmd.Role == roleId),
                string.Empty),
                Times.Once);
        }

        [Fact]
        public async Task LogClick_WithDifferentBannerNameAndRoleId_ShouldExecuteCommandWithCorrectParameters()
        {
            // Arrange
            const string bannerName = "PromoBanner";
            const byte roleId = 2;

            _dynamicRepositoryMock
                .Setup(x => x.ExecuteAsync(It.IsAny<BannerClickLogCommand>(), string.Empty))
                .ReturnsAsync(1);

            // Act
            var result = await _bannerService.LogClick(bannerName, roleId);

            // Assert
            result.Should().BeTrue();
            _dynamicRepositoryMock.Verify(x => x.ExecuteAsync(
                It.Is<BannerClickLogCommand>(cmd =>
                    cmd.PageUrl == $"DashboardBanner/{bannerName}" &&
                    cmd.Role == roleId),
                string.Empty),
                Times.Once);
        }

        [Fact]
        public async Task LogClick_WithEmptyBannerName_ShouldExecuteCommandWithEmptyBannerName()
        {
            // Arrange
            const string bannerName = "";
            const byte roleId = 1;

            _dynamicRepositoryMock
                .Setup(x => x.ExecuteAsync(It.IsAny<BannerClickLogCommand>(), string.Empty))
                .ReturnsAsync(1);

            // Act
            var result = await _bannerService.LogClick(bannerName, roleId);

            // Assert
            result.Should().BeTrue();
            _dynamicRepositoryMock.Verify(x => x.ExecuteAsync(
                It.Is<BannerClickLogCommand>(cmd =>
                    cmd.PageUrl == "DashboardBanner/" &&
                    cmd.Role == roleId),
                string.Empty),
                Times.Once);
        }

        [Fact]
        public async Task LogClick_WithZeroRoleId_ShouldExecuteCommandWithZeroRoleId()
        {
            // Arrange
            const string bannerName = "TestBanner";
            const byte roleId = 0;

            _dynamicRepositoryMock
                .Setup(x => x.ExecuteAsync(It.IsAny<BannerClickLogCommand>(), string.Empty))
                .ReturnsAsync(1);

            // Act
            var result = await _bannerService.LogClick(bannerName, roleId);

            // Assert
            result.Should().BeTrue();
            _dynamicRepositoryMock.Verify(x => x.ExecuteAsync(
                It.Is<BannerClickLogCommand>(cmd =>
                    cmd.PageUrl == $"DashboardBanner/{bannerName}" &&
                    cmd.Role == roleId),
                string.Empty),
                Times.Once);
        }

        [Fact]
        public async Task LogClick_WithNullBannerName_ShouldExecuteCommandWithNullBannerName()
        {
            // Arrange
            const string bannerName = null;
            const byte roleId = 3;

            _dynamicRepositoryMock
                .Setup(x => x.ExecuteAsync(It.IsAny<BannerClickLogCommand>(), string.Empty))
                .ReturnsAsync(1);

            // Act
            var result = await _bannerService.LogClick(bannerName, roleId);

            // Assert
            result.Should().BeTrue();
            _dynamicRepositoryMock.Verify(x => x.ExecuteAsync(
                It.Is<BannerClickLogCommand>(cmd =>
                    cmd.PageUrl == "DashboardBanner/" &&
                    cmd.Role == roleId),
                string.Empty),
                Times.Once);
        }

        [Fact]
        public async Task LogClick_WhenRepositoryThrowsException_ShouldPropagateException()
        {
            // Arrange
            const string bannerName = "TestBanner";
            const byte roleId = 4;
            var expectedException = new InvalidOperationException("Repository error");

            _dynamicRepositoryMock
                .Setup(x => x.ExecuteAsync(It.IsAny<BannerClickLogCommand>(), string.Empty))
                .ThrowsAsync(expectedException);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _bannerService.LogClick(bannerName, roleId));

            exception.Should().Be(expectedException);
            _dynamicRepositoryMock.Verify(x => x.ExecuteAsync(
                It.Is<BannerClickLogCommand>(cmd =>
                    cmd.PageUrl == $"DashboardBanner/{bannerName}" &&
                    cmd.Role == roleId),
                string.Empty),
                Times.Once);
        }

        [Fact]
        public async Task LogClick_ShouldCreateCommandWithCorrectSettingKey()
        {
            // Arrange
            const string bannerName = "TestBanner";
            const byte roleId = 4;

            _dynamicRepositoryMock
                .Setup(x => x.ExecuteAsync(It.IsAny<BannerClickLogCommand>(), string.Empty))
                .ReturnsAsync(1);

            // Act
            await _bannerService.LogClick(bannerName, roleId);

            // Assert
            _dynamicRepositoryMock.Verify(x => x.ExecuteAsync(
                It.Is<BannerClickLogCommand>(cmd =>
                    cmd.GetSettingKey() == "Age_UpdPageVisit"),
                string.Empty),
                Times.Once);
        }

        [Fact]
        public async Task LogClick_ShouldCreateValidCommand()
        {
            // Arrange
            const string bannerName = "TestBanner";
            const byte roleId = 4;

            _dynamicRepositoryMock
                .Setup(x => x.ExecuteAsync(It.IsAny<BannerClickLogCommand>(), string.Empty))
                .ReturnsAsync(1);

            // Act
            await _bannerService.LogClick(bannerName, roleId);

            // Assert
            _dynamicRepositoryMock.Verify(x => x.ExecuteAsync(
                It.Is<BannerClickLogCommand>(cmd => cmd.IsValid()),
                string.Empty),
                Times.Once);
        }

        [Fact]
        public async Task LogClick_WithMaximumRoleId_ShouldExecuteCommandWithMaximumRoleId()
        {
            // Arrange
            const string bannerName = "TestBanner";
            const byte roleId = byte.MaxValue;

            _dynamicRepositoryMock
                .Setup(x => x.ExecuteAsync(It.IsAny<BannerClickLogCommand>(), string.Empty))
                .ReturnsAsync(1);

            // Act
            var result = await _bannerService.LogClick(bannerName, roleId);

            // Assert
            result.Should().BeTrue();
            _dynamicRepositoryMock.Verify(x => x.ExecuteAsync(
                It.Is<BannerClickLogCommand>(cmd =>
                    cmd.PageUrl == $"DashboardBanner/{bannerName}" &&
                    cmd.Role == roleId),
                string.Empty),
                Times.Once);
        }

        [Fact]
        public async Task LogClick_WithWhitespaceBannerName_ShouldExecuteCommandWithWhitespaceBannerName()
        {
            // Arrange
            const string bannerName = "   ";
            const byte roleId = 1;

            _dynamicRepositoryMock
                .Setup(x => x.ExecuteAsync(It.IsAny<BannerClickLogCommand>(), string.Empty))
                .ReturnsAsync(1);

            // Act
            var result = await _bannerService.LogClick(bannerName, roleId);

            // Assert
            result.Should().BeTrue();
            _dynamicRepositoryMock.Verify(x => x.ExecuteAsync(
                It.Is<BannerClickLogCommand>(cmd =>
                    cmd.PageUrl == "DashboardBanner/   " &&
                    cmd.Role == roleId),
                string.Empty),
                Times.Once);
        }

        [Fact]
        public async Task LogClick_WithLongBannerName_ShouldExecuteCommandWithLongBannerName()
        {
            // Arrange
            const string bannerName = "VeryLongBannerNameThatExceedsNormalLength";
            const byte roleId = 4;

            _dynamicRepositoryMock
                .Setup(x => x.ExecuteAsync(It.IsAny<BannerClickLogCommand>(), string.Empty))
                .ReturnsAsync(1);

            // Act
            var result = await _bannerService.LogClick(bannerName, roleId);

            // Assert
            result.Should().BeTrue();
            _dynamicRepositoryMock.Verify(x => x.ExecuteAsync(
                It.Is<BannerClickLogCommand>(cmd =>
                    cmd.PageUrl == $"DashboardBanner/{bannerName}" &&
                    cmd.Role == roleId),
                string.Empty),
                Times.Once);
        }

        [Fact]
        public async Task LogClick_WithSpecialCharactersInBannerName_ShouldExecuteCommandWithSpecialCharacters()
        {
            // Arrange
            const string bannerName = "Test-Banner_123";
            const byte roleId = 4;

            _dynamicRepositoryMock
                .Setup(x => x.ExecuteAsync(It.IsAny<BannerClickLogCommand>(), string.Empty))
                .ReturnsAsync(1);

            // Act
            var result = await _bannerService.LogClick(bannerName, roleId);

            // Assert
            result.Should().BeTrue();
            _dynamicRepositoryMock.Verify(x => x.ExecuteAsync(
                It.Is<BannerClickLogCommand>(cmd =>
                    cmd.PageUrl == $"DashboardBanner/{bannerName}" &&
                    cmd.Role == roleId),
                string.Empty),
                Times.Once);
        }

        [Fact]
        public async Task LogClick_WhenRepositoryReturnsZero_ShouldStillReturnTrue()
        {
            // Arrange
            const string bannerName = "TestBanner";
            const byte roleId = 4;

            _dynamicRepositoryMock
                .Setup(x => x.ExecuteAsync(It.IsAny<BannerClickLogCommand>(), string.Empty))
                .ReturnsAsync(0);

            // Act
            var result = await _bannerService.LogClick(bannerName, roleId);

            // Assert
            result.Should().BeTrue();
            _dynamicRepositoryMock.Verify(x => x.ExecuteAsync(
                It.Is<BannerClickLogCommand>(cmd =>
                    cmd.PageUrl == $"DashboardBanner/{bannerName}" &&
                    cmd.Role == roleId),
                string.Empty),
                Times.Once);
        }

        [Fact]
        public async Task LogClick_WhenRepositoryReturnsNegativeValue_ShouldStillReturnTrue()
        {
            // Arrange
            const string bannerName = "TestBanner";
            const byte roleId = 4;

            _dynamicRepositoryMock
                .Setup(x => x.ExecuteAsync(It.IsAny<BannerClickLogCommand>(), string.Empty))
                .ReturnsAsync(-1);

            // Act
            var result = await _bannerService.LogClick(bannerName, roleId);

            // Assert
            result.Should().BeTrue();
            _dynamicRepositoryMock.Verify(x => x.ExecuteAsync(
                It.Is<BannerClickLogCommand>(cmd =>
                    cmd.PageUrl == $"DashboardBanner/{bannerName}" &&
                    cmd.Role == roleId),
                string.Empty),
                Times.Once);
        }

        [Fact]
        public async Task LogClick_WithMultipleCalls_ShouldExecuteCommandCorrectlyForEach()
        {
            // Arrange
            var calls = new[]
            {
                new { BannerName = "Banner1", RoleId = (byte)1 },
                new { BannerName = "Banner2", RoleId = (byte)2 },
                new { BannerName = "Banner3", RoleId = (byte)3 }
            };

            _dynamicRepositoryMock
                .Setup(x => x.ExecuteAsync(It.IsAny<BannerClickLogCommand>(), string.Empty))
                .ReturnsAsync(1);

            // Act
            foreach (var call in calls)
            {
                await _bannerService.LogClick(call.BannerName, call.RoleId);
            }

            // Assert
            foreach (var call in calls)
            {
                _dynamicRepositoryMock.Verify(x => x.ExecuteAsync(
                    It.Is<BannerClickLogCommand>(cmd =>
                        cmd.PageUrl == $"DashboardBanner/{call.BannerName}" &&
                        cmd.Role == call.RoleId),
                    string.Empty),
                    Times.Once);
            }
        }

        [Fact]
        public async Task LogClick_ShouldAlwaysReturnTrue_RegardlessOfRepositoryResult()
        {
            // Arrange
            const string bannerName = "TestBanner";
            const byte roleId = 4;
            var repositoryResults = new[] { -5, 0, 1, 100, 999 };

            foreach (var result in repositoryResults)
            {
                _dynamicRepositoryMock
                    .Setup(x => x.ExecuteAsync(It.IsAny<BannerClickLogCommand>(), string.Empty))
                    .ReturnsAsync(result);

                // Act
                var serviceResult = await _bannerService.LogClick(bannerName, roleId);

                // Assert
                serviceResult.Should().BeTrue();
            }
        }
    }
}
