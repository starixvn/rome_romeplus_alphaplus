using AlphaPlus.Reports.Features.Products;

namespace AlphaPlus.Reports.UnitTests.Features.Products;

[Trait("Category", "Products")]
public class GetProductsDetailCriteriaTest
{
    private readonly GetProductsDetailCriteria criteria;

    public GetProductsDetailCriteriaTest()
    {
        criteria = new GetProductsDetailCriteria();
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_Rpt_GetProductDetail", result);
    }

    [Fact]
    public void IsValid_ShouldReturnTrue()
    {
        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }
}