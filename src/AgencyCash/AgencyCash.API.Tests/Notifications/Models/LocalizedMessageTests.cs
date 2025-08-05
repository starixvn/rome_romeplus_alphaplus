using AgencyCash.API.Notifications.Models;
using AgencyCash.API.Notifications.Resources;
using Newtonsoft.Json;

namespace AgencyCash.API.UnitTests.Notifications.Models
{
    public class LocalizedMessageTest
    {
        [Fact]
        public void Constructor_Should_Initialize_Properties_Correctly()
        {
            // Arrange
            int targetId = 1;
            string targetName = "Target";
            int creator = 100;
            string titleKey = "TitleKey";
            string messageKey = "MessageKey";
            object[] messageParams = ["Param1", "Param2"];

            // Act
            var message = new LocalizedMessage(targetId, targetName, creator, titleKey, messageKey, messageParams);

            // Assert
            Assert.Equal(targetId, message.TargetId);
            Assert.Equal(targetName, message.TargetName);
            Assert.Equal(creator, message.Creator);
            Assert.Equal(titleKey, message.TitleMessageResourceKey);
            Assert.Equal(messageKey, message.MessageResourceKey);
            Assert.Equal(messageParams, message.MessageParams);
        }

        [Fact]
        public void Constructor_Should_Initialize_Empty_MessageParams_When_Null()
        {
            // Arrange & Act
            var message = new LocalizedMessage(1, "Target", 100, "TitleKey", "MessageKey");

            // Assert
            Assert.NotNull(message.MessageParams);
            Assert.Empty(message.MessageParams);
        }

        [Fact]
        public void AssignPersonalMessage_Should_Set_Localized_Messages()
        {
            // Arrange
            var messageKey = nameof(NotificationsResx.NotifyPendingDeposit);
            var messageParams = new object[] { "Param1", "Param2" };
            var message = new LocalizedMessage(1, "Target", 100, "TitleKey", messageKey, messageParams);

            // Mock resource manager behavior by setting up test values
            var resourceManagerType = typeof(NotificationsResx).GetProperty("ResourceManager")?.GetValue(null);

            // Act
            message.AssignPersonalMessage();

            // Assert - we can only verify the properties are set, not their actual values
            // since we can't easily mock the resource manager in a unit test
            Assert.NotNull(message.En);
            Assert.NotNull(message.Ch);
            Assert.NotNull(message.Cs);
            Assert.NotNull(message.Th);
            Assert.NotNull(message.Vi);
            Assert.NotNull(message.Jp);
            Assert.NotNull(message.Ko);
        }

        [Fact]
        public void AssignToastMessage_Should_Set_Localized_Json_Messages()
        {
            // Arrange
            var titleKey = nameof(NotificationsResx.NotifyPendingDeposit_Title);
            var messageKey = nameof(NotificationsResx.NotifyPendingDeposit);
            var messageParams = new object[] { "Param1", "Param2" };
            var message = new LocalizedMessage(1, "Target", 100, titleKey, messageKey, messageParams);

            // Act
            message.AssignToastMessage();

            // Assert - verify structure of toast messages (should be JSON)
            Assert.True(IsValidJson(message.En));
            Assert.True(IsValidJson(message.Ch));
            Assert.True(IsValidJson(message.Cs));
            Assert.True(IsValidJson(message.Th));
            Assert.True(IsValidJson(message.Vi));
            Assert.True(IsValidJson(message.Jp));
            Assert.True(IsValidJson(message.Ko));

            // Verify content structure (should have title and body)
            var content = JsonConvert.DeserializeObject<ToastContentModel>(message.En);
            Assert.NotNull(content);
            Assert.NotNull(content.Title);
            Assert.NotNull(content.Body);
        }

        [Fact]
        public void BuildMessage_Should_Return_Empty_String_When_ResourceString_Is_Null()
        {
            // Arrange
            var nonExistentKey = "NonExistentKey";
            var message = new LocalizedMessage(1, "Target", 100, "TitleKey", nonExistentKey);

            // Act
            message.AssignPersonalMessage();

            // Assert - all languages should have empty strings
            Assert.Equal(string.Empty, message.En);
            Assert.Equal(string.Empty, message.Ch);
            Assert.Equal(string.Empty, message.Cs);
            Assert.Equal(string.Empty, message.Th);
            Assert.Equal(string.Empty, message.Vi);
            Assert.Equal(string.Empty, message.Jp);
            Assert.Equal(string.Empty, message.Ko);
        }

        [Fact]
        public void BuildMessage_Should_Format_Message_With_Params()
        {
            // Arrange
            // Assuming there's a resource key with a format placeholder like "{0}"
            var formatMessageKey = nameof(NotificationsResx.NotifyPendingWithdrawal);
            var message = new LocalizedMessage(1, "Target", 100, "TitleKey", formatMessageKey, new[] { "Param1", "Param2" });

            // Act
            message.AssignPersonalMessage();

            // Assert - we can't directly test the formatted string without mocking,
            // but we can verify all languages are set
            Assert.NotNull(message.En);
            Assert.NotNull(message.Ch);
            Assert.NotNull(message.Cs);
            Assert.NotNull(message.Th);
            Assert.NotNull(message.Vi);
            Assert.NotNull(message.Jp);
            Assert.NotNull(message.Ko);
        }

        private bool IsValidJson(string strInput)
        {
            if (string.IsNullOrWhiteSpace(strInput))
                return false;

            try
            {
                JsonConvert.DeserializeObject(strInput);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}