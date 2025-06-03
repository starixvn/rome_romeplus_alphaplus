using AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

namespace AlphaPlus.BetsForecast.Infrastructure.UnitTests.Forecast.Criteria;

[Trait("Category", "BetsForecast")]
public class GetRunningLeagueMasterCriteriaTest
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        int masterId = 123;
        string betTypes = "1,2,3";

        // Act
        var criteria = new GetRunningLeagueMasterCriteria(masterId, betTypes);

        // Assert
        Assert.Equal(masterId, criteria.MAgId);
        Assert.Equal(betTypes, criteria.BetTypes);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new GetRunningLeagueMasterCriteria(1, "1,2,3");

        // Act
        var settingKey = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_LeagueSelRunning_MAg", settingKey);
    }

    [Theory]
    [InlineData(1, "1,2,3", true)]
    [InlineData(0, "1,2,3", false)]
    [InlineData(-1, "1,2,3", false)]
    [InlineData(1, "", false)]
    [InlineData(1, null, false)]
    [InlineData(1, "   ", false)]
    public void IsValid_ShouldReturnExpectedResult(int masterId, string betTypes, bool expectedResult)
    {
        // Arrange
        var criteria = new GetRunningLeagueMasterCriteria(masterId, betTypes);

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.Equal(expectedResult, isValid);
    }
}