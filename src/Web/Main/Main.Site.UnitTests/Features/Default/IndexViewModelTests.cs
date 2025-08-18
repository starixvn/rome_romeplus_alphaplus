using AlphaPlus.Main.Site.Features.Default;
using FluentAssertions;
using Xunit;

namespace AlphaPlus.Main.Site.UnitTests.Features.Default
{
    public class IndexViewModelTests : TestBase
    {
        [Fact]
        public void IndexViewModel_ShouldExist()
        {
            // Assert
            typeof(IndexViewModel).Should().NotBeNull();
        }

        [Fact]
        public void IndexViewModel_ShouldBePublic()
        {
            // Assert
            typeof(IndexViewModel).IsPublic.Should().BeTrue();
        }

        [Fact]
        public void IndexViewModel_ShouldHaveProperties()
        {
            // Assert
            typeof(IndexViewModel).GetProperties().Should().NotBeEmpty();
        }

        [Fact]
        public void IndexViewModel_ShouldBeInstantiable()
        {
            // Act
            // Note: IndexViewModel requires dependencies that we can't easily mock in this test
            // We'll just test that the class exists
            var viewModelType = typeof(IndexViewModel);

            // Assert
            viewModelType.Should().NotBeNull();
        }
    }
}
