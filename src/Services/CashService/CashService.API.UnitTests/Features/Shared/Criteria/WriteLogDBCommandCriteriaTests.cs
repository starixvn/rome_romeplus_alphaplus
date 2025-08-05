using AlphaPlus.CashService.API.Features.Shared.Criteria;
using AlphaPlus.CashService.API.Features.Shared.Models;
using AlphaPlus.CashService.API.Features.Shared.Requests;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Shared.Criteria;

public class WriteLogDBCommandCriteriaTests
{
    [Fact]
    public void Constructor_WhenValidParametersProvided_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var request = new BaseActionRequest
        {
            ActorId = 123,
            UserId = 456,
            CustDBGroupId = 789
        };

        var model = new WriteLogDBCommandModel
        {
            ActionId = 1,
            TrackingValue = "track1",
            TrackingValue2 = "track2",
            TrackingValueMask = "mask1",
            TrackingValueMask2 = "mask2"
        };

        // Act
        var criteria = new WriteLogDBCommandCriteria(request.CustDBGroupId, request, model);

        // Assert
        Assert.Equal(123, criteria.CustId);
        Assert.Equal(456, criteria.UserId);
        Assert.Equal(789, criteria.CustDbGroupId);
        Assert.Equal(1, criteria.ActionId);
        Assert.Equal("track1", criteria.TrackingValue);
        Assert.Equal("track2", criteria.TrackingValue2);
        Assert.Equal("mask1", criteria.TrackingValueMask);
        Assert.Equal("mask2", criteria.TrackingValueMask2);
    }

    [Fact]
    public void GetSettingKey_WhenSiteDBIsNegative_ShouldReturnDefaultKey()
    {
        // Arrange
        var request = new BaseActionRequest { CustDBGroupId = -1 };
        var model = new WriteLogDBCommandModel { ActionId = 1 };
        var criteria = new WriteLogDBCommandCriteria(request.CustDBGroupId, request, model);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("WriteLog_CashService_UserLog_RunSP", result);
    }

    [Fact]
    public void GetSettingKey_WhenSiteDBIsPositive_ShouldReturnSiteSpecificKey()
    {
        // Arrange
        var request = new BaseActionRequest { CustDBGroupId = 123 };
        var model = new WriteLogDBCommandModel { ActionId = 1 };
        var criteria = new WriteLogDBCommandCriteria(request.CustDBGroupId, request, model);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("CustDb_123_WriteLog_CashService_UserLog_RunSP", result);
    }

    [Theory]
    [InlineData(0, 1, false)]  // Invalid CustId
    [InlineData(1, 0, true)]  // Valid
    public void IsValid_WhenGivenDifferentInputs_ShouldReturnExpectedResult(int custId, int actionId, bool expected)
    {
        // Arrange
        var request = new BaseActionRequest { ActorId = custId };
        var model = new WriteLogDBCommandModel { ActionId = actionId };
        var criteria = new WriteLogDBCommandCriteria(request.CustDBGroupId, request, model);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expected, result);
    }
}