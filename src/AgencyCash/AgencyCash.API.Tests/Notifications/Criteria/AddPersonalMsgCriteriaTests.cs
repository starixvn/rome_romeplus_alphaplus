using AgencyCash.API.Notifications.Criteria;

namespace AgencyCash.API.Tests.Notifications.Criteria;

public class AddPersonalMsgCriteriaTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties_WithProvidedValues()
    {
        // Arrange
        var contentEN = "English content";
        var contentCH = "Chinese content";
        var contentCS = "Simplified Chinese content";
        var contentJP = "Japanese content";
        var contentTH = "Thai content";
        var contentKO = "Korean content";
        var contentVN = "Vietnamese content";

        // Act
        var criteria = new AddPersonalMsgCriteria(
            contentEN,
            contentCH,
            contentCS,
            contentJP,
            contentTH,
            contentKO,
            contentVN);

        // Assert
        Assert.Equal(contentEN, criteria.ContentEN);
        Assert.Equal(contentCH, criteria.ContentCH);
        Assert.Equal(contentCS, criteria.ContentCS);
        Assert.Equal(contentJP, criteria.ContentJP);
        Assert.Equal(contentTH, criteria.ContentTH);
        Assert.Equal(contentKO, criteria.ContentKO);
        Assert.Equal(contentVN, criteria.ContentVN);
        Assert.Equal(1, criteria.MessageType); // PersonalMsgType constant value
        Assert.Equal("Agency-Cash-API", criteria.CreatorName);
        Assert.True(criteria.IsBatch);
        Assert.Equal(0, criteria.MessageIdOutput);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new AddPersonalMsgCriteria(
            "content", "content", "content",
            "content", "content", "content", "content");

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_Msg_SetMessage", result);
    }

    [Fact]
    public void IsValid_ShouldReturnTrue()
    {
        // Arrange
        var criteria = new AddPersonalMsgCriteria(
            "content", "content", "content",
            "content", "content", "content", "content");

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Constructor_ShouldAcceptEmptyStrings()
    {
        // Arrange & Act
        var criteria = new AddPersonalMsgCriteria(
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty);

        // Assert
        Assert.Equal(string.Empty, criteria.ContentEN);
        Assert.Equal(string.Empty, criteria.ContentCH);
        Assert.Equal(string.Empty, criteria.ContentCS);
        Assert.Equal(string.Empty, criteria.ContentJP);
        Assert.Equal(string.Empty, criteria.ContentTH);
        Assert.Equal(string.Empty, criteria.ContentKO);
        Assert.Equal(string.Empty, criteria.ContentVN);
    }

    [Fact]
    public void Constructor_ShouldAcceptNullStrings()
    {
        // Arrange & Act
        var criteria = new AddPersonalMsgCriteria(
            null,
            null,
            null,
            null,
            null,
            null,
            null);

        // Assert
        Assert.Null(criteria.ContentEN);
        Assert.Null(criteria.ContentCH);
        Assert.Null(criteria.ContentCS);
        Assert.Null(criteria.ContentJP);
        Assert.Null(criteria.ContentTH);
        Assert.Null(criteria.ContentKO);
        Assert.Null(criteria.ContentVN);
    }
}
