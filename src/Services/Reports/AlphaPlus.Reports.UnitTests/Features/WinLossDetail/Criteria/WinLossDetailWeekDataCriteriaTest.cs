using AlphaPlus.Reports.Features.WinLossDetail;

namespace AlphaPlus.Reports.UnitTests.Features.WinLossDetail;

public class WinLossDetailWeekDataCriteriaTest
{
    [Fact]
    public void Constructor_Should_Initialize_Properties_Correctly()
    {
        // Arrange
        int custId = 123;
        DateTime fromDate = new DateTime(2023, 1, 1);
        DateTime toDate = new DateTime(2023, 1, 31);
        DateTime balanceUpDay = new DateTime(2023, 1, 31);
        DateTime moveBetsDay = new DateTime(2023, 1, 31);
        string productIds = "1,2,3";
        string productDetails = "Product Details";
        string username = "TestUser";
        bool isReward = true;
        byte levelType = 3;

        // Act
        var criteria = new WinLossDetailWeekDataCriteria(
            custId,
            fromDate,
            toDate,
            balanceUpDay,
            moveBetsDay,
            productIds,
            productDetails,
            username,
            isReward,
            levelType);

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(fromDate, criteria.FromDate);
        Assert.Equal(toDate, criteria.ToDate);
        Assert.Equal(balanceUpDay, criteria.BalanceUpDay);
        Assert.Equal(moveBetsDay, criteria.MoveBetsDay);
        Assert.Equal(productIds, criteria.ProductIds);
        Assert.Equal(productDetails, criteria.ProductDetails);
        Assert.Equal(username, criteria.Username);
        Assert.Equal(isReward, criteria.IsReward);
        Assert.Equal(levelType, criteria.LevelType);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Correct_Key()
    {
        // Arrange
        var criteria = new WinLossDetailWeekDataCriteria(
            custId: 1,
            fromDate: DateTime.Now,
            toDate: DateTime.Now,
            balanceUpDay: DateTime.Now,
            moveBetsDay: DateTime.Now,
            productIds: string.Empty,
            productDetails: string.Empty,
            username: string.Empty,
            isReward: false,
            levelType: 1);

        // Act
        string settingKey = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_Rpt_WinlossDetail_WeekDataSel", settingKey);
    }

    [Fact]
    public void Should_Inherit_From_WinLossDetailBaseCriteria()
    {
        // Arrange & Act
        var criteria = new WinLossDetailWeekDataCriteria(
            custId: 1,
            fromDate: DateTime.Now,
            toDate: DateTime.Now,
            balanceUpDay: DateTime.Now,
            moveBetsDay: DateTime.Now,
            productIds: string.Empty,
            productDetails: string.Empty,
            username: string.Empty,
            isReward: false,
            levelType: 1);

        // Assert
        Assert.IsAssignableFrom<WinLossDetailBaseCriteria>(criteria);
    }
}