using AlphaPlus.Settings.Site.Features.MemberInfo.Tests.Constants;

namespace AlphaPlus.Settings.Site.UnitTests.Constants;

public class LanguageCodeTests
{
    [Fact]
    public void LanguageCode_Should_Have_Correct_English_Value()
    {
        // Act & Assert
        Assert.Equal("en-US", LanguageCode.English);
    }

    [Fact]
    public void LanguageCode_Should_Have_NonEmpty_English_Value()
    {
        // Act & Assert
        Assert.False(string.IsNullOrEmpty(LanguageCode.English));
    }

    [Fact]
    public void LanguageCode_Should_Have_Valid_Culture_Format()
    {
        // Arrange
        var englishCode = LanguageCode.English;

        // Act & Assert
        Assert.Contains("-", englishCode);
        Assert.Equal(5, englishCode.Length); // language-culture format
    }
}
