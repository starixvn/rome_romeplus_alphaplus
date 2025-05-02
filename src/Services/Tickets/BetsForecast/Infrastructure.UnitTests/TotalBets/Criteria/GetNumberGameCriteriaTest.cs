using AlphaPlus.BetsForecast.Infrastructure.TotalBets.Criteria;

namespace AlphaPlus.BetsForecast.Infrastructure.UnitTests.TotalBets.Criteria;

[Trait("Category", "BetsForecast")]
public class GetNumberGameCriteriaTest
{
    [Fact]
    public void GetSettingKey_Should_Return_SuperLevel_Key_When_SuperLevel()
    {
        // Arrange
        const byte superLevel = 4; // Assuming 3 is super level
        const int custId = 123;
        var criteria = new GetNumberGameCriteria(superLevel, custId);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_TotalBets_Sel161_SAg", result);
    }

    [Fact]
    public void GetSettingKey_Should_Return_MasterLevel_Key_When_MasterLevel()
    {
        // Arrange
        const byte masterLevel = 3; // Assuming 2 is master level
        const int custId = 123;
        var criteria = new GetNumberGameCriteria(masterLevel, custId);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_TotalBets_Sel161_MAg", result);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Agent_Key_When_AgentLevel()
    {
        // Arrange
        const byte agentLevel = 2; // Assuming 1 is agent level
        const int custId = 123;
        var criteria = new GetNumberGameCriteria(agentLevel, custId);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_TotalBets_Sel161_Ag", result);
    }

    [Fact]
    public void Constructor_Should_Set_CustLevel_And_CustId()
    {
        // Arrange
        const byte custLevel = 2;
        const int custId = 456;

        // Act
        var criteria = new GetNumberGameCriteria(custLevel, custId);

        // Assert
        Assert.Equal(custLevel, criteria.CustLevel.Value);
        Assert.Equal(custId, criteria.CustId);
    }
}
