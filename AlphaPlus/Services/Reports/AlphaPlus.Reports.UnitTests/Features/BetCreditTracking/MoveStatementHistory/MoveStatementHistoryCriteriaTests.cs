using AlphaPlus.Reports.Features.BetCreditTracking.MoveStatementHistory;

namespace AlphaPlus.Reports.UnitTests.Features.BetCreditTracking.MoveStatementHistory;

[Trait("Category", "MoveStatementHistory")]
public class MoveStatementHistoryCriteriaTests
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var username = "testuser";
        var fromDate = new DateTime(2025, 1, 1);
        var toDate = new DateTime(2025, 1, 31);
        var loggedId = 1;
        var pageIndex = 0;
        var pageSize = 10;
        var isVariance = true;

        // Act
        var criteria = new MoveStatementHistoryCriteria(username, fromDate, toDate, loggedId, pageIndex, pageSize, isVariance);

        // Assert
        Assert.Equal(username, criteria.Username);
        Assert.Equal(fromDate, criteria.FromDate);
        Assert.Equal(toDate, criteria.ToDate);
        Assert.Equal(loggedId, criteria.LoggedId);
        Assert.Equal(pageIndex, criteria.PageIndex);
        Assert.Equal(pageSize, criteria.PageSize);
        Assert.Equal(isVariance, criteria.IsVariance);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new MoveStatementHistoryCriteria("testuser", DateTime.Now, DateTime.Now, 1, 0, 10, true);

        // Act
        var settingKey = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_Rpt_StatementMovement", settingKey);
    }

    [Fact]
    public void IsValid_ShouldReturnTrue_WhenCriteriaIsValid()
    {
        // Arrange
        var criteria = new MoveStatementHistoryCriteria("testuser", new DateTime(2025, 1, 1), new DateTime(2025, 1, 31), 1, 0, 10, true);

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_WhenFromDateIsGreaterThanToDate()
    {
        // Arrange
        var criteria = new MoveStatementHistoryCriteria("testuser", new DateTime(2025, 1, 31), new DateTime(2025, 1, 1), 1, 0, 10, true);

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.False(isValid);
    }
}
