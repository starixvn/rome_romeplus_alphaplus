using AlphaPlus.Main.Site.Features.Dashboard.Banner;
using FluentAssertions;
using MediatR;

namespace AlphaPlus.Main.Site.UnitTests.Features.Dashboard.Banner
{
    public class BannerClickLogRequestTests
    {
        [Fact]
        public void Constructor_WithValidParameters_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            const string bannerName = "TestBanner";
            const byte roleId = 4;

            // Act
            var request = new BannerClickLogRequest(bannerName, roleId);

            // Assert
            request.BannerName.Should().Be(bannerName);
            request.RoleId.Should().Be(roleId);
        }

        [Fact]
        public void Constructor_WithEmptyBannerName_ShouldSetEmptyBannerName()
        {
            // Arrange
            const string bannerName = "";
            const byte roleId = 2;

            // Act
            var request = new BannerClickLogRequest(bannerName, roleId);

            // Assert
            request.BannerName.Should().Be(bannerName);
            request.RoleId.Should().Be(roleId);
        }

        [Fact]
        public void Constructor_WithNullBannerName_ShouldSetNullBannerName()
        {
            // Arrange
            const string bannerName = null;
            const byte roleId = 3;

            // Act
            var request = new BannerClickLogRequest(bannerName, roleId);

            // Assert
            request.BannerName.Should().BeNull();
            request.RoleId.Should().Be(roleId);
        }

        [Fact]
        public void Constructor_WithZeroRoleId_ShouldSetZeroRoleId()
        {
            // Arrange
            const string bannerName = "TestBanner";
            const byte roleId = 0;

            // Act
            var request = new BannerClickLogRequest(bannerName, roleId);

            // Assert
            request.BannerName.Should().Be(bannerName);
            request.RoleId.Should().Be(roleId);
        }

        [Fact]
        public void Constructor_WithMaximumRoleId_ShouldSetMaximumRoleId()
        {
            // Arrange
            const string bannerName = "TestBanner";
            const byte roleId = byte.MaxValue;

            // Act
            var request = new BannerClickLogRequest(bannerName, roleId);

            // Assert
            request.BannerName.Should().Be(bannerName);
            request.RoleId.Should().Be(roleId);
        }

        [Fact]
        public void Constructor_WithWhitespaceBannerName_ShouldSetWhitespaceBannerName()
        {
            // Arrange
            const string bannerName = "   ";
            const byte roleId = 1;

            // Act
            var request = new BannerClickLogRequest(bannerName, roleId);

            // Assert
            request.BannerName.Should().Be(bannerName);
            request.RoleId.Should().Be(roleId);
        }

        [Fact]
        public void Constructor_WithLongBannerName_ShouldSetLongBannerName()
        {
            // Arrange
            const string bannerName = "VeryLongBannerNameThatExceedsNormalLength";
            const byte roleId = 4;

            // Act
            var request = new BannerClickLogRequest(bannerName, roleId);

            // Assert
            request.BannerName.Should().Be(bannerName);
            request.RoleId.Should().Be(roleId);
        }

        [Fact]
        public void Constructor_WithSpecialCharactersInBannerName_ShouldSetSpecialCharactersBannerName()
        {
            // Arrange
            const string bannerName = "Test-Banner_123";
            const byte roleId = 4;

            // Act
            var request = new BannerClickLogRequest(bannerName, roleId);

            // Assert
            request.BannerName.Should().Be(bannerName);
            request.RoleId.Should().Be(roleId);
        }

        [Fact]
        public void Constructor_WithDifferentBannerNameAndRoleId_ShouldSetDifferentValues()
        {
            // Arrange
            const string bannerName = "PromoBanner";
            const byte roleId = 2;

            // Act
            var request = new BannerClickLogRequest(bannerName, roleId);

            // Assert
            request.BannerName.Should().Be(bannerName);
            request.RoleId.Should().Be(roleId);
        }

        [Fact]
        public void ShouldImplementIRequestOfBool()
        {
            // Arrange & Act
            var request = new BannerClickLogRequest("TestBanner", 4);

            // Assert
            request.Should().BeAssignableTo<IRequest<bool>>();
        }

        [Fact]
        public void Properties_ShouldBeReadOnly()
        {
            // Arrange
            var request = new BannerClickLogRequest("TestBanner", 4);

            // Act & Assert
            request.BannerName.Should().Be("TestBanner");
            request.RoleId.Should().Be(4);

            // Properties should be read-only (no setters)
            var type = request.GetType();
            var bannerNameProperty = type.GetProperty(nameof(BannerClickLogRequest.BannerName));
            var roleIdProperty = type.GetProperty(nameof(BannerClickLogRequest.RoleId));

            bannerNameProperty.Should().NotBeNull();
            roleIdProperty.Should().NotBeNull();
            bannerNameProperty.CanWrite.Should().BeFalse();
            roleIdProperty.CanWrite.Should().BeFalse();
        }
    }
}

