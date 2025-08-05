using AlphaPlus.CashService.API.Features.Promotion.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Promotion.Criteria;

[Trait("Category", "CashService")]
public class EditPromotionCriteriaTests
{
    [Fact]
    public void Constructor_Should_Set_Properties_Correctly()
    {
        // Arrange
        var promotionId = 1;
        var agentId = 2;
        var subAccId = 3;
        var ip = "192.168.1.1";
        var siteDbGroupId = 4;
        var promotionContents = "Updated promotion content";
        var language = "en";

        // Act
        var criteria = new EditPromotionCriteria
        {
            PromotionId = promotionId,
            AgentId = agentId,
            SubAccId = subAccId,
            IP = ip,
            SiteDbGroupId = siteDbGroupId,
            PromotionContents = promotionContents,
            Language = language
        };

        // Assert
        Assert.Equal(promotionId, criteria.PromotionId);
        Assert.Equal(agentId, criteria.AgentId);
        Assert.Equal(subAccId, criteria.SubAccId);
        Assert.Equal(ip, criteria.IP);
        Assert.Equal(siteDbGroupId, criteria.SiteDbGroupId);
        Assert.Equal(promotionContents, criteria.PromotionContents);
        Assert.Equal(language, criteria.Language);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Correct_Key()
    {
        // Arrange
        var criteria = new EditPromotionCriteria();

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_CashService_CashPromotionUpd", result);
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
        var criteria = new EditPromotionCriteria
        {
            PromotionId = promotionId,
            AgentId = agentId
        };

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void IsValid_Should_Return_True_When_Required_Properties_Are_Valid()
    {
        // Arrange
        var criteria = new EditPromotionCriteria
        {
            PromotionId = 5,
            AgentId = 10
        };

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValid_Should_Return_False_When_PromotionId_Is_Zero()
    {
        // Arrange
        var criteria = new EditPromotionCriteria
        {
            PromotionId = 0,
            AgentId = 10
        };

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_Should_Return_False_When_AgentId_Is_Zero()
    {
        // Arrange
        var criteria = new EditPromotionCriteria
        {
            PromotionId = 5,
            AgentId = 0
        };

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Properties_Should_Accept_Null_Values()
    {
        // Arrange & Act
        var criteria = new EditPromotionCriteria
        {
            IP = null,
            PromotionContents = null,
            Language = null
        };

        // Assert
        Assert.Null(criteria.IP);
        Assert.Null(criteria.PromotionContents);
        Assert.Null(criteria.Language);
    }

    [Fact]
    public void Properties_Should_Accept_Empty_Strings()
    {
        // Arrange & Act
        var criteria = new EditPromotionCriteria
        {
            IP = "",
            PromotionContents = "",
            Language = ""
        };

        // Assert
        Assert.Equal("", criteria.IP);
        Assert.Equal("", criteria.PromotionContents);
        Assert.Equal("", criteria.Language);
    }
}