using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Reports.Site.Features.Shared.Tests
{
    [Trait("Category", "Shared")]
    public class SharedControllerTest
    {
        private readonly SharedController _controller;

        public SharedControllerTest()
        {
            _controller = new SharedController();
        }

        [Fact]
        public void InvalidReport_Should_Return_ViewResult()
        {
            // Act
            var result = _controller.InvalidReport();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void InvalidReport_Should_Return_Default_View()
        {
            // Act
            var result = _controller.InvalidReport() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.ViewName); // Default view name is null
        }

        [Fact]
        public void Controller_Should_Have_AllowProtectedModeAttribute()
        {
            // Arrange
            var controllerType = typeof(SharedController);

            // Act
            var attributes = controllerType.GetCustomAttributes(typeof(AlphaPlus.Core.Filters.AllowProtectedModeAttribute), true);

            // Assert
            Assert.NotEmpty(attributes);
            Assert.Single(attributes);
        }
    }
}