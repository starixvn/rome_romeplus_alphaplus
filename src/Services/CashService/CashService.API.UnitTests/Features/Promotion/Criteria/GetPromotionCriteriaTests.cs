using AlphaPlus.CashService.API.Features.Promotion.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Promotion.Criteria;

[Trait("Category", "CashService")]
public class GetPromotionCriteriaTests
{
    [Fact]
    public void Constructor_Should_Set_Properties_Correctly()
    {
        // Arrange
        var promotionType = 1;
        var status = 2;
        var siteDbGroupId = 3;
        var agentId = 4;

        // Act
        var criteria = new GetPromotionCriteria(promotionType, status, siteDbGroupId, agentId);

        // Assert
        Assert.Equal(promotionType, criteria.PromotionType);
        Assert.Equal(status, criteria.Status);
        Assert.Equal(siteDbGroupId, criteria.SiteDBGroupId);
        Assert.Equal(agentId, criteria.AgentId);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Correct_Key()
    {
        // Arrange
        var criteria = new GetPromotionCriteria(1, 2, 3, 4);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_CashService_CashPromotionSel", result);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    public void IsValid_Should_Return_Expected_Result(int agentId, bool expectedResult)
    {
        // Arrange
        var criteria = new GetPromotionCriteria(1, 2, 3, agentId);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void IsValid_Should_Return_True_When_AgentId_Is_Valid()
    {
        // Arrange
        var criteria = new GetPromotionCriteria(1, 2, 3, 10);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValid_Should_Return_False_When_AgentId_Is_Zero()
    {
        // Arrange
        var criteria = new GetPromotionCriteria(1, 2, 3, 0);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Properties_Can_Be_Modified_After_Construction()
    {
        // Arrange
        var criteria = new GetPromotionCriteria(1, 2, 3, 4);

        // Act
        criteria.PromotionType = 10;
        criteria.Status = 20;
        criteria.SiteDBGroupId = 30;
        criteria.AgentId = 40;

        // Assert
        Assert.Equal(10, criteria.PromotionType);
        Assert.Equal(20, criteria.Status);
        Assert.Equal(30, criteria.SiteDBGroupId);
        Assert.Equal(40, criteria.AgentId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-1)]
    public void IsValid_Should_Only_Depend_On_AgentId(int promotionType)
    {
        // Arrange
        var criteria = new GetPromotionCriteria(promotionType, 2, 3, 5);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-1)]
    public void IsValid_Should_Only_Depend_On_AgentId_For_Status(int status)
    {
        // Arrange
        var criteria = new GetPromotionCriteria(1, status, 3, 5);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-1)]
    public void IsValid_Should_Only_Depend_On_AgentId_For_SiteDbGroupId(int siteDbGroupId)
    {
        // Arrange
        var criteria = new GetPromotionCriteria(1, 2, siteDbGroupId, 5);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }
}