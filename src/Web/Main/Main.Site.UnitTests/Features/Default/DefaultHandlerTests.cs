using AlphaPlus.Main.Site.Features.Default;
using FluentAssertions;
using Xunit;

namespace AlphaPlus.Main.Site.UnitTests.Features.Default
{
    public class DefaultHandlerTests : TestBase
    {
        [Fact]
        public void DefaultHandler_ShouldExist()
        {
            // Assert
            typeof(DefaultHandler).Should().NotBeNull();
        }

        [Fact]
        public void DefaultHandler_ShouldBePublic()
        {
            // Assert
            typeof(DefaultHandler).IsPublic.Should().BeTrue();
        }

        [Fact]
        public void DefaultHandler_ShouldHaveHandleMethod()
        {
            // Assert - Use more specific method search to avoid ambiguity
            var handleMethods = typeof(DefaultHandler).GetMethods()
                .Where(m => m.Name == "Handle")
                .ToList();

            handleMethods.Should().NotBeEmpty();
        }
    }
}
