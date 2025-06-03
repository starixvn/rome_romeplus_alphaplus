using AlphaPlus.Reports.Features.Outstanding;

namespace AlphaPlus.Reports.UnitTests.Features.Outstanding;

[Trait("Category", "Outstanding")]
public class OutstandingMasterCriteriaTest
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        int custId = 123;
        string productIds = "1,2,3";
        byte levelType = 2;

        // Act
        var criteria = new OutstandingMasterCriteria(custId, productIds, levelType);

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(productIds, criteria.ProductIds);
        Assert.Equal(levelType, criteria.LevelType);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new OutstandingMasterCriteria(123, "1,2,3", 2);
        string expectedKey = "Age_Rpt_Outstanding_Master";

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, result);
    }

    [Theory]
    [InlineData(1, "1", 1)]
    [InlineData(999, "1,2,3,4,5", 3)]
    [InlineData(0, "", 0)]
    public void Constructor_ShouldSetPropertiesWithDifferentValues(int custId, string productIds, byte levelType)
    {
        // Act
        var criteria = new OutstandingMasterCriteria(custId, productIds, levelType);

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(productIds, criteria.ProductIds);
        Assert.Equal(levelType, criteria.LevelType);
    }

    [Fact]
    public void GetSettingKey_ShouldBeConstant_RegardlessOfInputParameters()
    {
        // Arrange
        var criteria1 = new OutstandingMasterCriteria(123, "1,2,3", 2);
        var criteria2 = new OutstandingMasterCriteria(456, "4,5,6", 3);
        string expectedKey = "Age_Rpt_Outstanding_Master";

        // Act
        var result1 = criteria1.GetSettingKey();
        var result2 = criteria2.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, result1);
        Assert.Equal(expectedKey, result2);
        Assert.Equal(result1, result2);
    }
}