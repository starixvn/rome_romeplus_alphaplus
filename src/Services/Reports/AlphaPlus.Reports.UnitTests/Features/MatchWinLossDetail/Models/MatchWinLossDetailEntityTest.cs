using AlphaPlus.Reports.Features.MatchWinLossDetail;

namespace AlphaPlus.Reports.UnitTests.Features.MatchWinLossDetail;

[Trait("Category", "MatchWinLossDetail")]
public class MatchWinLossDetailEntityTest
{
    [Theory]
    [InlineData(3, true)]
    [InlineData(4, true)]
    [InlineData(1, false)]
    [InlineData(2, false)]
    [InlineData(5, false)]
    public void IsParlay_ShouldReturnExpectedResult_BasedOnBetGroup(int betGroup, bool expected)
    {
        // Arrange
        var entity = new MatchWinLossDetailEntity { BetGroup = betGroup };

        // Act
        var result = entity.IsParlay;

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void CompanyComm_ShouldReturnSum_OfAllCommissions()
    {
        // Arrange
        var entity = new MatchWinLossDetailEntity
        {
            MemberComm = 10,
            AgComm = 20,
            MAgComm = 30,
            SAgComm = 40
        };

        // Act
        var result = entity.CompanyComm;

        // Assert
        Assert.Equal(100m, result);
    }

    [Fact]
    public void Addition_Operator_ShouldAdd_TwoEntities()
    {
        // Arrange
        var entity1 = new MatchWinLossDetailEntity
        {
            MatchId = 1,
            Turnover = 100,
            MemberWinLoss = 50,
            MemberComm = 10,
            AgComm = 15,
            MAgComm = 5,
            SAgComm = 10,
            GrossComm = 50
        };

        var entity2 = new MatchWinLossDetailEntity
        {
            MatchId = 2,
            Turnover = 200,
            MemberWinLoss = -30,
            MemberComm = 20,
            AgComm = 25,
            MAgComm = 15,
            SAgComm = 20,
            GrossComm = 100
        };

        // Act
        var result = entity1 + entity2;

        // Assert
        Assert.Equal(1, result.MatchId); // Keeps the first entity's MatchId
        Assert.Equal(300, result.Turnover); // 100 + 200
        Assert.Equal(20, result.MemberWinLoss); // 50 + (-30)
        Assert.Equal(30, result.MemberComm); // 10 + 20
        Assert.Equal(40, result.AgComm); // 15 + 25
        Assert.Equal(20, result.MAgComm); // 5 + 15
        Assert.Equal(30, result.SAgComm); // 10 + 20
        Assert.Equal(150, result.GrossComm); // 50 + 100
        Assert.Equal(120, result.CompanyComm); // 30 + 40 + 20 + 30
    }

    [Fact]
    public void CompanyComm_ShouldIgnore_SetterValue()
    {
        // Arrange
        var entity = new MatchWinLossDetailEntity
        {
            MemberComm = 10,
            AgComm = 20,
            MAgComm = 30,
            SAgComm = 40
        };

        // Use reflection to try to set the property
        var property = typeof(MatchWinLossDetailEntity).GetProperty("CompanyComm");

        // Act & Assert
        // We're just verifying that getter works correctly even after attempting to set
        Assert.Equal(100m, entity.CompanyComm);

        // No direct way to test the setter is ignored, but we can verify the property
        // continues to return the calculated value
        property?.SetValue(entity, 999m);
        Assert.Equal(100m, entity.CompanyComm);
    }
}