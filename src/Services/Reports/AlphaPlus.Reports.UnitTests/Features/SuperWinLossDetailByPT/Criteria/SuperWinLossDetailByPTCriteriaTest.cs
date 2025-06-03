using AlphaPlus.Reports.Features.SuperWinLossDetailByPT;

namespace AlphaPlus.Reports.UnitTests.Features.SuperWinLossDetailByPT;

[Trait("Category", "SuperWinLossDetailByPT")]
public class SuperWinLossDetailByPTCriteriaTest
{
    private readonly DateTime testFromDate = new(2023, 1, 1);
    private readonly DateTime testToDate = new(2023, 3, 31);
    private readonly DateTime testBalanceUpDay = new(2023, 3, 31);
    private readonly DateTime testMoveBetsDay = new(2023, 3, 31);
    private readonly string testProductIds = "1,2,3";
    private readonly string testProductDetails = "ProductA,ProductB";
    private readonly int testCustId = 123;

    [Fact]
    public void Constructor_Should_Initialize_Properties_Correctly()
    {
        // Act
        var criteria = new SuperWinLossDetailByPTCriteria(
            testCustId,
            testFromDate,
            testToDate,
            testBalanceUpDay,
            testMoveBetsDay,
            testProductIds,
            testProductDetails);

        // Assert
        Assert.Equal(testCustId, criteria.CustId);
        Assert.Equal(testFromDate, criteria.FromDate);
        Assert.Equal(testToDate, criteria.ToDate);
        Assert.Equal(testBalanceUpDay, criteria.BalanceUpDay);
        Assert.Equal(testMoveBetsDay, criteria.MoveBetsDay);
        Assert.Equal(testProductIds, criteria.ProductIds);
        Assert.Equal(testProductDetails, criteria.ProductDetails);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Correct_Value()
    {
        // Arrange
        var criteria = new SuperWinLossDetailByPTCriteria(
            testCustId,
            testFromDate,
            testToDate,
            testBalanceUpDay,
            testMoveBetsDay,
            testProductIds,
            testProductDetails);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_Rpt_WinLossDetailByPT_Super", result);
    }
}