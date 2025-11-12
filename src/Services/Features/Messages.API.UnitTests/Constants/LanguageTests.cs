using AlphaPlus.Features.Messages.API.Constants;
using Xunit;

namespace AlphaPlus.Features.Messages.API.UnitTests.Constants
{
    public class LanguageTests
    {
        [Fact]
        public void English_HasCorrectValue()
        {
            // Assert
            Assert.Equal("en-US", Language.English);
        }

        [Fact]
        public void ChineseTraditional_HasCorrectValue()
        {
            // Assert
            Assert.Equal("zh-TW", Language.ChineseTraditional);
        }

        [Fact]
        public void ChineseSimplified_HasCorrectValue()
        {
            // Assert
            Assert.Equal("zh-CN", Language.ChineseSimplified);
        }

        [Fact]
        public void Japanese_HasCorrectValue()
        {
            // Assert
            Assert.Equal("ja-JP", Language.Japanese);
        }

        [Fact]
        public void Thai_HasCorrectValue()
        {
            // Assert
            Assert.Equal("th-TH", Language.Thai);
        }

        [Fact]
        public void Korean_HasCorrectValue()
        {
            // Assert
            Assert.Equal("ko-KR", Language.Korean);
        }

        [Fact]
        public void Vietnamese_HasCorrectValue()
        {
            // Assert
            Assert.Equal("vi-VN", Language.Vietnamese);
        }

        [Fact]
        public void Default_IsEnglish()
        {
            // Assert
            Assert.Equal(Language.English, Language.Default);
        }

        [Fact]
        public void AllLanguages_AreNotNull()
        {
            // Assert
            Assert.NotNull(Language.English);
            Assert.NotNull(Language.ChineseTraditional);
            Assert.NotNull(Language.ChineseSimplified);
            Assert.NotNull(Language.Japanese);
            Assert.NotNull(Language.Thai);
            Assert.NotNull(Language.Korean);
            Assert.NotNull(Language.Vietnamese);
            Assert.NotNull(Language.Default);
        }

        [Fact]
        public void AllLanguages_AreNotEmpty()
        {
            // Assert
            Assert.NotEmpty(Language.English);
            Assert.NotEmpty(Language.ChineseTraditional);
            Assert.NotEmpty(Language.ChineseSimplified);
            Assert.NotEmpty(Language.Japanese);
            Assert.NotEmpty(Language.Thai);
            Assert.NotEmpty(Language.Korean);
            Assert.NotEmpty(Language.Vietnamese);
            Assert.NotEmpty(Language.Default);
        }

        [Fact]
        public void AllLanguages_HaveCorrectFormat()
        {
            // Assert - All should follow language-COUNTRY format
            Assert.Matches(@"^[a-z]{2}-[A-Z]{2}$", Language.English);
            Assert.Matches(@"^[a-z]{2}-[A-Z]{2}$", Language.ChineseTraditional);
            Assert.Matches(@"^[a-z]{2}-[A-Z]{2}$", Language.ChineseSimplified);
            Assert.Matches(@"^[a-z]{2}-[A-Z]{2}$", Language.Japanese);
            Assert.Matches(@"^[a-z]{2}-[A-Z]{2}$", Language.Thai);
            Assert.Matches(@"^[a-z]{2}-[A-Z]{2}$", Language.Korean);
            Assert.Matches(@"^[a-z]{2}-[A-Z]{2}$", Language.Vietnamese);
        }

        [Fact]
        public void AllLanguages_AreUnique()
        {
            // Arrange
            var languages = new[]
            {
                Language.English,
                Language.ChineseTraditional,
                Language.ChineseSimplified,
                Language.Japanese,
                Language.Thai,
                Language.Korean,
                Language.Vietnamese
            };

            // Assert
            Assert.Equal(languages.Length, languages.Distinct().Count());
        }

        [Fact]
        public void LanguageConstants_AreConsistent()
        {
            // Assert - Test that constants don't change accidentally
            Assert.Equal("en-US", Language.English);
            Assert.Equal("zh-TW", Language.ChineseTraditional);
            Assert.Equal("zh-CN", Language.ChineseSimplified);
            Assert.Equal("ja-JP", Language.Japanese);
            Assert.Equal("th-TH", Language.Thai);
            Assert.Equal("ko-KR", Language.Korean);
            Assert.Equal("vi-VN", Language.Vietnamese);
            Assert.Equal("en-US", Language.Default);
        }

        [Theory]
        [InlineData("en-US")]
        [InlineData("zh-TW")]
        [InlineData("zh-CN")]
        [InlineData("ja-JP")]
        [InlineData("th-TH")]
        [InlineData("ko-KR")]
        [InlineData("vi-VN")]
        public void LanguageConstants_ContainExpectedValues(string expectedValue)
        {
            // Arrange
            var allLanguages = new[]
            {
                Language.English,
                Language.ChineseTraditional,
                Language.ChineseSimplified,
                Language.Japanese,
                Language.Thai,
                Language.Korean,
                Language.Vietnamese
            };

            // Assert
            Assert.Contains(expectedValue, allLanguages);
        }

        [Fact]
        public void Default_IsOneOfTheLanguages()
        {
            // Arrange
            var allLanguages = new[]
            {
                Language.English,
                Language.ChineseTraditional,
                Language.ChineseSimplified,
                Language.Japanese,
                Language.Thai,
                Language.Korean,
                Language.Vietnamese
            };

            // Assert
            Assert.Contains(Language.Default, allLanguages);
        }
    }
}
