using AlphaPlus.Main.Site.Features.Default;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AlphaPlus.Main.Site.UnitTests.Features.Default
{
    public class DefaultControllerTests : TestBase
    {
        [Fact]
        public void DefaultController_ShouldExist()
        {
            // Assert
            typeof(DefaultController).Should().NotBeNull();
        }

        [Fact]
        public void DefaultController_ShouldInheritFromController()
        {
            // Assert
            typeof(DefaultController).BaseType.Should().BeAssignableTo<Controller>();
        }

        [Fact]
        public void DefaultController_ShouldHaveIndexAction()
        {
            // Assert
            var indexMethod = typeof(DefaultController).GetMethod("Index");
            indexMethod.Should().NotBeNull();
        }

        [Fact]
        public void DefaultController_ShouldHaveMenuBarAction()
        {
            // Assert
            var menuBarMethod = typeof(DefaultController).GetMethod("MenuBar");
            menuBarMethod.Should().NotBeNull();
        }

        [Fact]
        public void DefaultController_ShouldHaveRedirectAction()
        {
            // Assert - Use more specific method search to avoid ambiguity
            var redirectMethods = typeof(DefaultController).GetMethods()
                .Where(m => m.Name == "Redirect")
                .ToList();

            redirectMethods.Should().NotBeEmpty();
        }

        [Fact]
        public void DefaultController_ShouldBePublic()
        {
            // Assert
            typeof(DefaultController).IsPublic.Should().BeTrue();
        }
    }
}
