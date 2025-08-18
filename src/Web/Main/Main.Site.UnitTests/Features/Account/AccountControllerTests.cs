using AlphaPlus.Main.Site.Features.Account;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace AlphaPlus.Main.Site.UnitTests.Features.Account
{
    public class AccountControllerTests : TestBase
    {
        [Fact]
        public void AccountController_ShouldExist()
        {
            // Assert
            typeof(AccountController).Should().NotBeNull();
        }

        [Fact]
        public void AccountController_ShouldInheritFromController()
        {
            // Assert
            typeof(AccountController).BaseType.Should().BeAssignableTo<Controller>();
        }

        [Fact]
        public void AccountController_ShouldBePublic()
        {
            // Assert
            typeof(AccountController).IsPublic.Should().BeTrue();
        }
    }
}
