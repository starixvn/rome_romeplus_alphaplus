using AlphaPlus.Core.Models;
using AlphaPlus.Main.Site.Features.Dashboard;
using AlphaPlus.Main.Site.Features.Dashboard.Banner;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;

namespace AlphaPlus.Main.Site.UnitTests.Features.Dashboard
{
    public class DashboardBannerViewModelTests
    {
        private readonly Mock<IUserSession> _userSessionMock;
        private readonly Mock<IConfiguration> _configurationMock;

        public DashboardBannerViewModelTests()
        {
            _userSessionMock = new Mock<IUserSession>();
            _configurationMock = new Mock<IConfiguration>();
        }

        [Fact]
        public void Constructor_WithValidParameters_ShouldSetDependencies()
        {
            // Arrange & Act
            var viewModel = new DashboardBannerViewModel(_userSessionMock.Object, _configurationMock.Object);

            // Assert
            viewModel.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_ShouldCreateInstanceSuccessfully()
        {
            // Arrange
            _userSessionMock.Setup(x => x.Site).Returns("test-site");

            // Act
            var viewModel = new DashboardBannerViewModel(_userSessionMock.Object, _configurationMock.Object);

            // Assert
            viewModel.Should().NotBeNull();
            _userSessionMock.Verify(x => x.Site, Times.AtLeastOnce);
        }

        [Fact]
        public void Constructor_ShouldAccessUserSessionSite()
        {
            // Arrange
            const string expectedSite = "test-site";
            _userSessionMock.Setup(x => x.Site).Returns(expectedSite);

            // Act
            var viewModel = new DashboardBannerViewModel(_userSessionMock.Object, _configurationMock.Object);

            // Assert
            _userSessionMock.Verify(x => x.Site, Times.AtLeastOnce);
        }

        [Fact]
        public void Banners_ShouldReturnEnumerable()
        {
            // Arrange
            _userSessionMock.Setup(x => x.Site).Returns("test-site");

            // Act
            var viewModel = new DashboardBannerViewModel(_userSessionMock.Object, _configurationMock.Object);

            // Assert
            viewModel.Banners.Should().NotBeNull();
            viewModel.Banners.Should().BeAssignableTo<IEnumerable<BannerConfiguration>>();
        }

        [Fact]
        public void Banners_WhenIsShowIsFalse_ShouldReturnEmptyEnumerable()
        {
            // Arrange
            _userSessionMock.Setup(x => x.Site).Returns("unknown-site");

            // Act
            var viewModel = new DashboardBannerViewModel(_userSessionMock.Object, _configurationMock.Object);

            // Assert
            viewModel.Banners.Should().BeEmpty();
        }

        [Fact]
        public void Banners_WithEmptyConfigurationSection_ShouldReturnEmptyEnumerable()
        {
            // Arrange
            _userSessionMock.Setup(x => x.Site).Returns("MBC");
            _userSessionMock.Setup(x => x.Language).Returns("en");

            var mockSection = new Mock<IConfigurationSection>();
            mockSection.Setup(x => x.GetChildren()).Returns(new List<IConfigurationSection>());
            _configurationMock.Setup(x => x.GetSection("Banners:i1")).Returns(mockSection.Object);

            // Act
            var viewModel = new DashboardBannerViewModel(_userSessionMock.Object, _configurationMock.Object);

            // Assert
            viewModel.Banners.Should().BeEmpty();
        }

        [Fact]
        public void IsShow_Property_ShouldBeAccessible()
        {
            // Arrange
            _userSessionMock.Setup(x => x.Site).Returns("test-site");

            // Act
            var viewModel = new DashboardBannerViewModel(_userSessionMock.Object, _configurationMock.Object);

            // Assert
            viewModel.IsShow.Should().BeFalse(); // Default behavior when site is not in offered sites
        }

        [Fact]
        public void Banners_Property_ShouldBeAccessible()
        {
            // Arrange
            _userSessionMock.Setup(x => x.Site).Returns("test-site");

            // Act
            var viewModel = new DashboardBannerViewModel(_userSessionMock.Object, _configurationMock.Object);

            // Assert
            viewModel.Banners.Should().NotBeNull();
        }
    }
}
