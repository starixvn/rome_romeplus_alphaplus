using AlphaPlus.CashService.API.Features.Shared.Criteria;
using AlphaPlus.CashService.API.Features.Shared.Requests;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Shared.Criteria;

public class WriteLogUserRunSPCriteriaTests
{
    [Fact]
    public void Constructor_WhenValidParameters_ShouldInitializeProperties()
    {
        // Arrange
        var actionId = 1;
        var request = new BaseActionRequest
        {
            ActorId = 100,
            UserId = 200,
            SiteDB = 1
        };
        var baseActionCommand = new BaseActionCommand(
            custId: request.ActorId,
            subAccId: 0,
            ip: "127.0.0.1",
            language: "en-US")
        {
            TrackingValue = "track1",
            TrackingValue2 = "track2"
        };

        // Act
        var criteria = new WriteLogUserRunSPCriteria(actionId, request, baseActionCommand);

        // Assert
        Assert.Equal(actionId, criteria.ActionId);
        Assert.Equal(request.ActorId, criteria.CustId);
        Assert.Equal(request.UserId, criteria.UserId);
        Assert.Equal(request.SiteDB, criteria.SiteDB);
        Assert.Equal(baseActionCommand.TrackingValue, criteria.TrackingValue);
        Assert.Equal(baseActionCommand.TrackingValue2, criteria.TrackingValue2);
    }

    [Fact]
    public void GetSettingKey_WhenCalled_ShouldReturnCorrectKey()
    {
        // Arrange
        var actionId = 1;
        var request = new BaseActionRequest { SiteDB = 2 };
        var baseActionCommand = new BaseActionCommand(
            custId: request.ActorId,
            subAccId: 0,
            ip: "127.0.0.1",
            language: "en-US");
        var criteria = new WriteLogUserRunSPCriteria(actionId, request, baseActionCommand);

        // Act
        var settingKey = criteria.GetSettingKey();

        // Assert
        Assert.Equal("SiteDB_2_WriteLog_UserLog_RunSP", settingKey);
    }

    [Theory]
    [InlineData(1, 1, true)]  // Valid case
    [InlineData(0, 1, false)] // Invalid CustId
    [InlineData(1, 0, false)] // Invalid ActionId
    [InlineData(0, 0, false)] // Both invalid
    public void IsValid_WhenCalled_ShouldReturnExpectedResult(int custId, int actionId, bool expectedResult)
    {
        // Arrange
        var request = new BaseActionRequest { ActorId = custId };
        var baseActionCommand = new BaseActionCommand(
            custId: request.ActorId,
            subAccId: 0,
            ip: "127.0.0.1",
            language: "en-US");
        var criteria = new WriteLogUserRunSPCriteria(actionId, request, baseActionCommand);

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.Equal(expectedResult, isValid);
    }
} 