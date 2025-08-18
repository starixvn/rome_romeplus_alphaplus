using AlphaPlus.CashService.API.Features.Promotion.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Promotion.Criteria;

[Trait("Category", "CashService")]
public class RejectAllPendingPromotionCriteriaTests
{
    [Fact]
    public void Constructor_Should_Set_Properties_Correctly()
    {
        // Arrange
        var agentId = 1;
        var subAccId = 2;
        var promotionId = 3;
        var ip = "192.168.1.1";
        var siteDbGroupId = 5;

        // Act
        var criteria = new RejectAllPendingPromotionCriteria
        {
            AgentId = agentId,
            SubAccId = subAccId,
            PromotionId = promotionId,
            IP = ip,
            SiteDbGroupId = siteDbGroupId
        };

        // Assert
        Assert.Equal(agentId, criteria.AgentId);
        Assert.Equal(subAccId, criteria.SubAccId);
        Assert.Equal(promotionId, criteria.PromotionId);
        Assert.Equal(ip, criteria.IP);
        Assert.Equal(siteDbGroupId, criteria.SiteDbGroupId);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Correct_Key()
    {
        // Arrange
        var criteria = new RejectAllPendingPromotionCriteria
        {
            SiteDbGroupId = 10
        };

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal("SiteDb_10_Age_CashPromotion_RejectAll_ByPromotionID", result);
    }

    [Theory]
    [InlineData(1, 1, true)]
    [InlineData(0, 1, false)]
    [InlineData(1, 0, false)]
    [InlineData(0, 0, false)]
    [InlineData(-1, 1, false)]
    [InlineData(1, -1, false)]
    public void IsValid_Should_Return_Expected_Result(int agentId, int promotionId, bool expectedResult)
    {
        // Arrange
        var criteria = new RejectAllPendingPromotionCriteria
        {
            AgentId = agentId,
            PromotionId = promotionId
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
        var criteria = new RejectAllPendingPromotionCriteria
        {
            AgentId = 5,
            PromotionId = 10
        };

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValid_Should_Return_False_When_AgentId_Is_Zero()
    {
        // Arrange
        var criteria = new RejectAllPendingPromotionCriteria
        {
            AgentId = 0,
            PromotionId = 10
        };

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_Should_Return_False_When_PromotionId_Is_Zero()
    {
        // Arrange
        var criteria = new RejectAllPendingPromotionCriteria
        {
            AgentId = 5,
            PromotionId = 0
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
        var criteria = new RejectAllPendingPromotionCriteria
        {
            IP = null,
            ErrMsg = null,
            ErrExtraInfo = null
        };

        // Assert
        Assert.Null(criteria.IP);
        Assert.Null(criteria.ErrMsg);
        Assert.Null(criteria.ErrExtraInfo);
    }

    [Fact]
    public void Properties_Should_Accept_Empty_Strings()
    {
        // Arrange & Act
        var criteria = new RejectAllPendingPromotionCriteria
        {
            IP = "",
            ErrMsg = "",
            ErrExtraInfo = ""
        };

        // Assert
        Assert.Equal("", criteria.IP);
        Assert.Equal("", criteria.ErrMsg);
        Assert.Equal("", criteria.ErrExtraInfo);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-1)]
    public void IsValid_Should_Not_Depend_On_SubAccId(int subAccId)
    {
        // Arrange
        var criteria = new RejectAllPendingPromotionCriteria
        {
            AgentId = 5,
            PromotionId = 10,
            SubAccId = subAccId
        };

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.True(result);
    }
}
