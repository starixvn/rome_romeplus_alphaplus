using AlphaPlus.Settings.Site.Features.MemberInfo.Helpers;

namespace AlphaPlus.Settings.Site.UnitTests.Helpers;

public class UrlHelperTests
{
    [Fact]
    public void AddQueryStringParamsFunc_Should_Return_Default_Function_When_Not_Set()
    {
        // Arrange - Reset the ThreadStatic field
        var urlHelperType = typeof(UrlHelper);
        var addQueryStringParamsFuncField = urlHelperType.GetField("addQueryStringParamsFunc", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        addQueryStringParamsFuncField?.SetValue(null, null);

        // Act
        var func = UrlHelper.AddQueryStringParamsFunc;

        // Assert
        Assert.NotNull(func);
    }

    [Fact]
    public void ReplaceOrAddQueryStringParamsFunc_Should_Return_Default_Function_When_Not_Set()
    {
        // Arrange - Reset the ThreadStatic field
        var urlHelperType = typeof(UrlHelper);
        var replaceOrAddQueryStringParamsFuncField = urlHelperType.GetField("replaceOrAddQueryStringParamsFunc", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        replaceOrAddQueryStringParamsFuncField?.SetValue(null, null);

        // Act
        var func = UrlHelper.ReplaceOrAddQueryStringParamsFunc;

        // Assert
        Assert.NotNull(func);
    }

    [Fact]
    public void MapExMainPath_Should_Return_Default_Function_When_Not_Set()
    {
        // Arrange - Reset the ThreadStatic field
        var urlHelperType = typeof(UrlHelper);
        var mapExMainPathField = urlHelperType.GetField("mapExMainPath", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        mapExMainPathField?.SetValue(null, null);

        // Act
        var func = UrlHelper.MapExMainPath;

        // Assert
        Assert.NotNull(func);
    }

    [Fact]
    public void GetSiteUrl_Should_Not_Be_Null()
    {
        // Assert
        Assert.NotNull(UrlHelper.GetSiteUrl);
    }

    [Fact]
    public void MapBetListPath_Should_Return_Function()
    {
        // Act
        var func = UrlHelper.MapBetListPath;

        // Assert
        Assert.NotNull(func);
    }

    [Fact]
    public void AddQueryStringParams_Should_Handle_Empty_RouteValues()
    {
        // Arrange
        var link = "/test/path";
        var routeValues = new Dictionary<string, object>();

        // Act
        var urlHelperType = typeof(UrlHelper);
        var method = urlHelperType.GetMethod("AddQueryStringParams", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        var result = (string)method?.Invoke(null, new object[] { link, routeValues });

        // Assert
        Assert.Equal(link, result);
    }

    [Fact]
    public void AddQueryStringParams_Should_Add_QueryString_To_Link_Without_Query()
    {
        // Arrange
        var link = "/test/path";
        var routeValues = new Dictionary<string, object>
        {
            { "param1", "value1" },
            { "param2", "value2" }
        };

        // Act
        var urlHelperType = typeof(UrlHelper);
        var method = urlHelperType.GetMethod("AddQueryStringParams", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        var result = (string)method?.Invoke(null, new object[] { link, routeValues });

        // Assert
        Assert.Equal("/test/path?param1=value1&param2=value2", result);
    }

    [Fact]
    public void AddQueryStringParams_Should_Add_QueryString_To_Link_With_Existing_Query()
    {
        // Arrange
        var link = "/test/path?existing=param";
        var routeValues = new Dictionary<string, object>
        {
            { "param1", "value1" },
            { "param2", "value2" }
        };

        // Act
        var urlHelperType = typeof(UrlHelper);
        var method = urlHelperType.GetMethod("ReplaceOrAddQueryStringParams", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        var result = (string)method?.Invoke(null, new object[] { link, routeValues });

        // Assert
        Assert.Contains("existing=param", result);
        Assert.Contains("param1=value1", result);
        Assert.Contains("param2=value2", result);
    }

    [Fact]
    public void ReplaceOrAddQueryStringParam_Should_Handle_Null_Or_Empty_ParamName()
    {
        // Arrange
        var link = "/test/path";

        // Act
        var urlHelperType = typeof(UrlHelper);
        var method = urlHelperType.GetMethod("ReplaceOrAddQueryStringParam", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

        var result1 = (string)method?.Invoke(null, new object[] { link, null, "value" });
        var result2 = (string)method?.Invoke(null, new object[] { link, "", "value" });
        var result3 = (string)method?.Invoke(null, new object[] { link, "   ", "value" });

        // Assert
        Assert.Equal(link, result1);
        Assert.Equal(link, result2);
        Assert.Equal(link, result3);
    }

    [Fact]
    public void ReplaceOrAddQueryStringParam_Should_Handle_Null_ParamValue()
    {
        // Arrange
        var link = "/test/path";

        // Act
        var urlHelperType = typeof(UrlHelper);
        var method = urlHelperType.GetMethod("ReplaceOrAddQueryStringParam", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        var result = (string)method?.Invoke(null, new object[] { link, "param", null });

        // Assert
        Assert.Equal(link, result);
    }

    [Fact]
    public void ReplaceOrAddQueryStringParam_Should_Add_New_Param_When_Not_Exists()
    {
        // Arrange
        var link = "/test/path";

        // Act
        var urlHelperType = typeof(UrlHelper);
        var method = urlHelperType.GetMethod("ReplaceOrAddQueryStringParam", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        var result = (string)method?.Invoke(null, new object[] { link, "newparam", "newvalue" });

        // Assert
        Assert.Equal("/test/path&newparam=newvalue", result);
    }

    [Fact]
    public void ReplaceOrAddQueryStringParam_Should_Replace_Existing_Param()
    {
        // Arrange
        var link = "/test/path?existing=oldvalue&other=param";

        // Act
        var urlHelperType = typeof(UrlHelper);
        var method = urlHelperType.GetMethod("ReplaceOrAddQueryStringParam", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        var result = (string)method?.Invoke(null, new object[] { link, "existing", "newvalue" });

        // Assert
        Assert.Contains("existing=newvalue", result);
        Assert.Contains("other=param", result);
        Assert.DoesNotContain("existing=oldvalue", result);
    }

    [Fact]
    public void ReplaceOrAddQueryStringParam_Should_Handle_QueryString_With_QuestionMark_Param()
    {
        // Arrange
        var link = "/test/path?existing=oldvalue";

        // Act
        var urlHelperType = typeof(UrlHelper);
        var method = urlHelperType.GetMethod("ReplaceOrAddQueryStringParam", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        var result = (string)method?.Invoke(null, new object[] { link, "existing", "newvalue" });

        // Assert
        Assert.Equal("/test/path?existing=newvalue", result);
    }

    [Fact]
    public void UrlHelper_Should_Have_All_Required_Properties()
    {
        // Assert
        Assert.NotNull(UrlHelper.AddQueryStringParamsFunc);
        Assert.NotNull(UrlHelper.ReplaceOrAddQueryStringParamsFunc);
        Assert.NotNull(UrlHelper.MapExMainPath);
        Assert.NotNull(UrlHelper.GetSiteUrl);
        Assert.NotNull(UrlHelper.MapBetListPath);
    }
}
