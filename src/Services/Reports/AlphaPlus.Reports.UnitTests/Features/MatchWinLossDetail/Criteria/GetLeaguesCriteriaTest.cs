using AlphaPlus.Reports.Features.MatchWinLossDetail;

namespace AlphaPlus.Reports.UnitTests.Features.MatchWinLossDetail;

[Trait("Category", "MatchWinLossDetail")]
public class GetLeaguesCriteriaTest
{
    [Fact]
    public void GetSettingKey_ShouldReturnCorrectValue()
    {
        // Arrange
        var criteria = new GetLeaguesCriteria(
            custId: 123,
            productIds: "1,2,3",
            fromDate: DateTime.Now.AddDays(-7),
            toDate: DateTime.Now,
            moveBetsDay: DateTime.Now.AddDays(-1),
            language: "en-US",
            isShowAll: true);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_Rpt_MatchWinloss_GetLeagueByProduct", result);
    }

    [Theory]
    [InlineData(123, "1,2,3", true)]
    [InlineData(0, "1,2,3", false)]
    [InlineData(-1, "1,2,3", false)]
    [InlineData(123, "", false)]
    [InlineData(123, " ", false)]
    [InlineData(123, null, false)]
    public void IsValid_ShouldValidateCorrectly_ForCustIdAndProductIds(int custId, string productIds, bool expected)
    {
        // Arrange
        var fromDate = DateTime.Now.AddDays(-7);
        var toDate = DateTime.Now;
        var criteria = new GetLeaguesCriteria(
            custId: custId,
            productIds: productIds,
            fromDate: fromDate,
            toDate: toDate,
            moveBetsDay: DateTime.Now.AddDays(-1),
            language: "en-US",
            isShowAll: true);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_WhenFromDateIsAfterToDate()
    {
        // Arrange
        var fromDate = DateTime.Now.AddDays(1);
        var toDate = DateTime.Now;
        var criteria = new GetLeaguesCriteria(
            custId: 123,
            productIds: "1,2,3",
            fromDate: fromDate,
            toDate: toDate,
            moveBetsDay: DateTime.Now.AddDays(-1),
            language: "en-US",
            isShowAll: true);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_ShouldReturnTrue_WhenFromDateEqualsToDate()
    {
        // Arrange
        var date = DateTime.Now;
        var criteria = new GetLeaguesCriteria(
            custId: 123,
            productIds: "1,2,3",
            fromDate: date,
            toDate: date,
            moveBetsDay: DateTime.Now.AddDays(-1),
            language: "en-US",
            isShowAll: true);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        var custId = 123;
        var productIds = "1,2,3";
        var fromDate = DateTime.Now.AddDays(-7);
        var toDate = DateTime.Now;
        var moveBetsDay = DateTime.Now.AddDays(-1);
        var language = "en-US";
        var isShowAll = true;

        // Act
        var criteria = new GetLeaguesCriteria(
            custId: custId,
            productIds: productIds,
            fromDate: fromDate,
            toDate: toDate,
            moveBetsDay: moveBetsDay,
            language: language,
            isShowAll: isShowAll);

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(productIds, criteria.ProductIds);
        Assert.Equal(fromDate, criteria.FromDate);
        Assert.Equal(toDate, criteria.ToDate);
        Assert.Equal(moveBetsDay, criteria.MoveBetsDay);
        Assert.Equal(language, criteria.Language);
        Assert.Equal(isShowAll, criteria.IsShowAll);
    }
}