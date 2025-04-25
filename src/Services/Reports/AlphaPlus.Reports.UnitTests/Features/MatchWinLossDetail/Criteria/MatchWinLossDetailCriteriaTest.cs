using AlphaPlus.Reports.Features.MatchWinLossDetail;

namespace AlphaPlus.Reports.UnitTests.Features.MatchWinLossDetail;

[Trait("Category", "MatchWinLossDetail")]
public class MatchWinLossDetailCriteriaTest
{
    [Theory]
    [InlineData(1, "1,2,3", true)]
    [InlineData(0, "1,2,3", false)]
    [InlineData(-1, "1,2,3", false)]
    [InlineData(1, "", false)]
    [InlineData(1, null, false)]
    public void IsValid_Should_Return_Expected_Result(int custId, string productIds, bool expected)
    {
        // Arrange
        var fromDate = DateTime.Now.AddDays(-7);
        var toDate = DateTime.Now;
        var criteria = new MatchWinLossDetailCriteria(
            custId,
            fromDate,
            toDate,
            DateTime.Now,
            productIds,
            string.Empty,
            string.Empty,
            1);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void IsValid_Should_Return_False_When_FromDate_Greater_Than_ToDate()
    {
        // Arrange
        var fromDate = DateTime.Now;
        var toDate = DateTime.Now.AddDays(-7);
        var criteria = new MatchWinLossDetailCriteria(
            1,
            fromDate,
            toDate,
            DateTime.Now,
            "1,2,3",
            string.Empty,
            string.Empty,
            1);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(null, null, true)]
    [InlineData("", "", true)]
    [InlineData(" ", "", true)]
    [InlineData("1,2,3", "", false)]
    [InlineData("", "1,2,3", false)]
    [InlineData("1,2,3", "1,2,3", false)]
    public void IsGetAll_Should_Return_Expected_Result(string leagueIds, string league3rdIds, bool expected)
    {
        // Arrange
        var criteria = new MatchWinLossDetailCriteria(
            1,
            DateTime.Now.AddDays(-7),
            DateTime.Now,
            DateTime.Now,
            "1,2,3",
            leagueIds,
            league3rdIds,
            1);

        // Act
        var result = criteria.IsGetAll;

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(RoleIdConstants.Agent, "Age_Rpt_MatchWinLossDetail_Member")]
    [InlineData(RoleIdConstants.Master, "Age_Rpt_MatchWinLossDetail_Agent")]
    [InlineData(9, "Age_Rpt_MatchWinlossDetail_Master")] // Assuming 9 is SuperAdmin or some non-Agent, non-Master role
    public void GetSettingKey_Should_Return_Correct_Key_Based_On_ReportLevelId(byte reportLevelId, string expected)
    {
        // Arrange
        var criteria = new MatchWinLossDetailCriteria(
            1,
            DateTime.Now.AddDays(-7),
            DateTime.Now,
            DateTime.Now,
            "1,2,3",
            string.Empty,
            string.Empty,
            reportLevelId);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Constructor_Should_Initialize_Properties_Correctly()
    {
        // Arrange
        int custId = 123;
        var fromDate = DateTime.Now.AddDays(-7);
        var toDate = DateTime.Now;
        var moveBetsDay = DateTime.Now.AddDays(-1);
        string productIds = "1,2,3";
        string leagueIds = "4,5,6";
        string league3rdIds = "7,8,9";
        byte reportLevelId = 1;

        // Act
        var criteria = new MatchWinLossDetailCriteria(
            custId,
            fromDate,
            toDate,
            moveBetsDay,
            productIds,
            leagueIds,
            league3rdIds,
            reportLevelId);

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(fromDate, criteria.FromDate);
        Assert.Equal(toDate, criteria.ToDate);
        Assert.Equal(moveBetsDay, criteria.MoveBetsDay);
        Assert.Equal(productIds, criteria.ProductIds);
        Assert.Equal(leagueIds, criteria.LeagueIds);
        Assert.Equal(league3rdIds, criteria.League3rdIds);
    }
}