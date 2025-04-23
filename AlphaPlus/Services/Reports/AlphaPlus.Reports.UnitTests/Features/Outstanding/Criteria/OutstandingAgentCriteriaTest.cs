using AlphaPlus.Reports.Features.Outstanding;

namespace AlphaPlus.Reports.UnitTests.Features.Outstanding;

[Trait("Category", "Outstanding")]
public class OutstandingAgentCriteriaTest
{
    [Fact]
    public void Constructor_ShouldInitializeProperties_WithCorrectValues()
    {
        // Arrange
        int custId = 123;
        string productIds = "1,2,3";
        byte levelType = 2;

        // Act
        var criteria = new OutstandingAgentCriteria(custId, productIds, levelType);

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(productIds, criteria.ProductIds);
        Assert.Equal(levelType, criteria.LevelType);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new OutstandingAgentCriteria(123, "1,2,3", 2);
        string expectedKey = "Age_Rpt_Outstanding_Agent";

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, result);
    }

    [Fact]
    public void InheritedProperties_ShouldBeInitializedCorrectly()
    {
        // Arrange
        int custId = 456;
        string productIds = "4,5,6";
        byte levelType = 1;

        // Act
        var criteria = new OutstandingAgentCriteria(custId, productIds, levelType);

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(productIds, criteria.ProductIds);
    }
}