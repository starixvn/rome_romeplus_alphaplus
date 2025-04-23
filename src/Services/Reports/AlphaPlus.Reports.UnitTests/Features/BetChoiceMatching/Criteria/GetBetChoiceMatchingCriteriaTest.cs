using AlphaPlus.Reports.Features.BetChoiceMatching;

namespace AlphaPlus.Reports.UnitTests.Features.BetChoiceMatching;
[Trait("Category", "BetChoiceMatching")]
public class GetBetChoiceMatchingCriteriaTest
{
    [Fact]
    public void GetSettingKey_ShouldReturnExpectedKey()
    {
        // Arrange
        var custId = 1;
        var winlostDate = DateTime.Now;
        var moveBetsDay = DateTime.Now;
        var betCount = 10;
        var isExportExcel = true;
        var criteria = new GetBetChoiceMatchingCriteria(custId, winlostDate, moveBetsDay, betCount, isExportExcel);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_BetStatistics_BetMatching_Master", result);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    public void IsValid_ShouldReturnExpectedResult(int custId, bool expected)
    {
        // Arrange
        var winlostDate = DateTime.Now;
        var moveBetsDay = DateTime.Now;
        var betCount = 10;
        var isExportExcel = true;
        var criteria = new GetBetChoiceMatchingCriteria(custId, winlostDate, moveBetsDay, betCount, isExportExcel);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expected, result);
    }
}
