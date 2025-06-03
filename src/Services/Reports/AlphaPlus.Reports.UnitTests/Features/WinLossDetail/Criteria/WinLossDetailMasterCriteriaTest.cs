
using AlphaPlus.Reports.Features.WinLossDetail;

namespace AlphaPlus.Reports.UnitTests.Features.WinLossDetail;

[Trait("Category", "WinLossDetail")]
public class WinLossDetailMasterCriteriaTest
{
    private readonly DateTime testFromDate = new(2023, 1, 1);
    private readonly DateTime testToDate = new(2023, 3, 31);
    private readonly DateTime testBalanceUpDay = new(2023, 3, 31);
    private readonly DateTime testMoveBetsDay = new(2023, 3, 31);

    [Fact]
    public void Constructor_Should_Initialize_Properties_Correctly()
    {
        // Arrange & Act
        var criteria = new WinLossDetailMasterCriteria(
            custId: 123,
            fromDate: testFromDate,
            toDate: testToDate,
            balanceUpDay: testBalanceUpDay,
            moveBetsDay: testMoveBetsDay,
            productIds: "1,2,3",
            productDetails: "Product1,Product2",
            username: "TestUser",
            isReward: true,
            levelType: 2);

        // Assert
        Assert.Equal(123, criteria.CustId);
        Assert.Equal(testFromDate, criteria.FromDate);
        Assert.Equal(testToDate, criteria.ToDate);
        Assert.Equal(testBalanceUpDay, criteria.BalanceUpDay);
        Assert.Equal(testMoveBetsDay, criteria.MoveBetsDay);
        Assert.Equal("1,2,3", criteria.ProductIds);
        Assert.Equal("Product1,Product2", criteria.ProductDetails);
        Assert.Equal("TestUser", criteria.Username);
        Assert.True(criteria.IsReward);
        Assert.Equal(2, criteria.LevelType);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Correct_Key()
    {
        // Arrange
        var criteria = new WinLossDetailMasterCriteria(
            custId: 123,
            fromDate: testFromDate,
            toDate: testToDate,
            balanceUpDay: testBalanceUpDay,
            moveBetsDay: testMoveBetsDay,
            productIds: "1,2,3",
            productDetails: "Product1,Product2",
            username: "TestUser",
            isReward: false,
            levelType: 1);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_Rpt_WinLossDetail_Master", result);
    }

    [Fact]
    public void WinLossDetailMasterCriteria_Should_Inherit_From_WinLossDetailAgentCriteria()
    {
        // Arrange & Act
        var criteria = new WinLossDetailMasterCriteria(
            custId: 123,
            fromDate: testFromDate,
            toDate: testToDate,
            balanceUpDay: testBalanceUpDay,
            moveBetsDay: testMoveBetsDay,
            productIds: "1,2,3",
            productDetails: "Product1,Product2",
            username: "TestUser",
            isReward: false,
            levelType: 1);

        // Assert
        Assert.IsAssignableFrom<WinLossDetailAgentCriteria>(criteria);
    }
}