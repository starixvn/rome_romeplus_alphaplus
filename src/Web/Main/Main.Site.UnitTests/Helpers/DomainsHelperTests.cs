using AlphaPlus.Main.Site.Helpers;
using FluentAssertions;
using Xunit;

namespace AlphaPlus.Main.Site.UnitTests.Helpers
{
    public class DomainsHelperTests
    {
        [Fact]
        public void DomainsHelper_ShouldExist()
        {
            // Assert
            typeof(DomainsHelper).Should().NotBeNull();
        }

        [Fact]
        public void DomainsHelper_ShouldBeStatic()
        {
            // Assert
            typeof(DomainsHelper).IsAbstract.Should().BeTrue();
        }
    }
}
