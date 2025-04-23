using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Statement;

namespace AlphaPlus.Reports.UnitTests.Features.Statement;

[Trait("Category", "Statement")]
public class GetStatementBetTrans14CriteriaTest
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        int custId = 123;
        var fromDate = new DateTime(2023, 1, 5);
        var toDate = new DateTime(2023, 1, 10);
        var balanceUpDay = new DateTime(2023, 1, 1);

        // Act
        var criteria = new GetStatementBetTrans14Criteria(custId, fromDate, toDate, balanceUpDay);

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(fromDate, criteria.FromDate);
        Assert.Equal(toDate, criteria.ToDate);
        Assert.Equal(balanceUpDay, criteria.BalanceUpDay);
        Assert.Equal(ReportDateService.BetTrans14SPSuffix, criteria.StoreMappingSuffix);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new GetStatementBetTrans14Criteria(123, DateTime.Now, DateTime.Now, DateTime.Now);
        var expectedKey = "Statement_Report_GetList_14";

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, result);
    }
}