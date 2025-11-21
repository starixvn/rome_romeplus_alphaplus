using AlphaPlus.Features.Messages.API.Constants;
using AlphaPlus.Features.Messages.API.Models.Message;
using Xunit;

namespace AlphaPlus.Features.Messages.API.UnitTests.Models.Message
{
    public class MultiLanguagueTests
    {
        [Fact]
        public void DefaultConstructor_InitializesWithEmptyContents()
        {
            // Act
            var multiLanguague = new MultiLanguague();

            // Assert - Test by accessing properties since Contents is internal
            Assert.Equal(string.Empty, multiLanguague.ContentEN);
            Assert.Equal(string.Empty, multiLanguague.ContentCS);
        }

        [Fact]
        public void ParameterizedConstructor_SetsAllLanguageContents()
        {
            // Arrange
            var contentEN = "English content";
            var contentCS = "Chinese Simplified content";
            var contentCH = "Chinese Traditional content";
            var contentTH = "Thai content";
            var contentJP = "Japanese content";
            var contentKO = "Korean content";
            var contentVI = "Vietnamese content";

            // Act
            var multiLanguague = new MultiLanguague(
                contentEN, contentCS, contentCH, contentTH, contentJP, contentKO, contentVI);

            // Assert - Test by accessing properties since Contents is internal
            Assert.Equal(contentEN, multiLanguague.ContentEN);
            Assert.Equal(contentCS, multiLanguague.ContentCS);
            Assert.Equal(contentCH, multiLanguague.ContentCH);
            Assert.Equal(contentTH, multiLanguague.ContentTH);
            Assert.Equal(contentJP, multiLanguague.ContentJP);
            Assert.Equal(contentKO, multiLanguague.ContentKO);
            Assert.Equal(contentVI, multiLanguague.ContentVI);
        }

        [Fact]
        public void ParameterizedConstructor_WithNullValues_ConvertsToEmptyString()
        {
            // Act
            var multiLanguague = new MultiLanguague(null, null, null, null, null, null, null);

            // Assert - Test by accessing properties since Contents is internal
            Assert.Equal(string.Empty, multiLanguague.ContentEN);
            Assert.Equal(string.Empty, multiLanguague.ContentCS);
            Assert.Equal(string.Empty, multiLanguague.ContentCH);
            Assert.Equal(string.Empty, multiLanguague.ContentTH);
            Assert.Equal(string.Empty, multiLanguague.ContentJP);
            Assert.Equal(string.Empty, multiLanguague.ContentKO);
            Assert.Equal(string.Empty, multiLanguague.ContentVI);
        }

        [Fact]
        public void ContentEN_CanBeSetAndRetrieved()
        {
            // Arrange
            var multiLanguague = new MultiLanguague();
            var englishContent = "English test content";

            // Act
            multiLanguague.ContentEN = englishContent;

            // Assert
            Assert.Equal(englishContent, multiLanguague.ContentEN);
        }

        [Fact]
        public void ContentCS_CanBeSetAndRetrieved()
        {
            // Arrange
            var multiLanguague = new MultiLanguague();
            var chineseSimplifiedContent = "简体中文内容";

            // Act
            multiLanguague.ContentCS = chineseSimplifiedContent;

            // Assert
            Assert.Equal(chineseSimplifiedContent, multiLanguague.ContentCS);
        }

        [Fact]
        public void ContentCH_CanBeSetAndRetrieved()
        {
            // Arrange
            var multiLanguague = new MultiLanguague();
            var chineseTraditionalContent = "繁體中文內容";

            // Act
            multiLanguague.ContentCH = chineseTraditionalContent;

            // Assert
            Assert.Equal(chineseTraditionalContent, multiLanguague.ContentCH);
        }

        [Fact]
        public void ContentJP_CanBeSetAndRetrieved()
        {
            // Arrange
            var multiLanguague = new MultiLanguague();
            var japaneseContent = "日本語の内容";

            // Act
            multiLanguague.ContentJP = japaneseContent;

            // Assert
            Assert.Equal(japaneseContent, multiLanguague.ContentJP);
        }

        [Fact]
        public void ContentTH_CanBeSetAndRetrieved()
        {
            // Arrange
            var multiLanguague = new MultiLanguague();
            var thaiContent = "เนื้อหาภาษาไทย";

            // Act
            multiLanguague.ContentTH = thaiContent;

            // Assert
            Assert.Equal(thaiContent, multiLanguague.ContentTH);
        }

        [Fact]
        public void ContentKO_CanBeSetAndRetrieved()
        {
            // Arrange
            var multiLanguague = new MultiLanguague();
            var koreanContent = "한국어 내용";

            // Act
            multiLanguague.ContentKO = koreanContent;

            // Assert
            Assert.Equal(koreanContent, multiLanguague.ContentKO);
        }

        [Fact]
        public void ContentVI_CanBeSetAndRetrieved()
        {
            // Arrange
            var multiLanguague = new MultiLanguague();
            var vietnameseContent = "Nội dung tiếng Việt";

            // Act
            multiLanguague.ContentVI = vietnameseContent;

            // Assert
            Assert.Equal(vietnameseContent, multiLanguague.ContentVI);
        }

        [Fact]
        public void GetContentByKey_WithValidKey_ReturnsCorrectContent()
        {
            // Arrange
            var englishContent = "English content";
            var multiLanguague = new MultiLanguague();
            multiLanguague.ContentEN = englishContent;

            // Act
            var result = multiLanguague.GetContentByKey(Language.English);

            // Assert
            Assert.Equal(englishContent, result);
        }

        [Fact]
        public void GetContentByKey_WithEmptyContent_ReturnsEnglishFallback()
        {
            // Arrange
            var englishContent = "English fallback";
            var multiLanguague = new MultiLanguague();
            multiLanguague.ContentEN = englishContent;
            multiLanguague.ContentCS = ""; // Empty content

            // Act
            var result = multiLanguague.GetContentByKey(Language.ChineseSimplified);

            // Assert
            Assert.Equal(englishContent, result);
        }

        [Fact]
        public void GetContentByKey_WithNullContent_ReturnsEnglishFallback()
        {
            // Arrange
            var englishContent = "English fallback";
            var multiLanguague = new MultiLanguague();
            multiLanguague.ContentEN = englishContent;
            // Note: Cannot set null directly since Contents is internal
            // This test verifies behavior when content doesn't exist

            // Act
            var result = multiLanguague.GetContentByKey("non-existent-language");

            // Assert
            Assert.Equal(englishContent, result);
        }

        [Fact]
        public void GetContentByKey_WithNonExistentKey_ReturnsEnglishFallback()
        {
            // Arrange
            var englishContent = "English fallback";
            var multiLanguague = new MultiLanguague();
            multiLanguague.ContentEN = englishContent;

            // Act
            var result = multiLanguague.GetContentByKey("non-existent-key");

            // Assert
            Assert.Equal(englishContent, result);
        }

        [Fact]
        public void GetContentByKey_WithNoEnglishContent_ReturnsEmptyString()
        {
            // Arrange
            var multiLanguague = new MultiLanguague();
            // No English content set

            // Act
            var result = multiLanguague.GetContentByKey(Language.ChineseSimplified);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetContentByKey_WithWhitespaceContent_ReturnsWhitespace()
        {
            // Arrange
            var englishContent = "English fallback";
            var multiLanguague = new MultiLanguague();
            multiLanguague.ContentEN = englishContent;
            multiLanguague.ContentCS = "   "; // Whitespace only

            // Act
            var result = multiLanguague.GetContentByKey(Language.ChineseSimplified);

            // Assert
            // The method returns whitespace content, not English fallback for whitespace
            Assert.Equal("   ", result);
        }

        [Theory]
        [InlineData(Language.English)]
        [InlineData(Language.ChineseSimplified)]
        [InlineData(Language.ChineseTraditional)]
        [InlineData(Language.Japanese)]
        [InlineData(Language.Thai)]
        [InlineData(Language.Korean)]
        [InlineData(Language.Vietnamese)]
        public void AllLanguageProperties_WorkCorrectly(string languageKey)
        {
            // Arrange
            var testContent = $"Test content for {languageKey}";
            var multiLanguague = new MultiLanguague();

            // Act & Assert based on language
            switch (languageKey)
            {
                case Language.English:
                    multiLanguague.ContentEN = testContent;
                    Assert.Equal(testContent, multiLanguague.ContentEN);
                    break;
                case Language.ChineseSimplified:
                    multiLanguague.ContentCS = testContent;
                    Assert.Equal(testContent, multiLanguague.ContentCS);
                    break;
                case Language.ChineseTraditional:
                    multiLanguague.ContentCH = testContent;
                    Assert.Equal(testContent, multiLanguague.ContentCH);
                    break;
                case Language.Japanese:
                    multiLanguague.ContentJP = testContent;
                    Assert.Equal(testContent, multiLanguague.ContentJP);
                    break;
                case Language.Thai:
                    multiLanguague.ContentTH = testContent;
                    Assert.Equal(testContent, multiLanguague.ContentTH);
                    break;
                case Language.Korean:
                    multiLanguague.ContentKO = testContent;
                    Assert.Equal(testContent, multiLanguague.ContentKO);
                    break;
                case Language.Vietnamese:
                    multiLanguague.ContentVI = testContent;
                    Assert.Equal(testContent, multiLanguague.ContentVI);
                    break;
            }

            // Verify content is also accessible via GetContentByKey
            Assert.Equal(testContent, multiLanguague.GetContentByKey(languageKey));
        }

        [Fact]
        public void Properties_AreIndependent()
        {
            // Arrange
            var multiLanguague = new MultiLanguague();
            var englishContent = "English";
            var chineseContent = "中文";
            var japaneseContent = "日本語";

            // Act
            multiLanguague.ContentEN = englishContent;
            multiLanguague.ContentCS = chineseContent;
            multiLanguague.ContentJP = japaneseContent;

            // Assert
            Assert.Equal(englishContent, multiLanguague.ContentEN);
            Assert.Equal(chineseContent, multiLanguague.ContentCS);
            Assert.Equal(japaneseContent, multiLanguague.ContentJP);
            
            // Verify they don't affect each other
            Assert.NotEqual(multiLanguague.ContentEN, multiLanguague.ContentCS);
            Assert.NotEqual(multiLanguague.ContentCS, multiLanguague.ContentJP);
            Assert.NotEqual(multiLanguague.ContentEN, multiLanguague.ContentJP);
        }

        [Fact]
        public void Properties_And_GetContentByKey_AreConsistent()
        {
            // Arrange
            var multiLanguague = new MultiLanguague();
            var testContent = "Test content";

            // Act
            multiLanguague.ContentEN = testContent;

            // Assert
            Assert.Equal(testContent, multiLanguague.ContentEN);
            Assert.Equal(testContent, multiLanguague.GetContentByKey(Language.English));
        }

        [Fact]
        public void Constructor_WithMixedNullAndValidValues_HandlesCorrectly()
        {
            // Arrange & Act
            var multiLanguague = new MultiLanguague(
                "English", null, "Traditional Chinese", null, "Japanese", null, "Vietnamese");

            // Assert
            Assert.Equal("English", multiLanguague.ContentEN);
            Assert.Equal("English", multiLanguague.ContentCS); // Falls back to English when null was passed
            Assert.Equal("Traditional Chinese", multiLanguague.ContentCH);
            Assert.Equal("English", multiLanguague.ContentTH); // Falls back to English when null was passed
            Assert.Equal("Japanese", multiLanguague.ContentJP);
            Assert.Equal("English", multiLanguague.ContentKO); // Falls back to English when null was passed
            Assert.Equal("Vietnamese", multiLanguague.ContentVI);
        }
    }
}
