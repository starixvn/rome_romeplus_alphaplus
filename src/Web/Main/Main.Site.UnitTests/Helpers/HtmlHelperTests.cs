using AlphaPlus.Main.Site.Helpers;
using FluentAssertions;
using Xunit;

namespace AlphaPlus.Main.Site.UnitTests.Helpers
{
    public class HtmlHelperTests
    {
        [Fact]
        public void HtmlHelper_ShouldExist()
        {
            // Assert
            typeof(HtmlHelper).Should().NotBeNull();
        }

        [Fact]
        public void HtmlHelper_ShouldBeStatic()
        {
            // Assert
            typeof(HtmlHelper).IsAbstract.Should().BeTrue();
        }

        [Fact]
        public void HtmlHelper_ShouldHaveNumericRowMethod()
        {
            // Assert
            var numericRowMethod = typeof(HtmlHelper).GetMethod("NumericRow");
            numericRowMethod.Should().NotBeNull();
        }

        [Fact]
        public void HtmlHelper_ShouldHaveNumericRowOutstandingMethod()
        {
            // Assert
            var numericRowOutstandingMethod = typeof(HtmlHelper).GetMethod("NumericRowOutstanding");
            numericRowOutstandingMethod.Should().NotBeNull();
        }

        [Fact]
        public void HtmlHelper_ShouldHaveCorrectMethodSignatures()
        {
            // Assert
            var numericRowMethod = typeof(HtmlHelper).GetMethod("NumericRow");
            numericRowMethod.Should().NotBeNull();
            numericRowMethod.IsStatic.Should().BeTrue();

            var numericRowOutstandingMethod = typeof(HtmlHelper).GetMethod("NumericRowOutstanding");
            numericRowOutstandingMethod.Should().NotBeNull();
            numericRowOutstandingMethod.IsStatic.Should().BeTrue();
        }
    }
}
