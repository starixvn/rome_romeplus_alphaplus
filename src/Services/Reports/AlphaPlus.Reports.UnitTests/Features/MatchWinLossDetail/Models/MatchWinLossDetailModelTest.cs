using AlphaPlus.Reports.Features.MatchWinLossDetail;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.UnitTests.Features.MatchWinLossDetail;

[Trait("Category", "MatchWinLossDetail")]
public class MatchWinLossDetailModelTest
{
    [Fact]
    public void Constructor_Should_Initialize_Properties_Correctly()
    {
        // Arrange
        var entity = new MatchWinLossDetailEntity
        {
            LeagueId = 100,
            MatchId = 200,
            Race = 1,
            BetType = 1,
            IsNeutral = false
        };

        var betTypeWinLoss = new List<BetTypeWinLossModel>
        {
            new BetTypeWinLossModel(entity) { Turnover = 1000 }
        };

        var total = new BetTypeWinLossModel(entity) { Turnover = 1000 };

        // Act
        var model = new MatchWinLossDetailModel(
            entity,
            "Premier League",
            betTypeWinLoss,
            total,
            "Liverpool",
            "Manchester United",
            1);

        // Assert
        Assert.Equal(100, model.LeagueId);
        Assert.Equal(1, model.LeagueType);
        Assert.Equal("Premier League", model.LeagueName);
        Assert.Equal("Liverpool", model.HomeName);
        Assert.Equal("Manchester United", model.AwayName);
        Assert.Equal(200, model.MatchId);
        Assert.Equal(1, model.Race);
        Assert.Equal(betTypeWinLoss, model.BetTypeWinLoss);
        Assert.Equal(total, model.Total);
        Assert.Equal("Liverpool - vs - Manchester United", model.MatchName);
        Assert.True(model.RenderSubTotal);
    }

    [Fact]
    public void Constructor_Should_Set_MatchName_With_Neutral_Indicator()
    {
        // Arrange
        var entity = new MatchWinLossDetailEntity
        {
            IsNeutral = true,
            BetType = 1
        };

        // Act
        var model = new MatchWinLossDetailModel(
            entity,
            "League",
            Enumerable.Empty<BetTypeWinLossModel>(),
            new BetTypeWinLossModel(entity),
            "TeamA",
            "TeamB",
            1);

        // Assert
        Assert.Equal("TeamA(N) - vs - TeamB", model.MatchName);
    }

    [Fact]
    public void Constructor_Should_Set_MatchName_For_Outright_BetType()
    {
        // Arrange
        var entity = new MatchWinLossDetailEntity
        {
            BetType = BetTypeConstants.Outright
        };

        // Act
        var model = new MatchWinLossDetailModel(
            entity,
            "League",
            Enumerable.Empty<BetTypeWinLossModel>(),
            new BetTypeWinLossModel(entity),
            "TeamA",
            "TeamB",
            1);

        // Assert
        Assert.Equal(string.Empty, model.MatchName);
    }

    [Fact]
    public void Constructor_Should_Set_MatchName_For_NumberGame_BetType()
    {
        // Arrange
        var entity = new MatchWinLossDetailEntity
        {
            BetType = 90,
            MatchCode = "123"
        };

        // Act
        var model = new MatchWinLossDetailModel(
            entity,
            "NumberGameLeague",
            Enumerable.Empty<BetTypeWinLossModel>(),
            new BetTypeWinLossModel(entity),
            "TeamA",
            "TeamB",
            1);

        // Assert
        Assert.Equal($"{SharedResources.NumberGameNo} 123", model.MatchName);
        Assert.Equal($"{SharedResources.NumberGame} - NumberGameLeague", model.LeagueName);
    }

    [Fact]
    public void Constructor_Should_Set_LeagueName_For_NumberGame_When_LeagueName_Empty()
    {
        // Arrange
        var entity = new MatchWinLossDetailEntity
        {
            BetType = 100,
        };

        // Act
        var model = new MatchWinLossDetailModel(
            entity,
            string.Empty,
            Enumerable.Empty<BetTypeWinLossModel>(),
            new BetTypeWinLossModel(entity),
            "TeamA",
            "TeamB",
            1);

        // Assert
        Assert.Equal(SharedResources.NumberGame, model.LeagueName);
    }

    [Fact]
    public void Constructor_Should_Set_MatchName_For_Happy5_BetType()
    {
        // Arrange
        var entity = new MatchWinLossDetailEntity
        {
            BetType = 8105,
            MatchCode = "789"
        };

        // Act
        var model = new MatchWinLossDetailModel(
            entity,
            "Happy5League",
            Enumerable.Empty<BetTypeWinLossModel>(),
            new BetTypeWinLossModel(entity),
            "TeamA",
            "TeamB",
            1);

        // Assert
        Assert.Equal($"{SharedResources.No} 789", model.MatchName);
        Assert.Equal($"{SharedResources.Happy5} - Happy5League", model.LeagueName);
    }

    [Fact]
    public void Constructor_Should_Set_LeagueName_For_Happy5_When_LeagueName_Empty()
    {
        // Arrange
        var entity = new MatchWinLossDetailEntity
        {
            BetType = 8107
        };

        // Act
        var model = new MatchWinLossDetailModel(
            entity,
            string.Empty,
            Enumerable.Empty<BetTypeWinLossModel>(),
            new BetTypeWinLossModel(entity),
            "TeamA",
            "TeamB",
            1);

        // Assert
        Assert.Equal(SharedResources.Happy5, model.LeagueName);
    }

    [Fact]
    public void Constructor_Should_Set_MatchName_For_VirtualHorseRacing_BetType()
    {
        // Arrange
        var dateTimeString = "2023-05-15 14:30:00";
        var entity = new MatchWinLossDetailEntity
        {
            BetType = BetTypeConstants.VirtualHorseRacing_Win,
            ShowTime = dateTimeString
        };

        // Act
        var model = new MatchWinLossDetailModel(
            entity,
            "League",
            Enumerable.Empty<BetTypeWinLossModel>(),
            new BetTypeWinLossModel(entity),
            "TeamA",
            "TeamB",
            1);

        // Assert
        Assert.Equal(DateTime.Parse(dateTimeString).ToLongDateWithSlash(), model.MatchName);
    }

    [Fact]
    public void Constructor_Should_Set_MatchName_For_VirtualSport7_BetType()
    {
        // Arrange
        var dateTimeString = "2023-05-15 14:30:00";
        var entity = new MatchWinLossDetailEntity
        {
            BetType = 15501,
            ShowTime = dateTimeString
        };

        // Act
        var model = new MatchWinLossDetailModel(
            entity,
            "League",
            Enumerable.Empty<BetTypeWinLossModel>(),
            new BetTypeWinLossModel(entity),
            "TeamA",
            "TeamB",
            1);

        // Assert
        Assert.Equal(DateTime.Parse(dateTimeString).ToLongDateWithSlash(), model.MatchName);
    }

    [Fact]
    public void Constructor_Should_Set_Empty_MatchName_For_VirtualSport_With_Invalid_DateTime()
    {
        // Arrange
        var entity = new MatchWinLossDetailEntity
        {
            BetType = 15502,
            ShowTime = "invalid-datetime"
        };

        // Act
        var model = new MatchWinLossDetailModel(
            entity,
            "League",
            Enumerable.Empty<BetTypeWinLossModel>(),
            new BetTypeWinLossModel(entity),
            "TeamA",
            "TeamB",
            1);

        // Assert
        Assert.Equal(string.Empty, model.MatchName);
    }

    [Fact]
    public void RenderSubTotal_Should_Return_True_When_MatchId_Not_Zero()
    {
        // Arrange
        var entity = new MatchWinLossDetailEntity { MatchId = 100, Race = 0 };

        // Act
        var model = new MatchWinLossDetailModel(
            entity,
            "League",
            Enumerable.Empty<BetTypeWinLossModel>(),
            new BetTypeWinLossModel(entity),
            "TeamA",
            "TeamB",
            1);

        // Assert
        Assert.True(model.RenderSubTotal);
    }

    [Fact]
    public void RenderSubTotal_Should_Return_True_When_Race_Not_Zero()
    {
        // Arrange
        var entity = new MatchWinLossDetailEntity { MatchId = 0, Race = 5 };

        // Act
        var model = new MatchWinLossDetailModel(
            entity,
            "League",
            Enumerable.Empty<BetTypeWinLossModel>(),
            new BetTypeWinLossModel(entity),
            "TeamA",
            "TeamB",
            1);

        // Assert
        Assert.True(model.RenderSubTotal);
    }

    [Fact]
    public void RenderSubTotal_Should_Return_False_When_Both_MatchId_And_Race_Are_Zero()
    {
        // Arrange
        var entity = new MatchWinLossDetailEntity { MatchId = 0, Race = 0 };

        // Act
        var model = new MatchWinLossDetailModel(
            entity,
            "League",
            Enumerable.Empty<BetTypeWinLossModel>(),
            new BetTypeWinLossModel(entity),
            "TeamA",
            "TeamB",
            1);

        // Assert
        Assert.False(model.RenderSubTotal);
    }
}