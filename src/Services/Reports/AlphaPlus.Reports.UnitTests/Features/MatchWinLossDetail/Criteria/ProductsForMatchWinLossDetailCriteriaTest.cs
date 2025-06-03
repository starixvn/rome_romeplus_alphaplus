using AlphaPlus.Reports.Features.MatchWinLossDetail;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.UnitTests.Features.MatchWinLossDetail;

[Trait("Category", "MatchWinLossDetail")]
public class ProductsForMatchWinLossDetailCriteriaTest
{
    private readonly ProductsForMatchWinLossDetailCriteria criteria;

    public ProductsForMatchWinLossDetailCriteriaTest()
    {
        criteria = new ProductsForMatchWinLossDetailCriteria();
    }

    [Fact]
    public void GetSettingKey_Should_Return_Correct_Key()
    {
        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_Rpt_MatchWinLossDetail_GetProductList", result);
    }

    [Fact]
    public void IsValid_Should_Return_True()
    {
        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Constructor_Should_Initialize_CriteriaBase()
    {
        // Assert
        Assert.IsAssignableFrom<CriteriaBase>(criteria);
    }
}