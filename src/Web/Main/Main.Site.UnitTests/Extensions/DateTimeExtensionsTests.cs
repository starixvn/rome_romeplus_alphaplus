using AlphaPlus.Main.Site.Extensions;
using FluentAssertions;
using Xunit;

namespace AlphaPlus.Main.Site.UnitTests.Extensions
{
    public class DateTimeExtensionsTests
    {
        [Fact]
        public void DateTimeExtensions_ShouldExist()
        {
            // Assert
            typeof(DateTimeExtensions).Should().NotBeNull();
        }

        [Fact]
        public void DateTimeExtensions_ShouldBeStatic()
        {
            // Assert
            typeof(DateTimeExtensions).IsAbstract.Should().BeTrue();
        }
    }
}
