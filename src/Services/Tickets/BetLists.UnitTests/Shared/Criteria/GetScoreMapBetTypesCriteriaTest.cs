using AlphaPlus.BetLists.Shared.Criteria;

namespace AlphaPlus.BetLists.UnitTests.Shared.Criteria;

[Trait("Category", "BetLists")]
public class GetScoreMapBetTypesCriteriaTest
{
    [Fact]
    public void IsValid_ShouldAlwaysReturnTrue()
    {
        // Arrange
        var criteria = new GetScoreMapBetTypesCriteria();

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new GetScoreMapBetTypesCriteria();
        const string expectedKey = "Age_ScoreMap_SelBettype";

        // Act
        var key = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, key);
    }
}