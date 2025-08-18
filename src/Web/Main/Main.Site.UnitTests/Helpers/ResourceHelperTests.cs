using AlphaPlus.Main.Site.Helpers;
using FluentAssertions;
using Xunit;

namespace AlphaPlus.Main.Site.UnitTests.Helpers
{
    public class ResourceHelperTests
    {
        [Fact]
        public void ResourceHelper_ShouldExist()
        {
            // Assert
            typeof(ResourceHelper).Should().NotBeNull();
        }

        [Fact]
        public void ResourceHelper_ShouldBeStatic()
        {
            // Assert
            typeof(ResourceHelper).IsAbstract.Should().BeTrue();
        }
    }
}
