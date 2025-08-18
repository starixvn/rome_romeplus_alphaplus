using AlphaPlus.Core.Helpers;
using AlphaPlus.Main.Site.Shared.Services;
using AlphaPlus.NetThrottling;
using AlphaPlus.NetThrottling.Models;
using FluentAssertions;
using Moq;
using Xunit;

namespace AlphaPlus.Main.Site.UnitTests.Services
{
    public class NetThrottlingServiceTests : TestBase
    {
        [Fact]
        public void NetThrottlingService_ShouldExist()
        {
            // Assert
            typeof(NetThrottlingService).Should().NotBeNull();
        }

        [Fact]
        public void NetThrottlingService_ShouldImplementINetThrottlingService()
        {
            // Assert
            typeof(NetThrottlingService).Should().Implement<INetThrottlingService>();
        }

        [Fact]
        public void NetThrottlingService_ShouldHaveConstructor()
        {
            // Assert
            var constructors = typeof(NetThrottlingService).GetConstructors();
            constructors.Should().NotBeEmpty();
        }

        [Fact]
        public void NetThrottlingService_ShouldHaveThrottlePolicyProperty()
        {
            // Assert
            var property = typeof(NetThrottlingService).GetProperty("ThrottlePolicy");
            property.Should().NotBeNull();
        }

        [Fact]
        public void NetThrottlingService_ShouldHaveGetBlockRemainingSecondsMethod()
        {
            // Assert
            var method = typeof(NetThrottlingService).GetMethod("GetBlockRemainingSeconds");
            method.Should().NotBeNull();
        }

        [Fact]
        public void NetThrottlingService_ShouldHaveIsBlacklistMethod()
        {
            // Assert
            var method = typeof(NetThrottlingService).GetMethod("IsBlacklist");
            method.Should().NotBeNull();
        }

        [Fact]
        public void NetThrottlingService_ShouldHaveThrottleMethod()
        {
            // Assert
            var method = typeof(NetThrottlingService).GetMethod("Throttle");
            method.Should().NotBeNull();
        }
    }
}
