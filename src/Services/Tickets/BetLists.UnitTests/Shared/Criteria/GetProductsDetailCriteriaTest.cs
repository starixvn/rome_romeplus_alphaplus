using AlphaPlus.BetLists.Shared.Criteria;

namespace AlphaPlus.BetLists.UnitTests.Shared.Criteria;

[Trait("Category", "BetLists")]
public class GetProductsDetailCriteriaTest
{
    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new GetProductsDetailCriteria();
        const string expectedKey = "Age_Rpt_GetProductDetail";

        // Act
        var key = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, key);
    }

    [Fact]
    public void IsValid_ShouldAlwaysReturnTrue()
    {
        // Arrange
        var criteria = new GetProductsDetailCriteria();

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.True(isValid);
    }
}