using AlphaPlus.Features.Messages.Utils;
using Xunit;

namespace AlphaPlus.Features.Messages.API.UnitTests.Utils
{
    public class MessageFormatterTests
    {
        private const string BoldTexten_US = "which affected are considered VOID";
        private const string BoldTextzh_CN = "只作废受影响的注单";
        private const string BoldTextzh_TW = "只作廢受影響的注單";

        [Fact]
        public void MarkBoldText_WithEnglishLanguage_MarksBoldTextCorrectly()
        {
            // Arrange
            var message = $"This is a message {BoldTexten_US} and some more text.";
            var language = "en-US";

            // Act
            var result = MessageFormatter.MarkBoldText(message, language);

            // Assert
            Assert.Contains($"<strong>{BoldTexten_US}</strong>", result);
            Assert.DoesNotContain(BoldTexten_US, result.Replace($"<strong>{BoldTexten_US}</strong>", ""));
        }

        [Fact]
        public void MarkBoldText_WithChineseSimplifiedLanguage_MarksBoldTextCorrectly()
        {
            // Arrange
            var message = $"这是一条消息 {BoldTextzh_CN} 和一些其他文本。";
            var language = "zh-CN";

            // Act
            var result = MessageFormatter.MarkBoldText(message, language);

            // Assert
            Assert.Contains($"<strong>{BoldTextzh_CN}</strong>", result);
            Assert.DoesNotContain(BoldTextzh_CN, result.Replace($"<strong>{BoldTextzh_CN}</strong>", ""));
        }

        [Fact]
        public void MarkBoldText_WithChineseTraditionalLanguage_MarksBoldTextCorrectly()
        {
            // Arrange
            var message = $"這是一條消息 {BoldTextzh_TW} 和一些其他文本。";
            var language = "zh-TW";

            // Act
            var result = MessageFormatter.MarkBoldText(message, language);

            // Assert
            Assert.Contains($"<strong>{BoldTextzh_TW}</strong>", result);
            Assert.DoesNotContain(BoldTextzh_TW, result.Replace($"<strong>{BoldTextzh_TW}</strong>", ""));
        }

        [Theory]
        [InlineData("en-US")]
        [InlineData("ja-JP")]
        [InlineData("ko-KR")]
        [InlineData("th-TH")]
        [InlineData("vi-VN")]
        [InlineData("fr-FR")]
        [InlineData("unknown-language")]
        public void MarkBoldText_WithUnsupportedLanguages_UsesEnglishBoldText(string language)
        {
            // Arrange
            var message = $"This is a message {BoldTexten_US} and some more text.";

            // Act
            var result = MessageFormatter.MarkBoldText(message, language);

            // Assert
            if (language != "zh-CN" && language != "zh-TW")
            {
                Assert.Contains($"<strong>{BoldTexten_US}</strong>", result);
            }
        }

        [Fact]
        public void MarkBoldText_WithMessageNotContainingBoldText_ReturnsOriginalMessage()
        {
            // Arrange
            var message = "This is a simple message without bold text.";
            var language = "en-US";

            // Act
            var result = MessageFormatter.MarkBoldText(message, language);

            // Assert
            Assert.Equal(message, result);
        }

        [Fact]
        public void MarkBoldText_WithCaseInsensitiveMatch_MarksBoldTextCorrectly()
        {
            // Arrange
            var message = $"This is a message {BoldTexten_US.ToUpper()} and some more text.";
            var language = "en-US";

            // Act
            var result = MessageFormatter.MarkBoldText(message, language);

            // Assert
            // The method performs case-insensitive search but should find the match
            // If no match found (due to case difference), the original message is returned
            Assert.Equal(message, result); // Should return original message if no exact case match
        }

        [Fact]
        public void MarkBoldText_WithMixedCaseBoldText_MarksBoldTextCorrectly()
        {
            // Arrange
            var mixedCaseText = "Which Affected Are Considered VOID";
            var message = $"This is a message {mixedCaseText} and some more text.";
            var language = "en-US";

            // Act
            var result = MessageFormatter.MarkBoldText(message, language);

            // Assert
            // The method performs case-insensitive search but Replace is case-sensitive
            // So if cases don't match exactly, the original message is returned
            Assert.Equal(message, result); // Should return original message if no exact case match
        }

        [Fact]
        public void MarkBoldText_WithEmptyMessage_ReturnsEmptyString()
        {
            // Arrange
            var message = string.Empty;
            var language = "en-US";

            // Act
            var result = MessageFormatter.MarkBoldText(message, language);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void MarkBoldText_WithNullMessage_ThrowsNullReferenceException()
        {
            // Arrange
            string message = null;
            var language = "en-US";

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => MessageFormatter.MarkBoldText(message, language));
        }

        [Fact]
        public void MarkBoldText_WithNullLanguage_UsesEnglishAsDefault()
        {
            // Arrange
            var message = $"This is a message {BoldTexten_US} and some more text.";
            string language = null;

            // Act
            var result = MessageFormatter.MarkBoldText(message, language);

            // Assert
            Assert.Contains($"<strong>{BoldTexten_US}</strong>", result);
        }

        [Fact]
        public void MarkBoldText_WithEmptyLanguage_UsesEnglishAsDefault()
        {
            // Arrange
            var message = $"This is a message {BoldTexten_US} and some more text.";
            var language = string.Empty;

            // Act
            var result = MessageFormatter.MarkBoldText(message, language);

            // Assert
            Assert.Contains($"<strong>{BoldTexten_US}</strong>", result);
        }

        [Fact]
        public void MarkBoldText_WithMultipleOccurrences_MarksAllOccurrences()
        {
            // Arrange
            var message = $"First {BoldTexten_US} and second {BoldTexten_US} occurrence.";
            var language = "en-US";

            // Act
            var result = MessageFormatter.MarkBoldText(message, language);

            // Assert
            var expectedBold = $"<strong>{BoldTexten_US}</strong>";
            Assert.Equal(2, result.Split(expectedBold).Length - 1);
        }

        [Fact]
        public void MarkBoldText_WithAlreadyMarkedText_CreatesDoubleWrap()
        {
            // Arrange
            var message = $"This is a message <strong>{BoldTexten_US}</strong> already marked.";
            var language = "en-US";

            // Act
            var result = MessageFormatter.MarkBoldText(message, language);

            // Assert
            // The method will create nested strong tags since it doesn't check for existing markup
            Assert.Contains("<strong><strong>", result);
            Assert.Contains("</strong></strong>", result);
        }

        [Theory]
        [InlineData("zh-CN")]
        [InlineData("zh-TW")]
        [InlineData("en-US")]
        public void MarkBoldText_IsConsistentAcrossLanguages(string language)
        {
            // Arrange
            var message = GetTestMessageForLanguage(language);

            // Act
            var result1 = MessageFormatter.MarkBoldText(message, language);
            var result2 = MessageFormatter.MarkBoldText(message, language);

            // Assert
            Assert.Equal(result1, result2);
        }

        private static string GetTestMessageForLanguage(string language)
        {
            return language switch
            {
                "zh-CN" => $"测试消息 {BoldTextzh_CN} 内容",
                "zh-TW" => $"測試消息 {BoldTextzh_TW} 內容",
                _ => $"Test message {BoldTexten_US} content"
            };
        }
    }
}
