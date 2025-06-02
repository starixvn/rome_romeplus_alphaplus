using AlphaPlus.Reports.Features.MatchWinLossDetail;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.UnitTests.Features.MatchWinLossDetail;

[Trait("Category", "MatchWinLossDetail")]
public class BetTypeWinLossModelTest
{
    [Fact]
    public void Constructor_Should_Initialize_Properties_From_Entity()
    {
        // Arrange
        var entity = new MatchWinLossDetailEntity
        {
            BetType = 1,
            Turnover = 1000,
            GrossComm = 100,
            LiveIndicator = false,
            Race = 2,
            MemberWinLoss = 500,
            MemberComm = 50,
            AgWinLoss = 300,
            AgComm = 30,
            MAgWinLoss = 200,
            MAgComm = 20,
            SAgWinLoss = 100,
            SAgComm = 10
        };
        var betTypeName = "Test Bet Type";

        // Act
        var model = new BetTypeWinLossModel(entity, betTypeName);

        // Assert
        Assert.Equal(entity.BetType, model.Id);
        Assert.Equal(betTypeName, model.Name);
        Assert.Equal(entity.Turnover, model.Turnover);
        Assert.Equal(entity.GrossComm, model.GrossComm);
        Assert.Equal(entity.LiveIndicator, model.IsLive);
        Assert.Equal(entity.Race, model.Race);
        Assert.Equal(entity.MemberWinLoss, model.MemberWinLoss);
        Assert.Equal(entity.MemberComm, model.MemberComm);
        Assert.Equal(entity.AgWinLoss, model.AgWinLoss);
        Assert.Equal(entity.AgComm, model.AgComm);
        Assert.Equal(entity.MAgWinLoss, model.MAgWinLoss);
        Assert.Equal(entity.MAgComm, model.MAgComm);
        Assert.Equal(entity.SAgWinLoss, model.SAgWinLoss);
        Assert.Equal(entity.SAgComm, model.SAgComm);
        Assert.Equal(entity.CompanyComm, model.CompanyComm);
    }

    [Fact]
    public void Constructor_Should_Handle_Null_BetTypeName()
    {
        // Arrange
        var entity = new MatchWinLossDetailEntity { BetType = 1 };

        // Act
        var model = new BetTypeWinLossModel(entity);

        // Assert
        Assert.Equal(1, model.Id);
        Assert.Equal(string.Empty, model.Name);
    }

    [Fact]
    public void Constructor_Should_Add_Next_Prefix_For_Over_Under_BetType()
    {
        // Arrange
        var entity = new MatchWinLossDetailEntity { BetType = BetTypeConstants.N_Over_Under };
        var betTypeName = "Over Under";

        // Act
        var model = new BetTypeWinLossModel(entity, betTypeName);

        // Assert
        Assert.Equal($"{SharedResources.Next} {betTypeName}", model.Name);
    }

    [Fact]
    public void Constructor_Should_Add_Next_Prefix_For_Odd_Even_BetType_When_IsLive()
    {
        // Arrange
        var entity = new MatchWinLossDetailEntity
        {
            BetType = BetTypeConstants.N_Odd_Even,
            LiveIndicator = true
        };
        var betTypeName = "Odd Even";

        // Act
        var model = new BetTypeWinLossModel(entity, betTypeName);

        // Assert
        Assert.Equal($"{SharedResources.Next} {betTypeName}", model.Name);
    }

    [Fact]
    public void Constructor_Should_Add_FT_Prefix_For_Odd_Even_BetType_When_Not_IsLive()
    {
        // Arrange
        var entity = new MatchWinLossDetailEntity
        {
            BetType = BetTypeConstants.N_Odd_Even,
            LiveIndicator = false
        };
        var betTypeName = "Odd Even";

        // Act
        var model = new BetTypeWinLossModel(entity, betTypeName);

        // Assert
        Assert.Equal($"{SharedResources.FT} {betTypeName}", model.Name);
    }

    [Theory]
    [InlineData(BetTypeConstants.MixParlayBetRadarVirtualSports)]
    [InlineData(BetTypeConstants.MixParlayEdgeGamingVirtualSports)]
    [InlineData(BetTypeConstants.MixParlayVirtuallySports)]
    public void Constructor_Should_Add_VirtualSports_Suffix_For_VirtualSports_BetTypes(int betType)
    {
        // Arrange
        var entity = new MatchWinLossDetailEntity { BetType = betType };
        var betTypeName = "Parlay";

        // Act
        var model = new BetTypeWinLossModel(entity, betTypeName);

        // Assert
        Assert.Equal($"{betTypeName} ({SharedResources.VirtualSports})", model.Name);
    }

    [Fact]
    public void Constructor_Should_Not_Add_Prefix_Or_Suffix_For_Regular_BetTypes()
    {
        // Arrange
        var entity = new MatchWinLossDetailEntity { BetType = 100 }; // Regular bet type
        var betTypeName = "Regular Bet";

        // Act
        var model = new BetTypeWinLossModel(entity, betTypeName);

        // Assert
        Assert.Equal(betTypeName, model.Name);
    }
}