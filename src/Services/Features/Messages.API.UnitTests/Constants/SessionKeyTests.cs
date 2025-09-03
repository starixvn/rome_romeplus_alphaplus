using AlphaPlus.Features.Messages.API.Constants;
using Xunit;

namespace AlphaPlus.Features.Messages.API.UnitTests.Constants
{
    public class SessionKeyTests
    {
        [Fact]
        public void CustomerDatelimit_HasCorrectValue()
        {
            // Assert
            Assert.Equal("CustomerDateLimit", SessionKey.CustomerDatelimit);
        }

        [Fact]
        public void ShowedPopupAnnouncement_HasCorrectValue()
        {
            // Assert
            Assert.Equal("ShowedPopupAnnouncement", SessionKey.ShowedPopupAnnouncement);
        }

        [Fact]
        public void NumberOfUnreadMessage_HasCorrectValue()
        {
            // Assert
            Assert.Equal("NumberOfUnreadMessage", SessionKey.NumberOfUnreadMessage);
        }

        [Fact]
        public void AllKeys_AreNotNull()
        {
            // Assert
            Assert.NotNull(SessionKey.CustomerDatelimit);
            Assert.NotNull(SessionKey.ShowedPopupAnnouncement);
            Assert.NotNull(SessionKey.NumberOfUnreadMessage);
        }

        [Fact]
        public void AllKeys_AreNotEmpty()
        {
            // Assert
            Assert.NotEmpty(SessionKey.CustomerDatelimit);
            Assert.NotEmpty(SessionKey.ShowedPopupAnnouncement);
            Assert.NotEmpty(SessionKey.NumberOfUnreadMessage);
        }

        [Fact]
        public void AllKeys_AreUnique()
        {
            // Arrange
            var keys = new[]
            {
                SessionKey.CustomerDatelimit,
                SessionKey.ShowedPopupAnnouncement,
                SessionKey.NumberOfUnreadMessage
            };

            // Assert
            Assert.Equal(keys.Length, keys.Distinct().Count());
        }

        [Fact]
        public void AllKeys_AreStrings()
        {
            // Assert
            Assert.IsType<string>(SessionKey.CustomerDatelimit);
            Assert.IsType<string>(SessionKey.ShowedPopupAnnouncement);
            Assert.IsType<string>(SessionKey.NumberOfUnreadMessage);
        }

        [Theory]
        [InlineData("CustomerDateLimit")]
        [InlineData("ShowedPopupAnnouncement")]
        [InlineData("NumberOfUnreadMessage")]
        public void Constants_ContainExpectedValues(string expectedValue)
        {
            // Arrange
            var allKeys = new[]
            {
                SessionKey.CustomerDatelimit,
                SessionKey.ShowedPopupAnnouncement,
                SessionKey.NumberOfUnreadMessage
            };

            // Assert
            Assert.Contains(expectedValue, allKeys);
        }

        [Fact]
        public void Keys_AreConsistentAcrossAccess()
        {
            // Act
            var key1_1 = SessionKey.CustomerDatelimit;
            var key1_2 = SessionKey.CustomerDatelimit;
            var key2_1 = SessionKey.ShowedPopupAnnouncement;
            var key2_2 = SessionKey.ShowedPopupAnnouncement;
            var key3_1 = SessionKey.NumberOfUnreadMessage;
            var key3_2 = SessionKey.NumberOfUnreadMessage;

            // Assert
            Assert.Equal(key1_1, key1_2);
            Assert.Equal(key2_1, key2_2);
            Assert.Equal(key3_1, key3_2);
        }

        [Fact]
        public void Keys_FollowNamingConvention()
        {
            // Assert - Keys should follow PascalCase convention for session keys
            Assert.Matches(@"^[A-Z][a-zA-Z]*$", SessionKey.CustomerDatelimit);
            Assert.Matches(@"^[A-Z][a-zA-Z]*$", SessionKey.ShowedPopupAnnouncement);
            Assert.Matches(@"^[A-Z][a-zA-Z]*$", SessionKey.NumberOfUnreadMessage);
        }

        [Fact]
        public void ClassHasExpectedNumberOfConstants()
        {
            // Arrange
            var constantFields = typeof(SessionKey)
                .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.DeclaredOnly)
                .Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string));

            // Assert
            Assert.Equal(3, constantFields.Count());
        }

        [Fact]
        public void Constants_ArePublicStatic()
        {
            // Arrange
            var type = typeof(SessionKey);
            var customerDateLimitField = type.GetField(nameof(SessionKey.CustomerDatelimit));
            var showedPopupField = type.GetField(nameof(SessionKey.ShowedPopupAnnouncement));
            var unreadMessageField = type.GetField(nameof(SessionKey.NumberOfUnreadMessage));

            // Assert
            Assert.NotNull(customerDateLimitField);
            Assert.NotNull(showedPopupField);
            Assert.NotNull(unreadMessageField);
            
            Assert.True(customerDateLimitField.IsPublic);
            Assert.True(customerDateLimitField.IsStatic);
            Assert.True(showedPopupField.IsPublic);
            Assert.True(showedPopupField.IsStatic);
            Assert.True(unreadMessageField.IsPublic);
            Assert.True(unreadMessageField.IsStatic);
        }

        [Fact]
        public void Keys_HaveReasonableLength()
        {
            // Assert - Session keys should not be too short or too long
            Assert.InRange(SessionKey.CustomerDatelimit.Length, 5, 50);
            Assert.InRange(SessionKey.ShowedPopupAnnouncement.Length, 5, 50);
            Assert.InRange(SessionKey.NumberOfUnreadMessage.Length, 5, 50);
        }

        [Fact]
        public void Keys_DoNotContainSpecialCharacters()
        {
            // Assert - Session keys should not contain special characters for safety
            Assert.DoesNotContain(" ", SessionKey.CustomerDatelimit);
            Assert.DoesNotContain(" ", SessionKey.ShowedPopupAnnouncement);
            Assert.DoesNotContain(" ", SessionKey.NumberOfUnreadMessage);
            
            Assert.DoesNotContain(".", SessionKey.CustomerDatelimit);
            Assert.DoesNotContain(".", SessionKey.ShowedPopupAnnouncement);
            Assert.DoesNotContain(".", SessionKey.NumberOfUnreadMessage);
        }

        [Fact]
        public void Keys_AreDescriptive()
        {
            // Assert - Keys should be descriptive of their purpose
            Assert.Contains("Customer", SessionKey.CustomerDatelimit);
            Assert.Contains("Date", SessionKey.CustomerDatelimit);
            
            Assert.Contains("Popup", SessionKey.ShowedPopupAnnouncement);
            Assert.Contains("Announcement", SessionKey.ShowedPopupAnnouncement);
            
            Assert.Contains("Unread", SessionKey.NumberOfUnreadMessage);
            Assert.Contains("Message", SessionKey.NumberOfUnreadMessage);
        }
    }
}
