using AlphaPlus.BetsForecast.Infrastructure.TotalBets.Criteria;

namespace AlphaPlus.BetsForecast.Infrastructure.UnitTests.TotalBets.Criteria;

[Trait("Category", "BetsForecast")]
public class GetOE1X2DNDCriteriaTest
{
    [Fact]
    public void GetSettingKey_WhenSuperLevel_ShouldReturnSuperLevelKey()
    {
        // Arrange
        const byte superLevel = 4; // Assuming 3 is super level
        const int custId = 123;
        var criteria = new GetOE1X2DNDCriteria(superLevel, custId);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_TotalBets_Sel25_SAg", result);
    }

    [Fact]
    public void GetSettingKey_WhenMasterLevel_ShouldReturnMasterLevelKey()
    {
        // Arrange
        const byte masterLevel = 3; // Assuming 2 is master level
        const int custId = 123;
        var criteria = new GetOE1X2DNDCriteria(masterLevel, custId);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_TotalBets_Sel25_MAg", result);
    }

    [Fact]
    public void GetSettingKey_WhenNeitherSuperNorMasterLevel_ShouldReturnDefaultKey()
    {
        // Arrange
        const byte agentLevel = 2; // Assuming 1 is agent level
        const int custId = 123;
        var criteria = new GetOE1X2DNDCriteria(agentLevel, custId);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_TotalBets_Sel25_Ag", result);
    }

    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        const byte custLevel = 2;
        const int custId = 456;

        // Act
        var criteria = new GetOE1X2DNDCriteria(custLevel, custId);

        // Assert
        Assert.Equal(custLevel, criteria.CustLevel.Value);
        Assert.Equal(custId, criteria.CustId);
    }
}
