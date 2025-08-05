using AlphaPlus.CashService.API.Features.Promotion.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Promotion.Criteria;

[Trait("Category", "CashService")]
public class GetPromotionByIdCriteriaTests
{
    [Fact]
    public void Constructor_Should_Set_Properties_Correctly()
    {
        // Arrange
        var promotionId = 1;
        var siteDbGroupId = 2;
        var agentId = 3;

        // Act
        var criteria = new GetPromotionByIdCriteria(promotionId, siteDbGroupId, agentId);

        // Assert
        Assert.Equal(promotionId, criteria.PromotionId);
        Assert.Equal(siteDbGroupId, criteria.SiteDBGroupId);
        Assert.Equal(agentId, criteria.AgentId);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Correct_Key()
    {
        // Arrange
        var criteria = new GetPromotionByIdCriteria(1, 2, 3);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_CashService_CashPromotionSelByID", result);
    }

    [Theory]
    [InlineData(1, 1, true)]
    [InlineData(0, 1, false)]
    [InlineData(1, 0, false)]
    [InlineData(0, 0, false)]
    [InlineData(-1, 1, false)]
    [InlineData(1, -1, false)]
    public void IsValid_Should_Return_Expected_Result(int promotionId, int agentId, bool expectedResult)
    {
        // Arrange
        var criteria = new GetPromotionByIdCriteria(promotionId, 2, agentId);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void IsValid_Should_Return_True_When_Required_Properties_Are_Valid()
    {
        // Arrange
        var criteria = new GetPromotionByIdCriteria(5, 2, 10);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValid_Should_Return_False_When_PromotionId_Is_Zero()
    {
        // Arrange
        var criteria = new GetPromotionByIdCriteria(0, 2, 10);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_Should_Return_False_When_AgentId_Is_Zero()
    {
        // Arrange
        var criteria = new GetPromotionByIdCriteria(5, 2, 0);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Properties_Can_Be_Modified_After_Construction()
    {
        // Arrange
        var criteria = new GetPromotionByIdCriteria(1, 2, 3);

        // Act
        criteria.PromotionId = 10;
        criteria.SiteDBGroupId = 20;
        criteria.AgentId = 30;

        // Assert
        Assert.Equal(10, criteria.PromotionId);
        Assert.Equal(20, criteria.SiteDBGroupId);
        Assert.Equal(30, criteria.AgentId);
    }
}