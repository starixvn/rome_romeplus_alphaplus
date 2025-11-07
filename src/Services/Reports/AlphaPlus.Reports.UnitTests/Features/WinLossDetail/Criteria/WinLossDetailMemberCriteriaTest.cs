using AlphaPlus.Reports.Features.WinLossDetail;

namespace AlphaPlus.Reports.UnitTests.Features.WinLossDetail;

[Trait("Category", "WinLossDetail")]
public class WinLossDetailMemberCriteriaTest
{
    private readonly WinLossDetailMemberCriteria criteria;
    private readonly DateTime fromDate = new(2023, 1, 1);
    private readonly DateTime toDate = new(2023, 3, 31);
    private readonly DateTime balanceUpDay = new(2023, 3, 31);
    private readonly DateTime moveBetsDay = new(2023, 3, 31);

    public WinLossDetailMemberCriteriaTest()
    {
        criteria = new WinLossDetailMemberCriteria(
            custId: 123,
            fromDate: fromDate,
            toDate: toDate,
            balanceUpDay: balanceUpDay,
            moveBetsDay: moveBetsDay,
            productIds: "1,2,3",
            productDetails: "Product1,Product2",
            username: "testuser",
            isReward: false,
            "db052");
    }

    [Fact]
    public void Constructor_Should_Initialize_Properties_Correctly()
    {
        // Assert
        Assert.Equal(123, criteria.CustId);
        Assert.Equal(fromDate, criteria.FromDate);
        Assert.Equal(toDate, criteria.ToDate);
        Assert.Equal(balanceUpDay, criteria.BalanceUpDay);
        Assert.Equal(moveBetsDay, criteria.MoveBetsDay);
        Assert.Equal("1,2,3", criteria.ProductIds);
        Assert.Equal("Product1,Product2", criteria.ProductDetails);
        Assert.Equal("testuser", criteria.Username);
        Assert.False(criteria.IsReward);
        Assert.Equal(1, criteria.PageIndex);
        Assert.Equal(10000, criteria.PageSize);
        Assert.Equal(0, criteria.TotalRecords);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Correct_Key()
    {
        // Act
        var key = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_Rpt_WinLossDetail_Member_DB052", key);
    }

    [Fact]
    public void TotalRecords_Should_Be_Settable()
    {
        // Act
        criteria.TotalRecords = 100;

        // Assert
        Assert.Equal(100, criteria.TotalRecords);
    }

    [Fact]
    public void PageIndex_Should_Have_Default_Value_1()
    {
        // Assert
        Assert.Equal(1, criteria.PageIndex);
    }

    [Fact]
    public void PageSize_Should_Have_Default_Value_10000()
    {
        // Assert
        Assert.Equal(10000, criteria.PageSize);
    }
}