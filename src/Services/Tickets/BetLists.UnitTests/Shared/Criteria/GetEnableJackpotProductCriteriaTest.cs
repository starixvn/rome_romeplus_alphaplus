using AlphaPlus.BetLists.Shared.Criteria;

namespace AlphaPlus.BetLists.UnitTests.Shared.Criteria;

[Trait("Category", "BetLists")]
public class GetEnableJackpotProductCriteriaTest
{
    [Fact]
    public void Constructor_ShouldInitializeIsTestProperty()
    {
        // Arrange & Act
        var criteriaWithTestData = new GetEnableJackpotProductCriteria(true);
        var criteriaWithoutTestData = new GetEnableJackpotProductCriteria(false);

        // Assert
        Assert.True(criteriaWithTestData.IsTest);
        Assert.False(criteriaWithoutTestData.IsTest);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new GetEnableJackpotProductCriteria(true);
        var expectedKey = "Acc_GameLaunchSel";

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, result);
    }

    [Fact]
    public void IsValid_ShouldAlwaysReturnTrue()
    {
        // Arrange
        var criteriaWithTestData = new GetEnableJackpotProductCriteria(true);
        var criteriaWithoutTestData = new GetEnableJackpotProductCriteria(false);

        // Act
        var resultWithTestData = criteriaWithTestData.IsValid();
        var resultWithoutTestData = criteriaWithoutTestData.IsValid();

        // Assert
        Assert.True(resultWithTestData);
        Assert.True(resultWithoutTestData);
    }
}