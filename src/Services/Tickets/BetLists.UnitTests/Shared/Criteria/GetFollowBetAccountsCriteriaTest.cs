using AlphaPlus.BetLists.Shared.Criteria;

namespace AlphaPlus.BetLists.UnitTests.Shared.Criteria;

[Trait("Category", "BetLists")]
public class GetFollowBetAccountsCriteriaTest
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        const int expectedAgentId = 123;

        // Act
        var criteria = new GetFollowBetAccountsCriteria(expectedAgentId);

        // Assert
        Assert.Equal(expectedAgentId, criteria.DummyAgentId);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new GetFollowBetAccountsCriteria(123);
        var expectedKey = "Age_Customer_FollowBetAccountListSelByAgent";

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, result);
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    [InlineData(1, true)]
    [InlineData(123, true)]
    public void IsValid_ShouldValidateAgentIdCorrectly(int agentId, bool expectedResult)
    {
        // Arrange
        var criteria = new GetFollowBetAccountsCriteria(agentId);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expectedResult, result);
    }
}