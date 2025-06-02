using AlphaPlus.Reports.Features.WinLoss;

namespace AlphaPlus.Reports.UnitTests.Features.WinLoss;

[Trait("Category", "WinLoss")]
public class WinLossAgentCriteriaTest
{
    [Fact]
    public void Constructor_ShouldInitializeProperties_WithCorrectValues()
    {
        // Arrange
        const int custId = 123;
        var fromDate = new DateTime(2023, 1, 1);
        var toDate = new DateTime(2023, 1, 10);
        var balanceUpDay = new DateTime(2023, 1, 5);
        var moveBetsDay = new DateTime(2023, 1, 7);
        const string productIds = "1,2,3";
        const string productDetails = "Details";
        const byte levelType = 2;

        // Act
        var criteria = new WinLossAgentCriteria(
            custId,
            fromDate,
            toDate,
            balanceUpDay,
            moveBetsDay,
            productIds,
            productDetails,
            levelType);

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(fromDate, criteria.FromDate);
        Assert.Equal(toDate, criteria.ToDate);
        Assert.Equal(balanceUpDay, criteria.BalanceUpDay);
        Assert.Equal(moveBetsDay, criteria.MoveBetsDay);
        Assert.Equal(productIds, criteria.ProductIds);
        Assert.Equal(productDetails, criteria.ProductDetails);
        Assert.Equal(levelType, criteria.LevelType);
    }

    [Fact]
    public void GetSettingKey_ShouldReturn_CorrectValue()
    {
        // Arrange
        var criteria = new WinLossAgentCriteria(
            custId: 123,
            fromDate: new DateTime(2023, 1, 1),
            toDate: new DateTime(2023, 1, 10),
            balanceUpDay: new DateTime(2023, 1, 5),
            moveBetsDay: new DateTime(2023, 1, 7),
            productIds: "1,2,3",
            productDetails: "Details",
            levelType: 2);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_Rpt_WinLoss_Agent", result);
    }

    [Fact]
    public void Inheritance_ShouldInheritFrom_WinLossMemberCriteria()
    {
        // Arrange
        const int custId = 123;
        var fromDate = new DateTime(2023, 1, 1);
        var toDate = new DateTime(2023, 1, 10);
        var balanceUpDay = new DateTime(2023, 1, 5);
        var moveBetsDay = new DateTime(2023, 1, 7);
        const string productIds = "1,2,3";
        const string productDetails = "Details";
        const byte levelType = 2;

        // Act
        var criteria = new WinLossAgentCriteria(
            custId,
            fromDate,
            toDate,
            balanceUpDay,
            moveBetsDay,
            productIds,
            productDetails,
            levelType);

        // Assert
        Assert.IsAssignableFrom<WinLossMemberCriteria>(criteria);
    }
}