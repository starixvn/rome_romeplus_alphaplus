using AlphaPlus.Main.Site.Features.Dashboard.Banner;
using FluentAssertions;
using Xunit;

namespace AlphaPlus.Main.Site.UnitTests.Features.Dashboard.Banner
{
    public class BannerClickLogCommandTests
    {
        [Fact]
        public void Constructor_WithValidParameters_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            const string pageUrl = "DashboardBanner/TestBanner";
            const byte roleId = 4;

            // Act
            var command = new BannerClickLogCommand(pageUrl, roleId);

            // Assert
            command.PageUrl.Should().Be(pageUrl);
            command.Role.Should().Be(roleId);
        }

        [Fact]
        public void Constructor_WithEmptyPageUrl_ShouldSetEmptyPageUrl()
        {
            // Arrange
            const string pageUrl = "";
            const byte roleId = 2;

            // Act
            var command = new BannerClickLogCommand(pageUrl, roleId);

            // Assert
            command.PageUrl.Should().Be(pageUrl);
            command.Role.Should().Be(roleId);
        }

        [Fact]
        public void Constructor_WithNullPageUrl_ShouldSetNullPageUrl()
        {
            // Arrange
            const string pageUrl = null;
            const byte roleId = 3;

            // Act
            var command = new BannerClickLogCommand(pageUrl, roleId);

            // Assert
            command.PageUrl.Should().BeNull();
            command.Role.Should().Be(roleId);
        }

        [Fact]
        public void Constructor_WithZeroRoleId_ShouldSetZeroRoleId()
        {
            // Arrange
            const string pageUrl = "DashboardBanner/TestBanner";
            const byte roleId = 0;

            // Act
            var command = new BannerClickLogCommand(pageUrl, roleId);

            // Assert
            command.PageUrl.Should().Be(pageUrl);
            command.Role.Should().Be(roleId);
        }

        [Fact]
        public void Constructor_WithMaximumRoleId_ShouldSetMaximumRoleId()
        {
            // Arrange
            const string pageUrl = "DashboardBanner/TestBanner";
            const byte roleId = byte.MaxValue;

            // Act
            var command = new BannerClickLogCommand(pageUrl, roleId);

            // Assert
            command.PageUrl.Should().Be(pageUrl);
            command.Role.Should().Be(roleId);
        }

        [Fact]
        public void GetSettingKey_ShouldReturnCorrectSettingKey()
        {
            // Arrange
            const string pageUrl = "DashboardBanner/TestBanner";
            const byte roleId = 4;
            var command = new BannerClickLogCommand(pageUrl, roleId);

            // Act
            var settingKey = command.GetSettingKey();

            // Assert
            settingKey.Should().Be("Age_UpdPageVisit");
        }

        [Fact]
        public void IsValid_WithValidPageUrl_ShouldReturnTrue()
        {
            // Arrange
            const string pageUrl = "DashboardBanner/TestBanner";
            const byte roleId = 4;
            var command = new BannerClickLogCommand(pageUrl, roleId);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void IsValid_WithEmptyPageUrl_ShouldReturnFalse()
        {
            // Arrange
            const string pageUrl = "";
            const byte roleId = 4;
            var command = new BannerClickLogCommand(pageUrl, roleId);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void IsValid_WithNullPageUrl_ShouldReturnFalse()
        {
            // Arrange
            const string pageUrl = null;
            const byte roleId = 4;
            var command = new BannerClickLogCommand(pageUrl, roleId);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void IsValid_WithWhitespacePageUrl_ShouldReturnTrue()
        {
            // Arrange
            const string pageUrl = "   ";
            const byte roleId = 4;
            var command = new BannerClickLogCommand(pageUrl, roleId);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void IsValid_WithLongPageUrl_ShouldReturnTrue()
        {
            // Arrange
            const string pageUrl = "DashboardBanner/VeryLongBannerNameThatExceedsNormalLength";
            const byte roleId = 4;
            var command = new BannerClickLogCommand(pageUrl, roleId);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void IsValid_WithSpecialCharactersInPageUrl_ShouldReturnTrue()
        {
            // Arrange
            const string pageUrl = "DashboardBanner/Test-Banner_123";
            const byte roleId = 4;
            var command = new BannerClickLogCommand(pageUrl, roleId);

            // Act
            var isValid = command.IsValid();

            // Assert
            isValid.Should().BeTrue();
        }
    }
}
