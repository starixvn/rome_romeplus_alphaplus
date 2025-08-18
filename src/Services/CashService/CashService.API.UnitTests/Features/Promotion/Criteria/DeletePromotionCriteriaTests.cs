using AlphaPlus.CashService.API.Features.Promotion.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Promotion.Criteria;

[Trait("Category", "CashService")]
public class DeletePromotionCriteriaTests
{
    [Fact]
    public void Constructor_Should_Set_Properties_Correctly()
    {
        // Arrange
        var promotionId = 1;
        var siteDbGroupId = 2;
        var agentId = 3;
        var subAccId = 4;
        var ip = "192.168.1.1";
        var language = "en";
        var custDbGroupId = 5;

        // Act
        var criteria = new DeletePromotionCriteria
        {
            PromotionId = promotionId,
            SiteDbGroupId = siteDbGroupId,
            AgentId = agentId,
            SubAccId = subAccId,
            IP = ip,
            Language = language,
            CustDbGroupId = custDbGroupId
        };

        // Assert
        Assert.Equal(promotionId, criteria.PromotionId);
        Assert.Equal(siteDbGroupId, criteria.SiteDbGroupId);
        Assert.Equal(agentId, criteria.AgentId);
        Assert.Equal(subAccId, criteria.SubAccId);
        Assert.Equal(ip, criteria.IP);
        Assert.Equal(language, criteria.Language);
        Assert.Equal(custDbGroupId, criteria.CustDbGroupId);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Correct_Key()
    {
        // Arrange
        var criteria = new DeletePromotionCriteria
        {
            CustDbGroupId = 10
        };

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("CustDb_10_Age_CashService_CashPromotionDel", result);
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
        var criteria = new DeletePromotionCriteria
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
        var criteria = new DeletePromotionCriteria
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
        var criteria = new DeletePromotionCriteria
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
        var criteria = new DeletePromotionCriteria
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
        var criteria = new DeletePromotionCriteria
        {
            IP = null,
            Language = null
        };

        // Assert
        Assert.Null(criteria.IP);
        Assert.Null(criteria.Language);
    }

    [Fact]
    public void Properties_Should_Accept_Empty_Strings()
    {
        // Arrange & Act
        var criteria = new DeletePromotionCriteria
        {
            IP = "",
            Language = ""
        };

        // Assert
        Assert.Equal("", criteria.IP);
        Assert.Equal("", criteria.Language);
    }

    [Fact]
    public void ErrMsg_Should_Default_To_Empty_String()
    {
        // Arrange & Act
        var criteria = new DeletePromotionCriteria();

        // Assert
        Assert.Equal(string.Empty, criteria.ErrMsg);
    }
}