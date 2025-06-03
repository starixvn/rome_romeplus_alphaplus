using AlphaPlus.Reports.Features.BetChoiceMatching;

namespace AlphaPlus.Reports.UnitTests.Features.BetChoiceMatching;

[Trait("Category", "BetChoiceMatching")]
public class BetChoiceUplinesCriteriaTest
{
    [Fact]
    public void GetSettingKey_ShouldReturnExpectedKey()
    {
        // Arrange
        var custId = 1;
        var criteria = new BetChoiceUplinesCriteria(custId);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_Customer_Upline_Sel", result);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    public void IsValid_ShouldReturnExpectedResult(int custId, bool expected)
    {
        // Arrange
        var criteria = new BetChoiceUplinesCriteria(custId);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expected, result);
    }
}