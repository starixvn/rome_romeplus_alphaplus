namespace AlphaPlus.Reports.UnitTests.Features.WinLossDetail;

using Reports.Features.WinLossDetail.Criteria;

[Trait("Category", "WinLossDetail")]
public class WinLossDetailAgentCriteriaTest
{
    [Fact]
    public void Constructor_Should_Initialize_Properties_Correctly()
    {
        // Arrange
        int custId = 123;
        DateTime fromDate = new DateTime(2023, 1, 1);
        DateTime toDate = new DateTime(2023, 3, 31);
        DateTime balanceUpDay = new DateTime(2023, 3, 31);
        DateTime moveBetsDay = new DateTime(2023, 3, 31);
        string productIds = "1,2,3";
        string productDetails = "ProductA,ProductB";
        string username = "testUser";
        bool isReward = true;
        byte levelType = 2;

        // Act
        var criteria = new WinLossDetailAgentCriteria(
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
    public void GetSettingKey_Should_Return_Correct_Value()
    {
        // Arrange
        var criteria = new WinLossDetailAgentCriteria(
            custId: 123,
            fromDate: new DateTime(2023, 1, 1),
            toDate: new DateTime(2023, 3, 31),
            balanceUpDay: new DateTime(2023, 3, 31),
            moveBetsDay: new DateTime(2023, 3, 31),
            productIds: "1,2,3",
            productDetails: "ProductA,ProductB",
            username: "testUser",
            isReward: true,
            levelType: 2,
            string.Empty);

        // Act
        string settingKey = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_Rpt_WinLossDetail_Agent", settingKey);
    }
}