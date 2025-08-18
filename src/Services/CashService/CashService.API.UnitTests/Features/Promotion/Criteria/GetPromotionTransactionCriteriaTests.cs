using AlphaPlus.CashService.API.Features.Promotion.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Promotion.Criteria;

[Trait("Category", "CashService")]
public class GetPromotionTransactionCriteriaTests
{
    [Fact]
    public void Constructor_Should_Set_Properties_Correctly()
    {
        // Arrange
        var custId = 1;
        var fromDate = new DateTime(2023, 1, 1);
        var toDate = new DateTime(2023, 1, 31);
        var status = 2;
        int? promotionType = 3;
        var pageSize = 10;
        var pageIndex = 1;
        var siteDbGroupId = 5;

        // Act
        var criteria = new GetPromotionTransactionCriteria
        {
            CustId = custId,
            FromDate = fromDate,
            ToDate = toDate,
            Status = status,
            PromotionType = promotionType,
            PageSize = pageSize,
            PageIndex = pageIndex,
            SiteDbGroupId = siteDbGroupId
        };

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(fromDate, criteria.FromDate);
        Assert.Equal(toDate, criteria.ToDate);
        Assert.Equal(status, criteria.Status);
        Assert.Equal(promotionType, criteria.PromotionType);
        Assert.Equal(pageSize, criteria.PageSize);
        Assert.Equal(pageIndex, criteria.PageIndex);
        Assert.Equal(siteDbGroupId, criteria.SiteDbGroupId);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Correct_Key()
    {
        // Arrange
        var criteria = new GetPromotionTransactionCriteria
        {
            SiteDbGroupId = 10
        };

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("SiteDb_10_Age_CashPromotion_Transaction_Sel", result);
    }

    [Fact]
    public void IsValid_Should_Return_True()
    {
        // Arrange
        var criteria = new GetPromotionTransactionCriteria();

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-1)]
    public void IsValid_Should_Return_True_For_Any_CustId(int custId)
    {
        // Arrange
        var criteria = new GetPromotionTransactionCriteria
        {
            CustId = custId
        };

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void TotalRecords_Should_Default_To_Zero()
    {
        // Arrange & Act
        var criteria = new GetPromotionTransactionCriteria();

        // Assert
        Assert.Equal(0, criteria.TotalRecords);
    }

    [Fact]
    public void PromotionType_Should_Accept_Null_Value()
    {
        // Arrange & Act
        var criteria = new GetPromotionTransactionCriteria
        {
            PromotionType = null
        };

        // Assert
        Assert.Null(criteria.PromotionType);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(null)]
    public void PromotionType_Should_Accept_Various_Values(int? promotionType)
    {
        // Arrange & Act
        var criteria = new GetPromotionTransactionCriteria
        {
            PromotionType = promotionType
        };

        // Assert
        Assert.Equal(promotionType, criteria.PromotionType);
    }

    [Fact]
    public void Properties_Should_Accept_Various_Values()
    {
        // Arrange & Act
        var criteria = new GetPromotionTransactionCriteria
        {
            CustId = 100,
            FromDate = DateTime.Now,
            ToDate = DateTime.Now.AddDays(30),
            Status = 5,
            PromotionType = 10,
            PageSize = 50,
            PageIndex = 2,
            TotalRecords = 150,
            SiteDbGroupId = 20
        };

        // Assert
        Assert.Equal(100, criteria.CustId);
        Assert.Equal(5, criteria.Status);
        Assert.Equal(10, criteria.PromotionType);
        Assert.Equal(50, criteria.PageSize);
        Assert.Equal(2, criteria.PageIndex);
        Assert.Equal(150, criteria.TotalRecords);
        Assert.Equal(20, criteria.SiteDbGroupId);
    }
}