using AlphaPlus.Reports.Site.Features.Shared.Helpers;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using Newtonsoft.Json;
using Breadcrumb = AlphaPlus.Reports.Features.Shared.Models.Breadcrumb;

namespace AlphaPlus.Reports.Site.UnitTests.Features.Shared.Helpers;

[Trait("Category", "Shared")]
public class BreadcrumbHelperTest
{
    [Fact]
    public void BuildBreadcrumbs_With_Empty_BreadcrumbsJson_Should_Return_Only_Current_User()
    {
        // Arrange
        var userSession = GlobalMockUserSession.CreateMockUserSessionDefault();
        var message = new BaseQuery(userSession.Object);

        // Act
        var result = BreadcrumbHelper.BuildBreadcrumbs(message).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal(message.CustId, result[0].CustId);
        Assert.Equal(message.CustName, result[0].CustName);
        Assert.Equal(message.CustLevelId, result[0].CustLevelId);
    }

    [Fact]
    public void BuildBreadcrumbs_With_Invalid_Json_Should_Return_Only_Current_User()
    {
        // Arrange
        var userSession = GlobalMockUserSession.CreateMockUserSessionDefault();
        var message = new BaseQuery(userSession.Object)
        {
            BreadcrumbsJson = "invalid json"
        };

        // Act
        var result = BreadcrumbHelper.BuildBreadcrumbs(message).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal(message.CustId, result[0].CustId);
        Assert.Equal(message.CustName, result[0].CustName);
        Assert.Equal(message.CustLevelId, result[0].CustLevelId);
    }

    [Fact]
    public void BuildBreadcrumbs_With_Valid_Breadcrumbs_Should_Return_Expected_Result()
    {
        // Arrange
        var breadcrumbs = new[]
        {
            new Breadcrumb(4, "Super", 4),
            new Breadcrumb(3, "Master", 3),
            new Breadcrumb(2, "Agent", 2)
        };

        var userSession = GlobalMockUserSession.CreateMockUserSessionDefault();
        var message = new BaseQuery(userSession.Object)
        {
            CustId = 1,
            CustName = "Member",
            CustLevelId = 1,
            BreadcrumbsJson = JsonConvert.SerializeObject(breadcrumbs)
        };

        // Act
        var result = BreadcrumbHelper.BuildBreadcrumbs(message).ToList();

        // Assert
        Assert.Equal(4, result.Count);
        Assert.Equal(4, result[0].CustId);
        Assert.Equal("Super", result[0].CustName);
        Assert.Equal(4, result[0].CustLevelId);
        Assert.Equal(3, result[1].CustId);
        Assert.Equal("Master", result[1].CustName);
        Assert.Equal(3, result[1].CustLevelId);
        Assert.Equal(2, result[2].CustId);
        Assert.Equal("Agent", result[2].CustName);
        Assert.Equal(2, result[2].CustLevelId);
        Assert.Equal(1, result[3].CustId);
        Assert.Equal("Member", result[3].CustName);
        Assert.Equal(1, result[3].CustLevelId);
    }

    [Fact]
    public void BuildBreadcrumbs_Should_Stop_At_Same_CustLevelId()
    {
        // Arrange
        var breadcrumbs = new[]
        {
            new Breadcrumb(4, "Super", 4),
            new Breadcrumb(3, "Master", 3),
            new Breadcrumb(2, "Agent", 2)
        };

        var userSession = GlobalMockUserSession.CreateMockUserSessionDefault();
        var message = new BaseQuery(userSession.Object)
        {
            CustId = 1,
            CustName = "AnotherAgent",
            CustLevelId = 2, // Same as Agent above
            BreadcrumbsJson = JsonConvert.SerializeObject(breadcrumbs)
        };

        // Act
        var result = BreadcrumbHelper.BuildBreadcrumbs(message).ToList();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal(4, result[0].CustId);
        Assert.Equal("Super", result[0].CustName);
        Assert.Equal(4, result[0].CustLevelId);
        Assert.Equal(3, result[1].CustId);
        Assert.Equal("Master", result[1].CustName);
        Assert.Equal(3, result[1].CustLevelId);
        Assert.Equal(1, result[2].CustId);
        Assert.Equal("AnotherAgent", result[2].CustName);
        Assert.Equal(2, result[2].CustLevelId);
    }

    [Fact]
    public void BuildBreadcrumbs_With_Empty_BreadcrumbList_Should_Return_Only_Current_User()
    {
        // Arrange
        var userSession = GlobalMockUserSession.CreateMockUserSessionDefault();
        var message = new BaseQuery(userSession.Object)
        {
            BreadcrumbsJson = "[]"
        };

        // Act
        var result = BreadcrumbHelper.BuildBreadcrumbs(message).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal(message.CustId, result[0].CustId);
        Assert.Equal(message.CustName, result[0].CustName);
        Assert.Equal(message.CustLevelId, result[0].CustLevelId);
    }
}