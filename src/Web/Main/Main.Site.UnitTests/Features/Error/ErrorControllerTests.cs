using AlphaPlus.Main.Site.Features.Error;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace AlphaPlus.Main.Site.UnitTests.Features.Error
{
    public class ErrorControllerTests : TestBase
    {
        [Fact]
        public void ErrorController_ShouldExist()
        {
            // Assert
            typeof(ErrorController).Should().NotBeNull();
        }

        [Fact]
        public void ErrorController_ShouldInheritFromController()
        {
            // Assert
            typeof(ErrorController).BaseType.Should().BeAssignableTo<Controller>();
        }

        [Fact]
        public void ErrorController_ShouldBePublic()
        {
            // Assert
            typeof(ErrorController).IsPublic.Should().BeTrue();
        }
    }
}
