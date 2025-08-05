using AlphaPlus.CashService.API.Features.Promotion.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Promotion.Criteria;

[Trait("Category", "CashService")]
public class GetProductListCriteriaTests
{
    [Fact]
    public void Constructor_Should_Set_Properties_Correctly()
    {
        // Arrange
        var site = "test-site";
        var currency = 1;
        var settingStatus = 2;

        // Act
        var criteria = new GetProductListCriteria(site, currency, settingStatus);

        // Assert
        Assert.Equal(site, criteria.Site);
        Assert.Equal(currency, criteria.Currency);
        Assert.Equal(settingStatus, criteria.SettingStatus);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Correct_Key()
    {
        // Arrange
        var criteria = new GetProductListCriteria("test", 1, 2);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_CashService_PromotionProductList", result);
    }

    [Fact]
    public void IsValid_Should_Return_True()
    {
        // Arrange
        var criteria = new GetProductListCriteria("test", 1, 2);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData("site1")]
    [InlineData("site2")]
    [InlineData("")]
    [InlineData(null)]
    public void IsValid_Should_Return_True_For_Any_Site(string site)
    {
        // Arrange
        var criteria = new GetProductListCriteria(site, 1, 2);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-1)]
    public void IsValid_Should_Return_True_For_Any_Currency(int currency)
    {
        // Arrange
        var criteria = new GetProductListCriteria("test", currency, 2);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-1)]
    public void IsValid_Should_Return_True_For_Any_SettingStatus(int settingStatus)
    {
        // Arrange
        var criteria = new GetProductListCriteria("test", 1, settingStatus);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Properties_Can_Be_Modified_After_Construction()
    {
        // Arrange
        var criteria = new GetProductListCriteria("original", 1, 2);

        // Act
        criteria.Site = "modified";
        criteria.Currency = 10;
        criteria.SettingStatus = 20;

        // Assert
        Assert.Equal("modified", criteria.Site);
        Assert.Equal(10, criteria.Currency);
        Assert.Equal(20, criteria.SettingStatus);
    }

    [Fact]
    public void Properties_Should_Accept_Null_Site()
    {
        // Arrange & Act
        var criteria = new GetProductListCriteria(null, 1, 2);

        // Assert
        Assert.Null(criteria.Site);
    }

    [Fact]
    public void Properties_Should_Accept_Empty_Site()
    {
        // Arrange & Act
        var criteria = new GetProductListCriteria("", 1, 2);

        // Assert
        Assert.Equal("", criteria.Site);
    }
}