namespace AlphaPlus.Reports.UnitTests.Features.WinLossDetail.Criteria;

using AlphaPlus.Reports.Features.WinLossDetail;

public class WinLossDetailWeekDataCriteriaTest
{
    [Fact]
    public void Constructor_Should_Initialize_Properties_Correctly()
    {
        // Arrange
        var custId = 123;
        var fromDate = new DateTime(2023, 1, 1);
        var toDate = new DateTime(2023, 1, 31);
        var balanceUpDay = new DateTime(2023, 1, 31);
        var moveBetsDay = new DateTime(2023, 1, 31);
        var productIds = "1,2,3";
        var productDetails = "Product Details";
        var username = "TestUser";
        var isReward = true;
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
            levelType,
            string.Empty);

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
            levelType: 1,
            string.Empty);

        // Act
        var settingKey = criteria.GetSettingKey();

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
            levelType: 1,
            string.Empty);

        // Assert
        Assert.IsAssignableFrom<WinLossDetailBaseCriteria>(criteria);
    }
}