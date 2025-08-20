using AlphaPlus.Main.Site.Helpers;
using FluentAssertions;
using Xunit;

namespace AlphaPlus.Main.Site.UnitTests.Helpers
{
    public class DarkModeHelperTests
    {
        [Fact]
        public void DarkModeHelper_ShouldExist()
        {
            // Assert
            typeof(DarkModeHelper).Should().NotBeNull();
        }

        [Fact]
        public void DarkModeHelper_ShouldBeStatic()
        {
            // Assert
            typeof(DarkModeHelper).IsAbstract.Should().BeTrue();
        }
    }
}
