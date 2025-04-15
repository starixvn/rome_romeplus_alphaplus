using AlphaPlus.Reports.Features.BetCreditTracking.BetCreditHistory;

namespace AlphaPlus.Reports.UnitTests.Features.BetCreditTracking.BetCreditHistory;

[Trait("Category", "BetCreditHistory")]
public class BetCreditHistoryCriteriaTests
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var username = "testuser";
        var fromDate = new DateTime(2025, 1, 1);
        var toDate = new DateTime(2025, 1, 31);
        long? refNo = 123456789;
        var loggedId = 1;
        var pageIndex = 0;
        var pageSize = 10;

        // Act
        var criteria = new BetCreditHistoryCriteria(username, fromDate, toDate, refNo, loggedId, pageIndex, pageSize);

        // Assert
        Assert.Equal(username, criteria.Username);
        Assert.Equal(fromDate, criteria.FromDate);
        Assert.Equal(toDate, criteria.ToDate);
        Assert.Equal(refNo, criteria.RefNo);
        Assert.Equal(loggedId, criteria.LoggedId);
        Assert.Equal(pageIndex, criteria.PageIndex);
        Assert.Equal(pageSize, criteria.PageSize);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new BetCreditHistoryCriteria("testuser", DateTime.Now, DateTime.Now, null, 1, 0, 10);

        // Act
        var settingKey = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_Rpt_BetCreditHistory", settingKey);
    }

    [Fact]
    public void IsValid_ShouldReturnTrue_WhenCriteriaIsValid()
    {
        // Arrange
        var criteria = new BetCreditHistoryCriteria("testuser", new DateTime(2025, 1, 1), new DateTime(2025, 1, 31), null, 1, 0, 10);

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_WhenUsernameIsEmpty()
    {
        // Arrange
        var criteria = new BetCreditHistoryCriteria("", new DateTime(2025, 1, 1), new DateTime(2025, 1, 31), null, 1, 0, 10);

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_WhenFromDateIsGreaterThanToDate()
    {
        // Arrange
        var criteria = new BetCreditHistoryCriteria("testuser", new DateTime(2025, 1, 31), new DateTime(2025, 1, 1), null, 1, 0, 10);

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.False(isValid);
    }
}
