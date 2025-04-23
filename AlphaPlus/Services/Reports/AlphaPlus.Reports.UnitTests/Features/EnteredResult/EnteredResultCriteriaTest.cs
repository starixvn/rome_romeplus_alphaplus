using AlphaPlus.Reports.Features.EnteredResult;

namespace AlphaPlus.Reports.UnitTests.Features.EnteredResult;

[Trait("Category", "EnteredResult")]
public class EnteredResultCriteriaTest
{
    [Fact]
    public void Constructor_ShouldSetIncludeTestDataProperty()
    {
        // Arrange & Act
        var criteriaWithTestData = new EnteredResultCriteria(true);
        var criteriaWithoutTestData = new EnteredResultCriteria(false);

        // Assert
        Assert.True(criteriaWithTestData.IncludeTestData);
        Assert.False(criteriaWithoutTestData.IncludeTestData);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new EnteredResultCriteria(true);
        var expectedKey = "Age_Rpt_MatchSelResultEntered";

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, result);
    }

    [Fact]
    public void IsValid_ShouldAlwaysReturnTrue()
    {
        // Arrange
        var criteria = new EnteredResultCriteria(true);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }
}