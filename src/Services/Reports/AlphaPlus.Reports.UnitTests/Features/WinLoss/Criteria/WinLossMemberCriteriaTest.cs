using AlphaPlus.Reports.Features.WinLoss;

namespace AlphaPlus.Reports.UnitTests.Features.WinLoss;

[Trait("Category", "WinLoss")]
public class WinLossMemberCriteriaTest
{
    [Fact]
    public void Constructor_ShouldInitializeProperties_WithCorrectValues()
    {
        // Arrange
        var custId = 123;
        var fromDate = DateTime.Today.AddDays(-7);
        var toDate = DateTime.Today;
        var balanceUpDay = DateTime.Today;
        var moveBetsDay = DateTime.Today;
        var productIds = "1,2,3";
        var productDetails = "Sport,Casino";

        // Act
        var criteria = new WinLossMemberCriteria(
            custId,
            fromDate,
            toDate,
            balanceUpDay,
            moveBetsDay,
            productIds,
            productDetails);

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(fromDate, criteria.FromDate);
        Assert.Equal(toDate, criteria.ToDate);
        Assert.Equal(balanceUpDay, criteria.BalanceUpDay);
        Assert.Equal(moveBetsDay, criteria.MoveBetsDay);
        Assert.Equal(productIds, criteria.ProductIds);
        Assert.Equal(productDetails, criteria.ProductDetails);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new WinLossMemberCriteria(
            custId: 1,
            fromDate: DateTime.Today,
            toDate: DateTime.Today,
            balanceUpDay: DateTime.Today,
            moveBetsDay: DateTime.Today,
            productIds: "1",
            productDetails: "Sport");

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_Rpt_WinLoss_Member", result);
    }

    [Fact]
    public void WinLossMemberCriteria_ShouldInheritFromReportProductCriteria()
    {
        // Arrange
        var criteria = new WinLossMemberCriteria(
            custId: 1,
            fromDate: DateTime.Today,
            toDate: DateTime.Today,
            balanceUpDay: DateTime.Today,
            moveBetsDay: DateTime.Today,
            productIds: "1",
            productDetails: "Sport");

        // Assert
        Assert.IsAssignableFrom<AlphaPlus.Reports.Features.Shared.Criteria.ReportProductCriteria>(criteria);
    }
}