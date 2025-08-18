using AlphaPlus.CashService.API.Features.Promotion.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Promotion.Criteria;

[Trait("Category", "CashService")]
public class UpdatePromotionStatusCriteriaTests
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
        var enabled = true;
        var language = "en";

        // Act
        var criteria = new UpdatePromotionStatusCriteria
        {
            PromotionId = promotionId,
            AgentId = agentId,
            SubAccId = subAccId,
            IP = ip,
            SiteDbGroupId = siteDbGroupId,
            Enabled = enabled,
            Language = language
        };

        // Assert
        Assert.Equal(promotionId, criteria.PromotionId);
        Assert.Equal(agentId, criteria.AgentId);
        Assert.Equal(subAccId, criteria.SubAccId);
        Assert.Equal(ip, criteria.IP);
        Assert.Equal(siteDbGroupId, criteria.SiteDbGroupId);
        Assert.Equal(enabled, criteria.Enabled);
        Assert.Equal(language, criteria.Language);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Correct_Key()
    {
        // Arrange
        var criteria = new UpdatePromotionStatusCriteria();

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_CashService_CashPromotionStatusUpd", result);
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
        var criteria = new UpdatePromotionStatusCriteria
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
        var criteria = new UpdatePromotionStatusCriteria
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
        var criteria = new UpdatePromotionStatusCriteria
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
        var criteria = new UpdatePromotionStatusCriteria
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
        var criteria = new UpdatePromotionStatusCriteria
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
        var criteria = new UpdatePromotionStatusCriteria
        {
            IP = "",
            Language = ""
        };

        // Assert
        Assert.Equal("", criteria.IP);
        Assert.Equal("", criteria.Language);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Enabled_Should_Accept_Boolean_Values(bool enabled)
    {
        // Arrange & Act
        var criteria = new UpdatePromotionStatusCriteria
        {
            Enabled = enabled
        };

        // Assert
        Assert.Equal(enabled, criteria.Enabled);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-1)]
    public void IsValid_Should_Not_Depend_On_SubAccId(int subAccId)
    {
        // Arrange
        var criteria = new UpdatePromotionStatusCriteria
        {
            PromotionId = 5,
            AgentId = 10,
            SubAccId = subAccId
        };

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }
}
