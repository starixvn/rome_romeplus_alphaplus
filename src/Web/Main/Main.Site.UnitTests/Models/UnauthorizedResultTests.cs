using AlphaPlus.Main.Site.Shared.Models;
using FluentAssertions;
using Xunit;

namespace AlphaPlus.Main.Site.UnitTests.Models
{
    public class UnauthorizedResultTests
    {
        [Fact]
        public void UnauthorizedResult_ShouldExist()
        {
            // Assert
            typeof(UnauthorizedResult).Should().NotBeNull();
        }

        [Fact]
        public void UnauthorizedResult_ShouldBeInstantiable()
        {
            // Act
            var result = new UnauthorizedResult();

            // Assert
            result.Should().NotBeNull();
        }
    }
}
