using AlphaPlus.Reports.Features.Consolidation;
using AlphaPlus.Reports.Features.Shared.Criteria;

namespace AlphaPlus.Reports.UnitTests.Features.Consolidation;

[Trait("Category", "Consolidation")]
public class GetConsolidationCriteriaTest
{
    private readonly DateTime fromDate = new(2023, 1, 1);
    private readonly DateTime toDate = new(2023, 3, 31);
    private readonly DateTime balanceUpDay = new(2023, 3, 31);
    private readonly DateTime moveBetsDay = new(2023, 3, 31);
    private readonly string productIds = "1,2,3";
    private readonly string productDetails = "Product1,Product2,Product3";
    private readonly int custId = 123;
    private readonly int currency = 20;
    private readonly byte reportType = 1;

    [Fact]
    public void Constructor_Should_Initialize_Properties_Correctly()
    {
        // Act
        var criteria = new GetConsolidationCriteria(
            custId,
            currency,
            fromDate,
            toDate,
            reportType,
            balanceUpDay,
            moveBetsDay,
            productIds,
            productDetails);

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(currency, criteria.Currency);
        Assert.Equal(fromDate, criteria.FromDate);
        Assert.Equal(toDate, criteria.ToDate);
        Assert.Equal(reportType, criteria.ReportType);
        Assert.Equal(balanceUpDay, criteria.BalanceUpDay);
        Assert.Equal(moveBetsDay, criteria.MoveBetsDay);
        Assert.Equal(productIds, criteria.ProductIds);
        Assert.Equal(productDetails, criteria.ProductDetails);
    }

    [Fact]
    public void GetSettingKey_Should_Return_CorrectKey()
    {
        // Arrange
        var criteria = new GetConsolidationCriteria(
            custId,
            currency,
            fromDate,
            toDate,
            reportType,
            balanceUpDay,
            moveBetsDay,
            productIds,
            productDetails);

        // Act
        var settingKey = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_Rpt_ConsolidatedGroup", settingKey);
    }

    [Fact]
    public void Inheritance_Should_Be_FromReportProductCriteria()
    {
        // Arrange
        var criteria = new GetConsolidationCriteria(
            custId,
            currency,
            fromDate,
            toDate,
            reportType,
            balanceUpDay,
            moveBetsDay,
            productIds,
            productDetails);

        // Assert
        Assert.IsAssignableFrom<ReportProductCriteria>(criteria);
    }
}