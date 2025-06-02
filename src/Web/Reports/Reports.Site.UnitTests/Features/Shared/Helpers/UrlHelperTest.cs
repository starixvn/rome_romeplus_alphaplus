using AlphaPlus.Reports.Site.Features.Shared.Helpers;

namespace AlphaPlus.Reports.Site.UnitTests.Features.Shared.Helpers;

[Trait("Category", "Shared")]
public class UrlHelperTest
{
    [Fact]
    public void AddQueryStringParams_Should_Return_Original_Link_When_Params_Empty()
    {
        // Arrange
        string link = "https://example.com";
        var queryParams = Enumerable.Empty<KeyValuePair<string, string>>();

        // Act
        var result = UrlHelper.AddQueryStringParams(link, queryParams);

        // Assert
        Assert.Equal(link, result);
    }

    [Fact]
    public void AddQueryStringParams_Should_Add_Query_Params_To_Link()
    {
        // Arrange
        string link = "https://example.com";
        var queryParams = new List<KeyValuePair<string, string>>
        {
            new("param1", "value1"),
            new("param2", "value2")
        };

        // Act
        var result = UrlHelper.AddQueryStringParams(link, queryParams);

        // Assert
        Assert.Equal("https://example.com?param1=value1&param2=value2", result);
    }

    [Fact]
    public void ReplaceOrAddQueryStringParams_Should_Return_Original_Link_When_Params_Empty()
    {
        // Arrange
        string link = "https://example.com";
        var queryParams = Enumerable.Empty<KeyValuePair<string, string>>();

        // Act
        var result = UrlHelper.ReplaceOrAddQueryStringParams(link, queryParams);

        // Assert
        Assert.Equal(link, result);
    }

    [Fact]
    public void ReplaceOrAddQueryStringParams_Should_Add_Query_Params_When_No_Query_String_Exists()
    {
        // Arrange
        string link = "https://example.com";
        var queryParams = new List<KeyValuePair<string, string>>
        {
            new("param1", "value1"),
            new("param2", "value2")
        };

        // Act
        var result = UrlHelper.ReplaceOrAddQueryStringParams(link, queryParams);

        // Assert
        Assert.Equal("https://example.com?param1=value1&param2=value2", result);
    }

    [Fact]
    public void ReplaceOrAddQueryStringParams_Should_Replace_Existing_Params()
    {
        // Arrange
        string link = "https://example.com?param1=oldvalue&param3=value3";
        var queryParams = new List<KeyValuePair<string, string>>
        {
            new("param1", "newvalue"),
            new("param2", "value2")
        };

        // Act
        var result = UrlHelper.ReplaceOrAddQueryStringParams(link, queryParams);

        // Assert
        Assert.Contains("param1=newvalue", result);
        Assert.Contains("param2=value2", result);
        Assert.Contains("param3=value3", result);
    }

    [Fact]
    public void ReplaceOrAddQueryStringParams_Should_Skip_Empty_Keys_Or_Values()
    {
        // Arrange
        string link = "https://example.com?existing=value";
        var queryParams = new List<KeyValuePair<string, string>>
        {
            new("", "value1"),
            new("param2", ""),
            new("param3", "value3")
        };

        // Act
        var result = UrlHelper.ReplaceOrAddQueryStringParams(link, queryParams);

        // Assert
        Assert.Equal("https://example.com?existing=value&param3=value3", result);
    }

    [Fact]
    public void ReplaceOrAddQueryStringParam_Should_Replace_Existing_Param()
    {
        // Arrange
        string link = "https://example.com?param1=oldvalue&param2=value2";

        // Act
        var result = UrlHelper.ReplaceOrAddQueryStringParam(link, "param1", "newvalue");

        // Assert
        Assert.Equal("https://example.com?param1=newvalue&param2=value2", result);
    }

    [Fact]
    public void ReplaceOrAddQueryStringParam_Should_Be_Case_Insensitive()
    {
        // Arrange
        string link = "https://example.com?PARAM1=oldvalue&param2=value2";

        // Act
        var result = UrlHelper.ReplaceOrAddQueryStringParam(link, "param1", "newvalue");

        // Assert
        Assert.Equal("https://example.com?PARAM1=newvalue&param2=value2", result);
    }

    [Fact]
    public void ReplaceOrAddQueryStringParam_Should_Add_New_Param_If_Not_Exists()
    {
        // Arrange
        string link = "https://example.com?param1=value1";

        // Act
        var result = UrlHelper.ReplaceOrAddQueryStringParam(link, "param2", "value2");

        // Assert
        Assert.Equal("https://example.com?param1=value1&param2=value2", result);
    }
}
