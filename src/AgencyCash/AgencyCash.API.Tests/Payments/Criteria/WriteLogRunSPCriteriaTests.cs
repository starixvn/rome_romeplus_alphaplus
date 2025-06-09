using AgencyCash.API.Payments.Criteria;

namespace AgencyCash.API.Tests.Payments.Criteria;

[Trait("Category", "Payments")]
public class WriteLogRunSPCriteriaTests
{
    [Fact]
    public void Constructor_Should_Initialize_Properties_With_Correct_Values()
    {
        // Arrange
        int userId = 123;
        int custId = 456;
        int actionId = 789;
        string trackingValueMask = "Mask1";
        string trackingValueMask2 = "Mask2";
        string trackingValue = "Value1";
        string trackingValue2 = "Value2";

        // Act
        var criteria = new WriteLogRunSPCriteria(
            userId,
            custId,
            actionId,
            trackingValueMask,
            trackingValueMask2,
            trackingValue,
            trackingValue2);

        // Assert
        Assert.Equal(userId, criteria.UserId);
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(actionId, criteria.ActionId);
        Assert.Equal(trackingValueMask, criteria.TrackingValueMask);
        Assert.Equal(trackingValueMask2, criteria.TrackingValueMask2);
        Assert.Equal(trackingValue, criteria.TrackingValue);
        Assert.Equal(trackingValue2, criteria.TrackingValue2);
    }

    [Fact]
    public void IsValid_Should_Return_True_When_CustId_Greater_Than_Zero()
    {
        // Arrange
        var criteria = new WriteLogRunSPCriteria(
            userId: 100,
            custId: 1,
            actionId: 1,
            trackingValueMask: "mask",
            trackingValueMask2: "mask2",
            trackingValue: "value",
            trackingValue2: "value2");

        // Act
        bool result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValid_Should_Return_False_When_CustId_Is_Zero()
    {
        // Arrange
        var criteria = new WriteLogRunSPCriteria(
            userId: 100,
            custId: 0,
            actionId: 1,
            trackingValueMask: "mask",
            trackingValueMask2: "mask2",
            trackingValue: "value",
            trackingValue2: "value2");

        // Act
        bool result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_Should_Return_False_When_CustId_Is_Negative()
    {
        // Arrange
        var criteria = new WriteLogRunSPCriteria(
            userId: 100,
            custId: -1,
            actionId: 1,
            trackingValueMask: "mask",
            trackingValueMask2: "mask2",
            trackingValue: "value",
            trackingValue2: "value2");

        // Act
        bool result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Default_Key_When_CustDBGroupId_Is_Null()
    {
        // Arrange
        var criteria = new WriteLogRunSPCriteria(
            userId: 100,
            custId: 1,
            actionId: 1,
            trackingValueMask: "mask",
            trackingValueMask2: "mask2",
            trackingValue: "value",
            trackingValue2: "value2");

        // Act
        string key = criteria.GetSettingKey();

        // Assert
        Assert.Equal("DBLOG_WriteLog_CashService_UserLog_RunSP", key);
    }

    [Fact]
    public void GetSettingKey_Should_Return_CustDB_Prefix_Key_When_CustDBGroupId_Has_Value()
    {
        // Arrange
        var criteria = new WriteLogRunSPCriteria(
            userId: 100,
            custId: 1,
            actionId: 1,
            trackingValueMask: "mask",
            trackingValueMask2: "mask2",
            trackingValue: "value",
            trackingValue2: "value2")
        {
            CustDBGroupId = 5
        };

        // Act
        string key = criteria.GetSettingKey();

        // Assert
        Assert.Equal("CustDB5_WriteLog_CashService_UserLog_RunSP", key);
    }
}