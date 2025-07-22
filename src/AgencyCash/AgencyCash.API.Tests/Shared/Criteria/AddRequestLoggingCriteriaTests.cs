using AgencyCash.API.Shared.Criteria;

namespace AgencyCash.API.Tests.Shared.Criteria;

[Trait("Category", "Shared")]
public class AddRequestLoggingCriteriaTest
{
    [Fact]
    public void Constructor_Should_Initialize_Properties_Correctly()
    {
        // Arrange
        int userId = 123;
        int custId = 456;
        string funcName = "TestFunction";
        string requestUrl = "https://example.com/api";
        string requestBody = "{\"data\":\"test\"}";
        string from = "Test From";
        string to = "Test To";

        // Act
        var criteria = new AddRequestLoggingCriteria(
            userId,
            custId,
            funcName,
            requestUrl,
            requestBody,
            from,
            to);

        // Assert
        Assert.Equal(userId, criteria.UserId);
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(funcName, criteria.FuncName);
        Assert.Equal(requestUrl, criteria.RequestUrl);
        Assert.Equal(requestBody, criteria.RequestBody);
        Assert.Equal(from, criteria.From);
        Assert.Equal(to, criteria.To);
    }

    [Fact]
    public void Constructor_Should_Use_Default_Values_When_Not_Specified()
    {
        // Arrange
        int userId = 123;
        int custId = 456;
        string funcName = "TestFunction";
        string requestUrl = "https://example.com/api";
        string requestBody = "{\"data\":\"test\"}";

        // Act
        var criteria = new AddRequestLoggingCriteria(
            userId,
            custId,
            funcName,
            requestUrl,
            requestBody);

        // Assert
        Assert.Equal("Member Site", criteria.From);
        Assert.Equal("Agency Cash API", criteria.To);
    }

    [Fact]
    public void GetSettingKey_Should_Return_CustDBPrefix_When_CustDBGroupId_HasValue()
    {
        // Arrange
        var criteria = new AddRequestLoggingCriteria(1, 1, "test", "test", "test")
        {
            CustDBGroupId = 5
        };

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("CustDB5_WriteLog_CashService_UserLog_API", result);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Default_When_CustDBGroupId_Has_No_Value()
    {
        // Arrange
        var criteria = new AddRequestLoggingCriteria(1, 1, "test", "test", "test");

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("DBLOG_WriteLog_CashService_UserLog_API", result);
    }

    [Fact]
    public void IsValid_Should_Always_Return_True()
    {
        // Arrange
        var criteria = new AddRequestLoggingCriteria(1, 1, "test", "test", "test");

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void AdditionalProperties_Should_Be_Settable()
    {
        // Arrange
        var criteria = new AddRequestLoggingCriteria(1, 1, "test", "test", "test");

        // Act
        criteria.StatusCode = 200;
        criteria.Exception = "Test exception";
        criteria.Duration = 100;
        criteria.ResponseBody = "{\"result\":\"success\"}";

        // Assert
        Assert.Equal(200, criteria.StatusCode);
        Assert.Equal("Test exception", criteria.Exception);
        Assert.Equal(100, criteria.Duration);
        Assert.Equal("{\"result\":\"success\"}", criteria.ResponseBody);
    }
}