using AlphaPlus.BetsForecast.Infrastructure.TotalBets.Criteria;

namespace AlphaPlus.BetsForecast.Infrastructure.UnitTests.TotalBets.Criteria;

[Trait("Category", "BetsForecast")]
public class GetOutrightCriteriaTest
{
    [Fact]
    public void GetSettingKey_When_SuperLevel_Returns_SuperAgentKey()
    {
        // Arrange
        byte superLevel = 4; // Assuming 0 is super level
        int custId = 123;
        var criteria = new GetOutrightCriteria(superLevel, custId);

        // Act
        string result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_TotalBets_Sel10_SAg", result);
    }

    [Fact]
    public void GetSettingKey_When_MasterLevel_Returns_MasterAgentKey()
    {
        // Arrange
        byte masterLevel = 3; // Assuming 1 is master level
        int custId = 123;
        var criteria = new GetOutrightCriteria(masterLevel, custId);

        // Act
        string result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_TotalBets_Sel10_MAg", result);
    }

    [Fact]
    public void GetSettingKey_When_AgentLevel_Returns_AgentKey()
    {
        // Arrange
        byte agentLevel = 2; // Assuming 2 is agent level
        int custId = 123;
        var criteria = new GetOutrightCriteria(agentLevel, custId);

        // Act
        string result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_TotalBets_Sel10_Ag", result);
    }

    [Fact]
    public void Constructor_Sets_CustLevel_And_CustId()
    {
        // Arrange
        byte custLevel = 2;
        int custId = 456;

        // Act
        var criteria = new GetOutrightCriteria(custLevel, custId);

        // Assert
        Assert.Equal(custLevel, criteria.CustLevel.Value);
        Assert.Equal(custId, criteria.CustId);
    }
}
