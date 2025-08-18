using AlphaPlus.Main.Site;
using FluentAssertions;
using Xunit;

namespace AlphaPlus.Main.Site.UnitTests
{
    public class AppSettingsTests
    {
        [Fact]
        public void AppSettings_ShouldBeStatic()
        {
            // Assert
            typeof(AppSettings).IsAbstract.Should().BeTrue();
        }

        [Fact]
        public void AppSettings_ShouldHaveRequiredProperties()
        {
            // Assert
            typeof(AppSettings).GetProperties().Should().NotBeEmpty();
        }

        [Fact]
        public void AppSettings_ShouldHaveDevicesUseCustomScrollbarProperty()
        {
            // Assert
            var property = typeof(AppSettings).GetProperty("DevicesUseCustomScrollbar");
            property.Should().NotBeNull();
        }

        [Fact]
        public void AppSettings_ShouldHaveIsDisabledContactUsLinkProperty()
        {
            // Assert
            var property = typeof(AppSettings).GetProperty("IsDisabledContactUsLink");
            property.Should().NotBeNull();
        }

        [Fact]
        public void AppSettings_ShouldHaveIsMBCSiteProperty()
        {
            // Assert
            var property = typeof(AppSettings).GetProperty("IsMBCSite");
            property.Should().NotBeNull();
        }

        [Fact]
        public void AppSettings_ShouldHaveSiteLabelProperty()
        {
            // Assert
            var property = typeof(AppSettings).GetProperty("SiteLabel");
            property.Should().NotBeNull();
        }

        [Fact]
        public void AppSettings_ShouldHaveBannerIsHideProperty()
        {
            // Assert
            var property = typeof(AppSettings).GetProperty("BannerIsHide");
            property.Should().NotBeNull();
        }

        [Fact]
        public void AppSettings_ShouldHaveLiveChatLicenseIDProperty()
        {
            // Assert
            var property = typeof(AppSettings).GetProperty("LiveChatLicenseID");
            property.Should().NotBeNull();
        }

        [Fact]
        public void AppSettings_ShouldHaveAPIGatewaysURLProperty()
        {
            // Assert
            var property = typeof(AppSettings).GetProperty("APIGatewaysURL");
            property.Should().NotBeNull();
        }

        [Fact]
        public void AppSettings_ShouldHaveHideQRCodeProperty()
        {
            // Assert
            var property = typeof(AppSettings).GetProperty("HideQRCode");
            property.Should().NotBeNull();
        }

        [Fact]
        public void AppSettings_ShouldHaveDashboardBannerOfferedSitesProperty()
        {
            // Assert
            var property = typeof(AppSettings).GetProperty("DashboardBannerOfferedSites");
            property.Should().NotBeNull();
        }

        [Fact]
        public void AppSettings_ShouldHaveIsOfferedWhiteLabelMethod()
        {
            // Assert
            var method = typeof(AppSettings).GetMethod("IsOfferedWhiteLabel");
            method.Should().NotBeNull();
        }
    }
}
