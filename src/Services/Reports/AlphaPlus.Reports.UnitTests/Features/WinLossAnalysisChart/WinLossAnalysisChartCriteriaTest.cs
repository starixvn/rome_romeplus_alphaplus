using AlphaPlus.Reports.Features.WinLossAnalysisChart;

namespace AlphaPlus.Reports.UnitTests.Features.WinLossAnalysisChart;

[Trait("Category", "WinLossAnalysisChart")]
public class WinLossAnalysisChartCriteriaTest
{
    [Theory]
    [InlineData(RoleIdConstants.Agent, "Age_Chart_WinlossAnalysis_Member")]
    [InlineData(RoleIdConstants.Master, "Age_Chart_WinlossAnalysis_Agent")]
    [InlineData(RoleIdConstants.Super, "Age_Chart_WinlossAnalysis_Master")]
    [InlineData(9, "Age_Chart_WinlossAnalysis_Master")]
    public void GetSettingKey_Should_Return_Correct_Setting_Key_Based_On_ReportLevelId(byte reportLevelId, string expectedSettingKey)
    {
        // Arrange
        var custId = 123;
        var fromDate = new DateTime(2023, 1, 1);
        var toDate = new DateTime(2023, 1, 31);
        var balanceUpDay = new DateTime(2023, 1, 31);
        var moveBetsDay = new DateTime(2023, 1, 31);
        var productIds = "1,2,3";

        var criteria = new WinLossAnalysisChartCriteria(
            custId,
            fromDate,
            toDate,
            balanceUpDay,
            moveBetsDay,
            productIds,
            reportLevelId);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedSettingKey, result);
    }

    [Fact]
    public void Constructor_Should_Initialize_Base_Properties_Correctly()
    {
        // Arrange
        var custId = 123;
        var fromDate = new DateTime(2023, 1, 1);
        var toDate = new DateTime(2023, 1, 31);
        var balanceUpDay = new DateTime(2023, 1, 31);
        var moveBetsDay = new DateTime(2023, 1, 31);
        var productIds = "1,2,3";
        var reportLevelId = RoleIdConstants.Master;

        // Act
        var criteria = new WinLossAnalysisChartCriteria(
            custId,
            fromDate,
            toDate,
            balanceUpDay,
            moveBetsDay,
            productIds,
            reportLevelId);

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(fromDate, criteria.FromDate);
        Assert.Equal(toDate, criteria.ToDate);
        Assert.Equal(balanceUpDay, criteria.BalanceUpDay);
        Assert.Equal(moveBetsDay, criteria.MoveBetsDay);
        Assert.Equal(productIds, criteria.ProductIds);
    }
}