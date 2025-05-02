using AlphaPlus.BetsForecast.Domain.Forecast;
using AlphaPlus.BetsForecast.Infrastructure.Forecast;
using AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;
using AlphaPlus.BetsForecast.Infrastructure.Shared;
using AlphaPlus.PartialUM.Services;
using Fanex.Data.Repository;
using Match = AlphaPlus.BetsForecast.Domain.Forecast.Match;

namespace AlphaPlus.BetsForecast.Infrastructure.UnitTests.Forecast;

[Trait("Category", "BetsForecast")]
public class SoccerServiceTest
{
    private readonly Mock<IDynamicRepository> _repositoryMock;
    private readonly Mock<ICacheRefService> _cacheRefServiceMock;
    private readonly Mock<IPartialUMService> _partialUMServiceMock;
    private readonly SoccerService _soccerService;

    public SoccerServiceTest()
    {
        _repositoryMock = new Mock<IDynamicRepository>();
        _cacheRefServiceMock = new Mock<ICacheRefService>();
        _partialUMServiceMock = new Mock<IPartialUMService>();
        _soccerService = new SoccerService(_repositoryMock.Object, _cacheRefServiceMock.Object, _partialUMServiceMock.Object);
    }

    [Fact]
    public async Task GetLeagues_Should_Return_Leagues_Ordered_By_Name()
    {
        // Arrange
        var isLive = false;
        var isShowAll = true;
        var language = "en";
        var siteName = "test";
        var leagues = new List<League>
        {
            new League { LeagueId = 1 },
            new League { LeagueId = 2 }
        };

        _repositoryMock.Setup(r => r.FetchAsync<League>(It.IsAny<GetSoccerLeagueCriteria>(), string.Empty, default))
            .ReturnsAsync(leagues);

        _cacheRefServiceMock.Setup(s => s.GetLeagueName(1, language)).Returns("B League");
        _cacheRefServiceMock.Setup(s => s.GetLeagueName(2, language)).Returns("A League");

        // Act
        var result = (await _soccerService.GetLeagues(isLive, isShowAll, language, siteName)).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("A League", result[0].LeagueName);
        Assert.Equal("B League", result[1].LeagueName);
        _partialUMServiceMock.Verify(s => s.ThrowIfUM(It.IsAny<string>(), siteName), Times.Once);
    }

    [Fact]
    public async Task GetMatches_Should_Return_Matches_With_Team_Names()
    {
        // Arrange
        var leagueId = 1;
        var isLive = false;
        var language = "en";
        var siteName = "test";
        var matches = new List<Match>
        {
            new Match { MatchId = 1, HomeId = 101, AwayId = 102 }
        };

        _repositoryMock.Setup(r => r.FetchAsync<Match>(It.IsAny<GetSoccerMatchCriteria>(), string.Empty, default))
            .ReturnsAsync(matches);

        _cacheRefServiceMock.Setup(s => s.GetTeamName(101, language)).Returns("Home Team");
        _cacheRefServiceMock.Setup(s => s.GetTeamName(102, language)).Returns("Away Team");

        // Act
        var result = (await _soccerService.GetMatches(leagueId, isLive, language, siteName)).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal("Home Team", result[0].HomeName);
        Assert.Equal("Away Team", result[0].AwayName);
        Assert.Equal("Home Team - Away Team", result[0].Event);
        _partialUMServiceMock.Verify(s => s.ThrowIfUM(It.IsAny<string>(), siteName), Times.Once);
    }

    [Fact]
    public async Task GetResults_Should_Return_Empty_When_No_Matches()
    {
        // Arrange
        var custId = 1;
        var matchIds = "1,2,3";
        var isLive = false;
        var language = "en";
        var siteName = "test";

        _repositoryMock.Setup(r => r.FetchMultipleAsync<SoccerMatch, SoccerBetType, SoccerWinLoss>(
                It.IsAny<GetSoccerForecastCriteria>(), string.Empty, default))
            .ReturnsAsync((new List<SoccerMatch>(), new List<SoccerBetType>(), new List<SoccerWinLoss>()));

        // Act
        var result = await _soccerService.GetResults(custId, matchIds, isLive, language, siteName);

        // Assert
        Assert.Empty(result);
        _partialUMServiceMock.Verify(s => s.ThrowIfUM(It.IsAny<string>(), siteName), Times.Once);
    }

    [Fact]
    public async Task GetResults_Should_Build_Results_From_Matches()
    {
        // Arrange
        var custId = 1;
        var matchIds = "1";
        var isLive = false;
        var language = "en";
        var siteName = "test";

        var matches = new List<SoccerMatch>
        {
            new SoccerMatch
            {
                MatchId = 1,
                HomeId = 101,
                AwayId = 102,
                KickoffTime = DateTime.Now.AddHours(1),
                IsLive = isLive
            }
        };

        var betTypes = new List<SoccerBetType>
        {
            new SoccerBetType
            {
                MatchId = 1,
                BetTypeId = BetTypeConstants.Handicap,
                WinLoss1 = 10.0m,
                WinLoss2 = 20.0m,
                WinLoss3 = 30.0m
            }
        };

        var winloss = new List<SoccerWinLoss>
        {
            new SoccerWinLoss
            {
                MatchId = 1,
                BetTypeId = BetTypeConstants.Handicap,
                BetCount = 5,
                Turnover = 100.0m,
                Column1 = 1,
                Column2 = 2,
                Column3 = 0
            }
        };

        _repositoryMock.Setup(r => r.FetchMultipleAsync<SoccerMatch, SoccerBetType, SoccerWinLoss>(
                It.IsAny<GetSoccerForecastCriteria>(), string.Empty, default))
            .ReturnsAsync((matches, betTypes, winloss));

        _cacheRefServiceMock.Setup(s => s.GetTeamName(101, language)).Returns("Home Team");
        _cacheRefServiceMock.Setup(s => s.GetTeamName(102, language)).Returns("Away Team");
        _cacheRefServiceMock.Setup(s => s.GetBetTypeName(BetTypeConstants.Handicap, language)).Returns("Handicap");

        // Act
        var result = (await _soccerService.GetResults(custId, matchIds, isLive, language, siteName)).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal(1, result[0].MatchId);
        Assert.Equal("Home Team", result[0].HomeName);
        Assert.Equal("Away Team", result[0].AwayName);
        Assert.Equal(5, result[0].TotalBetCount);
        Assert.Equal(100.0m, result[0].TotalTurnover);

        var betTypeForecast = result[0].BetTypeForecasts.First();
        Assert.Equal(BetTypeConstants.Handicap, betTypeForecast.BetType);
        Assert.Equal("Handicap", betTypeForecast.BetTypeName);
        Assert.Equal(5, betTypeForecast.BetCount);
        Assert.Equal(100.0m, betTypeForecast.Turnover);
        Assert.True(betTypeForecast.IsHandicap);
        Assert.Equal(2, betTypeForecast.MainWinLossIndex);
    }

    [Fact]
    public async Task GetResults_Should_Filter_BetTypes_For_Live_Matches_After_HalfTime()
    {
        // Arrange
        var custId = 1;
        var matchIds = "1";
        var isLive = true;
        var language = "en";
        var siteName = "test";

        var matches = new List<SoccerMatch>
        {
            new SoccerMatch
            {
                MatchId = 1,
                HomeId = 101,
                AwayId = 102,
                KickoffTime = DateTime.Now.AddMinutes(-46), // More than 45 minutes ago
                IsLive = isLive
            }
        };

        var betTypes = new List<SoccerBetType>
        {
            new SoccerBetType
            {
                MatchId = 1,
                BetTypeId = BetTypeConstants.Handicap, // FullTime
                WinLoss1 = 10.0m,
                WinLoss2 = 20.0m,
                WinLoss3 = 30.0m
            },
            new SoccerBetType
            {
                MatchId = 1,
                BetTypeId = BetTypeConstants._1stHandicap, // Not FullTime
                WinLoss1 = 10.0m,
                WinLoss2 = 20.0m,
                WinLoss3 = 30.0m
            }
        };

        var winloss = new List<SoccerWinLoss>
        {
            new SoccerWinLoss
            {
                MatchId = 1,
                BetTypeId = BetTypeConstants.Handicap,
                BetCount = 5,
                Turnover = 100.0m
            },
            new SoccerWinLoss
            {
                MatchId = 1,
                BetTypeId = BetTypeConstants._1stHandicap,
                BetCount = 3,
                Turnover = 50.0m
            }
        };

        _repositoryMock.Setup(r => r.FetchMultipleAsync<SoccerMatch, SoccerBetType, SoccerWinLoss>(
                It.IsAny<GetSoccerForecastCriteria>(), string.Empty, default))
            .ReturnsAsync((matches, betTypes, winloss));

        _cacheRefServiceMock.Setup(s => s.GetBetTypeName(It.IsAny<int>(), language)).Returns("BetType");

        // Act
        var result = (await _soccerService.GetResults(custId, matchIds, isLive, language, siteName)).ToList();

        // Assert
        Assert.Single(result);

        // Should only include full time bet types after half time
        Assert.Single(result[0].BetTypeForecasts);
        Assert.Equal(BetTypeConstants.Handicap, result[0].BetTypeForecasts.First().BetType);
    }

    [Fact]
    public void BuildShowTime_Should_Format_Show_Time_Correctly()
    {
        // Using reflection to test private method
        var methodInfo = typeof(SoccerService).GetMethod("BuildShowTime",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

        // Test empty string
        var result1 = methodInfo.Invoke(null, new object[] { "" });
        Assert.Equal(string.Empty, result1);

        // Test single part
        var result2 = methodInfo.Invoke(null, new object[] { "Today" });
        Assert.Equal("Today", result2);

        // Test two parts
        var result3 = methodInfo.Invoke(null, new object[] { "Today 12:00" });
        Assert.Equal("Today<br/>12:00", result3);

        // Test multiple parts
        var result4 = methodInfo.Invoke(null, new object[] { "Monday 12 June 12:00" });
        Assert.Equal("Monday 12 June<br/>12:00", result4);
    }
}
