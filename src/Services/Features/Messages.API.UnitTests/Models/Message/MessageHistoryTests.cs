using AlphaPlus.Features.Messages.API.Constants;
using AlphaPlus.Features.Messages.API.Models.Message;
using Xunit;

namespace AlphaPlus.Features.Messages.API.UnitTests.Models.Message
{
    public class MessageHistoryTests
    {
        [Fact]
        public void DefaultConstructor_InitializesWithDefaults()
        {
            // Act
            var messageHistory = new MessageHistory();

            // Assert
            Assert.Equal(0, messageHistory.Id);
            Assert.Equal(default(DateTime), messageHistory.PostTime);
            Assert.Equal(0, messageHistory.MessageType);
            Assert.False(messageHistory.IsSticky);
            Assert.Equal(string.Empty, messageHistory.BetTransId);
            Assert.Equal(0, messageHistory.TotalMessasge);
            Assert.Equal(0, messageHistory.RowNum);
        }

        [Fact]
        public void ParameterizedConstructor_SetsPropertiesCorrectly()
        {
            // Arrange
            var id = 123;
            var contentEN = "English content";
            var contentCS = "Chinese Simplified content";
            var contentCH = "Chinese Traditional content";
            var contentJP = "Japanese content";
            var contentTH = "Thai content";
            var contentKO = "Korean content";
            var contentVI = "Vietnamese content";
            var postTime = new DateTime(2024, 1, 15);
            byte messageType = 1;
            var isSticky = true;
            var betTransId = "BET123";
            long rowNum = 456;

            // Act
            var messageHistory = new MessageHistory(
                id, contentEN, contentCS, contentCH, contentJP, contentTH, contentKO, contentVI,
                postTime, messageType, isSticky, betTransId, rowNum);

            // Assert
            Assert.Equal(id, messageHistory.Id);
            Assert.Equal(postTime, messageHistory.PostTime);
            Assert.Equal(messageType, messageHistory.MessageType);
            Assert.Equal(isSticky, messageHistory.IsSticky);
            Assert.Equal(betTransId, messageHistory.BetTransId);
            Assert.Equal(rowNum, messageHistory.RowNum);
            
            // Verify inherited properties from MultiLanguague
            Assert.Equal(contentEN, messageHistory.ContentEN);
            Assert.Equal(contentCS, messageHistory.ContentCS);
            Assert.Equal(contentCH, messageHistory.ContentCH);
            Assert.Equal(contentJP, messageHistory.ContentJP);
            Assert.Equal(contentTH, messageHistory.ContentTH);
            Assert.Equal(contentKO, messageHistory.ContentKO);
            Assert.Equal(contentVI, messageHistory.ContentVI);
        }

        [Fact]
        public void Properties_CanBeSetAndRetrieved()
        {
            // Arrange
            var messageHistory = new MessageHistory();
            var id = 789;
            var postTime = new DateTime(2024, 6, 1);
            byte messageType = 2;
            var isSticky = true;
            var betTransId = "TRANS456";
            var totalMessage = 100;
            long rowNum = 999;

            // Act
            messageHistory.Id = id;
            messageHistory.PostTime = postTime;
            messageHistory.MessageType = messageType;
            messageHistory.IsSticky = isSticky;
            messageHistory.BetTransId = betTransId;
            messageHistory.TotalMessasge = totalMessage;
            messageHistory.RowNum = rowNum;

            // Assert
            Assert.Equal(id, messageHistory.Id);
            Assert.Equal(postTime, messageHistory.PostTime);
            Assert.Equal(messageType, messageHistory.MessageType);
            Assert.Equal(isSticky, messageHistory.IsSticky);
            Assert.Equal(betTransId, messageHistory.BetTransId);
            Assert.Equal(totalMessage, messageHistory.TotalMessasge);
            Assert.Equal(rowNum, messageHistory.RowNum);
        }

        [Fact]
        public void HasRedColor_WhenContentContainsRedMarkup_ReturnsTrue()
        {
            // Arrange
            var messageHistory = new MessageHistory();
            messageHistory.ContentEN = $"This is {MessageConstants.ContentFontStartMarkup}red text{MessageConstants.ContentFontEndMarkup}";

            // Act
            var result = messageHistory.HasRedColor;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void HasRedColor_WhenContentDoesNotContainRedMarkup_ReturnsFalse()
        {
            // Arrange
            var messageHistory = new MessageHistory();
            messageHistory.ContentEN = "This is normal text";

            // Act
            var result = messageHistory.HasRedColor;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void HasRedColor_WhenContentIsEmpty_ReturnsFalse()
        {
            // Arrange
            var messageHistory = new MessageHistory();
            messageHistory.ContentEN = string.Empty;

            // Act
            var result = messageHistory.HasRedColor;

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(Language.English)]
        [InlineData(Language.ChineseSimplified)]
        [InlineData(Language.ChineseTraditional)]
        [InlineData(Language.Japanese)]
        [InlineData(Language.Thai)]
        [InlineData(Language.Korean)]
        [InlineData(Language.Vietnamese)]
        public void Content_WithNormalMessageType_RemovesMarkupAndReplacesBetTransId(string language)
        {
            // Arrange
            var betTransId = "BET123";
            var rawContent = $"{MessageConstants.ContentFontStartMarkup}Important message{MessageConstants.ContentFontEndMarkup} for @@";
            var messageHistory = new MessageHistory();
            messageHistory.MessageType = (byte)MessageType.Normal;
            messageHistory.BetTransId = betTransId;
            
            // Set content for the specific language
            SetContentForLanguage(messageHistory, language, rawContent);

            // Act
            var result = messageHistory.Content(language);

            // Assert
            Assert.DoesNotContain(MessageConstants.ContentFontStartMarkup, result);
            Assert.DoesNotContain(MessageConstants.ContentFontEndMarkup, result);
            Assert.Contains(betTransId, result);
            Assert.DoesNotContain("@@", result);
            Assert.Contains("Important message", result);
        }

        [Theory]
        [InlineData(Language.English, "en-US")]
        [InlineData(Language.ChineseSimplified, "zh-CN")]
        [InlineData(Language.ChineseTraditional, "zh-TW")]
        public void Content_WithPersonalMessageType_AddsBoldMarkupForSupportedLanguages(string language, string languageCode)
        {
            // Arrange
            var messageHistory = new MessageHistory();
            messageHistory.MessageType = (byte)MessageType.Personal;
            messageHistory.BetTransId = "BET123";
            
            var content = GetBoldTextForLanguage(languageCode);
            SetContentForLanguage(messageHistory, language, $"Test message {content} here");

            // Act
            var result = messageHistory.Content(language);

            // Assert
            Assert.Contains($"<strong>{content}</strong>", result);
        }

        [Fact]
        public void Content_WithPersonalMessageType_DoesNotAddBoldForUnsupportedLanguages()
        {
            // Arrange
            var messageHistory = new MessageHistory();
            messageHistory.MessageType = (byte)MessageType.Personal;
            messageHistory.BetTransId = "BET123";
            messageHistory.ContentJP = "Japanese message without bold text";

            // Act
            var result = messageHistory.Content(Language.Japanese);

            // Assert
            Assert.DoesNotContain("<strong>", result);
        }

        [Fact]
        public void Content_ReplacesDoubleBetTransIdPlaceholder()
        {
            // Arrange
            var betTransId = "TRANSACTION123";
            var messageHistory = new MessageHistory();
            messageHistory.BetTransId = betTransId;
            messageHistory.ContentEN = "Your bet @@ has been processed";

            // Act
            var result = messageHistory.Content(Language.English);

            // Assert
            Assert.Contains(betTransId, result);
            Assert.DoesNotContain("@@", result);
            Assert.Equal($"Your bet {betTransId} has been processed", result);
        }

        [Fact]
        public void Content_WithEmptyBetTransId_ReplacesWithEmptyString()
        {
            // Arrange
            var messageHistory = new MessageHistory();
            messageHistory.BetTransId = string.Empty;
            messageHistory.ContentEN = "Your bet @@ status";

            // Act
            var result = messageHistory.Content(Language.English);

            // Assert
            Assert.Equal("Your bet  status", result);
        }

        [Fact]
        public void Content_WithMultiplePlaceholders_ReplacesAll()
        {
            // Arrange
            var betTransId = "ABC123";
            var messageHistory = new MessageHistory();
            messageHistory.BetTransId = betTransId;
            messageHistory.ContentEN = "Bet @@ processed for transaction @@";

            // Act
            var result = messageHistory.Content(Language.English);

            // Assert
            Assert.Equal($"Bet {betTransId} processed for transaction {betTransId}", result);
        }

        [Fact]
        public void Content_WithNullOrEmptyContent_ReturnsProcessedEmptyString()
        {
            // Arrange
            var messageHistory = new MessageHistory();
            messageHistory.BetTransId = "TEST";

            // Act
            var result = messageHistory.Content(Language.English);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [InlineData("unknown-language")]
        [InlineData("")]
        public void Content_WithInvalidLanguage_FallsBackToEnglish(string language)
        {
            // Arrange
            var messageHistory = new MessageHistory();
            messageHistory.BetTransId = "TEST";
            messageHistory.ContentEN = "English content for @@";

            // Act
            var result = messageHistory.Content(language);

            // Assert
            Assert.Contains("English content for TEST", result);
        }

        [Fact]
        public void Content_WithNullLanguage_ThrowsArgumentNullException()
        {
            // Arrange
            var messageHistory = new MessageHistory();
            messageHistory.BetTransId = "TEST";
            messageHistory.ContentEN = "English content for @@";

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => messageHistory.Content(null));
        }

        private static void SetContentForLanguage(MessageHistory messageHistory, string language, string content)
        {
            switch (language)
            {
                case Language.English:
                    messageHistory.ContentEN = content;
                    break;
                case Language.ChineseSimplified:
                    messageHistory.ContentCS = content;
                    break;
                case Language.ChineseTraditional:
                    messageHistory.ContentCH = content;
                    break;
                case Language.Japanese:
                    messageHistory.ContentJP = content;
                    break;
                case Language.Thai:
                    messageHistory.ContentTH = content;
                    break;
                case Language.Korean:
                    messageHistory.ContentKO = content;
                    break;
                case Language.Vietnamese:
                    messageHistory.ContentVI = content;
                    break;
            }
        }

        private static string GetBoldTextForLanguage(string languageCode)
        {
            return languageCode switch
            {
                "zh-CN" => "只作废受影响的注单",
                "zh-TW" => "只作廢受影響的注單",
                _ => "which affected are considered VOID"
            };
        }
    }
}
