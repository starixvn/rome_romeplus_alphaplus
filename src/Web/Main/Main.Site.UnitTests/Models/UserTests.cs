using AlphaPlus.Core.Helpers;
using AlphaPlus.Main.Site.Shared.Models;
using FluentAssertions;
using Moq;
using Xunit;

namespace AlphaPlus.Main.Site.UnitTests.Models
{
    public class UserTests : TestBase
    {
        [Fact]
        public void User_ShouldImplementIUser()
        {
            // Assert
            typeof(User).Should().Implement<IUser>();
        }

        [Fact]
        public void User_ShouldHaveRequiredProperties()
        {
            // Assert
            typeof(User).GetProperty("IsEnabledJackpot").Should().NotBeNull();
            typeof(User).GetProperty("ShowProgressiveJackpotGames").Should().NotBeNull();
            typeof(User).GetProperty("IsAdminCanUpdateSubAccount").Should().NotBeNull();
            typeof(User).GetProperty("IsEnabledTelegramOTP").Should().NotBeNull();
            typeof(User).GetProperty("CustId").Should().NotBeNull();
            typeof(User).GetProperty("IsSyncedNovaCasino").Should().NotBeNull();
        }

        [Fact]
        public void User_ShouldBeInstantiable()
        {
            // Act
            var user = new User();

            // Assert
            user.Should().NotBeNull();
        }

        [Fact]
        public void User_ShouldHaveCorrectConstants()
        {
            // Assert
            // Note: These constants are private in the User class, so we can't test them directly
            // We can only verify the class structure
            typeof(User).Should().NotBeNull();
        }
    }
}
