using AlphaPlus.Reports.Features.Consolidation;

namespace AlphaPlus.Reports.UnitTests.Features.Consolidation;

[Trait("Category", "Consolidation")]
public class ConsolidationEntityTest
{
    [Fact]
    public void PositionTakingText_Should_Format_PositionTaking_Correctly()
    {
        // Arrange
        var entity = new ConsolidationEntity { PositionTaking = 0.05m };

        // Act
        var result = entity.PositionTakingText;

        // Assert
        Assert.Equal("5.0% PT", result);
    }

    [Theory]
    [InlineData(4, false)] // Super
    [InlineData(3, false)] // Master
    [InlineData(2, false)] // Agent
    [InlineData(1, true)]  // Member
    public void IsMemberLevel_Should_Return_Expected_Result(byte custLevel, bool expected)
    {
        // Arrange
        var entity = new ConsolidationEntity { CustLevel = custLevel };

        // Act
        var result = entity.IsMemberLevel();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(4, "Super")]
    [InlineData(3, "Master")]
    [InlineData(2, "Agent")]
    [InlineData(1, "Member")]
    public void CustLevelName_Should_Return_Correct_Level_Name(byte custLevel, string expectedName)
    {
        // Arrange
        var entity = new ConsolidationEntity { CustLevel = custLevel };

        // Act
        var result = entity.CustLevelName;

        // Assert
        Assert.Equal(expectedName, result);
    }

    [Fact]
    public void Addition_Operator_Should_Add_Values_Correctly()
    {
        // Arrange
        var entity1 = new ConsolidationEntity
        {
            Turnover = 100m,
            GrossComm = 10m,
            MemberWinLoss = 20m,
            MemberComm = 5m,
            MemberTotal = 25m,
            OwnerTurnover = 50m,
            OwnerWinLoss = 15m,
            OwnerComm = 3m,
            OwnerTotal = 18m,
            Company = 7m,
            BetCount = 5
        };

        var entity2 = new ConsolidationEntity
        {
            Turnover = 200m,
            GrossComm = 20m,
            MemberWinLoss = 30m,
            MemberComm = 10m,
            MemberTotal = 40m,
            OwnerTurnover = 100m,
            OwnerWinLoss = 25m,
            OwnerComm = 7m,
            OwnerTotal = 32m,
            Company = 8m,
            BetCount = 10
        };

        // Act
        var result = entity1 + entity2;

        // Assert
        Assert.Equal(300m, result.Turnover);
        Assert.Equal(30m, result.GrossComm);
        Assert.Equal(50m, result.MemberWinLoss);
        Assert.Equal(15m, result.MemberComm);
        Assert.Equal(65m, result.MemberTotal);
        Assert.Equal(150m, result.OwnerTurnover);
        Assert.Equal(40m, result.OwnerWinLoss);
        Assert.Equal(10m, result.OwnerComm);
        Assert.Equal(50m, result.OwnerTotal);
        Assert.Equal(15m, result.Company);
        Assert.Equal(15, result.BetCount);
    }
}