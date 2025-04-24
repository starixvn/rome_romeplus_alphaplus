using AlphaPlus.Reports.Features.WinLossAnalysis;

namespace AlphaPlus.Reports.UnitTests.Features.WinLossAnalysis;

[Trait("Category", "WinLossAnalysis")]
public class WinLossAnalysisCriteriaTest
{
    [Fact]
    public void Constructor_ShouldInitializeProperties_WithValidParameters()
    {
        // Arrange
        int custId = 123;
        int reportType = 1;
        DateTime fromDate = new DateTime(2023, 1, 1);
        DateTime toDate = new DateTime(2023, 1, 31);
        DateTime balanceUpDay = new DateTime(2023, 1, 15);
        DateTime moveBetsDay = new DateTime(2023, 1, 20);
        string productIds = "1,2,3";
        byte reportLevelId = 1;

        // Act
        var criteria = new WinLossAnalysisCriteria(
            custId,
            reportType,
            fromDate,
            toDate,
            balanceUpDay,
            moveBetsDay,
            productIds,
            reportLevelId);

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(reportType, criteria.ReportType);
        Assert.Equal(fromDate, criteria.FromDate);
        Assert.Equal(toDate, criteria.ToDate);
        Assert.Equal(balanceUpDay, criteria.BalanceUpDay);
        Assert.Equal(moveBetsDay, criteria.MoveBetsDay);
        Assert.Equal(productIds, criteria.ProductIds);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnAgentKey_WhenMasterLevelIdProvided()
    {
        // Arrange
        var criteria = new WinLossAnalysisCriteria(
            custId: 123,
            reportType: 1,
            fromDate: DateTime.Now.AddDays(-7),
            toDate: DateTime.Now,
            balanceUpDay: DateTime.Now.AddDays(-3),
            moveBetsDay: DateTime.Now.AddDays(-2),
            productIds: "1,2,3",
            reportLevelId: RoleIdConstants.Master);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_Rpt_WinlossAnalysis_Agent", result);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnMasterKey_WhenSuperLevelIdProvided()
    {
        // Arrange
        var criteria = new WinLossAnalysisCriteria(
            custId: 123,
            reportType: 2,
            fromDate: DateTime.Now.AddDays(-7),
            toDate: DateTime.Now,
            balanceUpDay: DateTime.Now.AddDays(-3),
            moveBetsDay: DateTime.Now.AddDays(-2),
            productIds: "1,2,3",
            reportLevelId: RoleIdConstants.Super);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_Rpt_WinlossAnalysis_Master", result);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnMemberKey_WhenAgentLevelIdProvided()
    {
        // Arrange
        var criteria = new WinLossAnalysisCriteria(
            custId: 123,
            reportType: 3,
            fromDate: DateTime.Now.AddDays(-7),
            toDate: DateTime.Now,
            balanceUpDay: DateTime.Now.AddDays(-3),
            moveBetsDay: DateTime.Now.AddDays(-2),
            productIds: "1,2,3",
            reportLevelId: RoleIdConstants.Agent);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_Rpt_WinlossAnalysis_Member", result);
    }
}