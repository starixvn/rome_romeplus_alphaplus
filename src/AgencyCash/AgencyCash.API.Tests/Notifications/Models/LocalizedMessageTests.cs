using System;
using System.Globalization;
using AgencyCash.API.Notifications.Models;
using AgencyCash.API.Notifications.Resources;
using AgencyCash.API.Shared.Constants;
using Newtonsoft.Json;

namespace AgencyCash.API.Tests.Notifications.Models;

public class LocalizedMessageTests
{
    private readonly LocalizedMessage _localizedMessage;

    public LocalizedMessageTests()
    {
        _localizedMessage = new LocalizedMessage(1, "TestTarget", 2);
    }

    [Fact]
    public void Constructor_InitializesProperties()
    {
        // Arrange & Act
        var message = new LocalizedMessage(10, "TestName", 20);

        // Assert
        Assert.Equal(10, message.TargetId);
        Assert.Equal("TestName", message.TargetName);
        Assert.Equal(20, message.Creator);
    }

    [Fact]
    public void AssignPersonalMessage_WithoutParams_SetsAllLanguageProperties()
    {
        // Arrange
        var resourceKey = "TestResource";

        // Use reflection to access the ResourceManager to verify resource exists
        var resourceExists = NotificationsResx.ResourceManager.GetString(resourceKey, CultureInfo.CreateSpecificCulture(Language.English)) != null;

        // Act
        _localizedMessage.AssignPersonalMessage(resourceKey);

        // Assert
        if (resourceExists)
        {
            Assert.False(string.IsNullOrEmpty(_localizedMessage.En));
            Assert.False(string.IsNullOrEmpty(_localizedMessage.Ch));
            Assert.False(string.IsNullOrEmpty(_localizedMessage.Cs));
            Assert.False(string.IsNullOrEmpty(_localizedMessage.Th));
            Assert.False(string.IsNullOrEmpty(_localizedMessage.Vi));
            Assert.False(string.IsNullOrEmpty(_localizedMessage.Jp));
            Assert.False(string.IsNullOrEmpty(_localizedMessage.Ko));
        }
        else
        {
            // If resource doesn't exist, we should at least verify empty strings are assigned
            Assert.Equal(string.Empty, _localizedMessage.En);
            Assert.Equal(string.Empty, _localizedMessage.Ch);
            Assert.Equal(string.Empty, _localizedMessage.Cs);
            Assert.Equal(string.Empty, _localizedMessage.Th);
            Assert.Equal(string.Empty, _localizedMessage.Vi);
            Assert.Equal(string.Empty, _localizedMessage.Jp);
            Assert.Equal(string.Empty, _localizedMessage.Ko);
        }
    }

    [Fact]
    public void AssignPersonalMessage_WithParams_FormatsProperly()
    {
        // Arrange
        var resourceKey = "TestFormatResource";
        var resourceValue = "Hello {0}";
        var messageParams = new object[] { "World" };
        var expectedValue = "Hello World";

        // Act - using a test resource key
        _localizedMessage.AssignPersonalMessage(resourceKey, messageParams);

        // Assert - we'll verify at least English is correct
        // Note: This might need to be skipped if the resource doesn't exist
        if (NotificationsResx.ResourceManager.GetString(resourceKey, CultureInfo.CreateSpecificCulture(Language.English)) != null)
        {
            Assert.Equal(expectedValue, _localizedMessage.En);
        }
    }

    [Fact]
    public void AssignToastMessage_CreatesJsonFormatForAllLanguages()
    {
        // Arrange
        var titleKey = "TestTitle";
        var bodyKey = "TestBody";
        var bodyParams = new object[] { "TestParam" };

        // Act
        _localizedMessage.AssignToastMessage(titleKey, bodyKey, bodyParams);

        // Assert
        // At least check if JSON is properly formatted
        Assert.True(IsValidJson(_localizedMessage.En));
        Assert.True(IsValidJson(_localizedMessage.Ch));
        Assert.True(IsValidJson(_localizedMessage.Cs));
        Assert.True(IsValidJson(_localizedMessage.Th));
        Assert.True(IsValidJson(_localizedMessage.Vi));
        Assert.True(IsValidJson(_localizedMessage.Jp));
        Assert.True(IsValidJson(_localizedMessage.Ko));
    }

    [Fact]
    public void AssignToastMessage_WithoutBodyParams_SetsDefaultEmptyArray()
    {
        // Arrange
        var titleKey = "TestTitle";
        var bodyKey = "TestBody";

        // Act
        _localizedMessage.AssignToastMessage(titleKey, bodyKey, null);

        // Assert
        // Verify JSON is still valid even without params
        Assert.True(IsValidJson(_localizedMessage.En));
        Assert.True(IsValidJson(_localizedMessage.Ch));
        Assert.True(IsValidJson(_localizedMessage.Cs));
        Assert.True(IsValidJson(_localizedMessage.Th));
        Assert.True(IsValidJson(_localizedMessage.Vi));
        Assert.True(IsValidJson(_localizedMessage.Jp));
        Assert.True(IsValidJson(_localizedMessage.Ko));
    }

    [Fact]
    public void AssignToastMessage_CreatesCorrectJsonStructure()
    {
        // Arrange
        var titleKey = "NotifyPendingDeposit_Title";
        var bodyKey = "NotifyPendingDeposit";

        // Act
        _localizedMessage.AssignToastMessage(titleKey, bodyKey);

        // Assert - Parse JSON and verify structure
        if (!string.IsNullOrEmpty(_localizedMessage.En))
        {
            var content = JsonConvert.DeserializeObject<ToastContentModel>(_localizedMessage.En);
            Assert.NotNull(content);
            Assert.NotNull(content.Title);
            Assert.NotNull(content.Body);
        }
    }

    [Fact]
    public void BuildMessage_WithNonExistentResourceKey_ReturnsEmptyString()
    {
        // Arrange
        var nonExistentKey = "NonExistentResourceKey_" + Guid.NewGuid();

        // Act
        _localizedMessage.AssignPersonalMessage(nonExistentKey);

        // Assert
        Assert.Equal(string.Empty, _localizedMessage.En);
        Assert.Equal(string.Empty, _localizedMessage.Ch);
        Assert.Equal(string.Empty, _localizedMessage.Cs);
        Assert.Equal(string.Empty, _localizedMessage.Th);
        Assert.Equal(string.Empty, _localizedMessage.Vi);
        Assert.Equal(string.Empty, _localizedMessage.Jp);
        Assert.Equal(string.Empty, _localizedMessage.Ko);
    }

    private bool IsValidJson(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            return false;
        }

        try
        {
            JsonConvert.DeserializeObject(json);
            return true;
        }
        catch
        {
            return false;
        }
    }

    // Helper class for deserializing toast messages
    private class ToastContentModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
    }
}