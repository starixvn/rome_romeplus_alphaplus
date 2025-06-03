
using AlphaPlus.Reports.Features.Outstanding;

namespace AlphaPlus.Reports.UnitTests.Features.Outstanding;

[Trait("Category", "Outstanding")]
public class GetFollowBetAccountsCriteriaTest
{
    [Fact]
    public void Constructor_ShouldSetProperties_WhenCalled()
    {
        // Arrange & Act
        var agentId = 123;
        var criteria = new GetFollowBetAccountsCriteria(agentId);

        // Assert
        Assert.Equal(agentId, criteria.DummyAgentId);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey_WhenCalled()
    {
        // Arrange
        var criteria = new GetFollowBetAccountsCriteria(1);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_Customer_FollowBetAccountListSelByAgent", result);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(100, true)]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    public void IsValid_ShouldReturnExpectedResult_BasedOnAgentId(int agentId, bool expectedResult)
    {
        // Arrange
        var criteria = new GetFollowBetAccountsCriteria(agentId);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expectedResult, result);
    }
}