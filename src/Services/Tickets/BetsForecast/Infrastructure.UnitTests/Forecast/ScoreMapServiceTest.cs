using AlphaPlus.BetsForecast.Domain.Forecast;
using AlphaPlus.BetsForecast.Infrastructure.Forecast;
using AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;
using AlphaPlus.BetsForecast.Infrastructure.Forecast.Params;
using AlphaPlus.BetsForecast.Infrastructure.Shared;
using AlphaPlus.PartialUM.Services;
using Fanex.Data.Repository;
using Match = AlphaPlus.BetsForecast.Domain.Forecast.Match;

namespace AlphaPlus.BetsForecast.Infrastructure.UnitTests.Forecast;

[Trait("Category", "BetsForecast")]
public class ScoreMapServiceTest
{
    private readonly Mock<IDynamicRepository> _repositoryMock;
    private readonly Mock<ICacheRefService> _cacheRefServiceMock;
    private readonly Mock<IPartialUMService> _partialUMServiceMock;
    private readonly ScoreMapService _service;

    public ScoreMapServiceTest()
    {
        _repositoryMock = new Mock<IDynamicRepository>();
        _cacheRefServiceMock = new Mock<ICacheRefService>();
        _partialUMServiceMock = new Mock<IPartialUMService>();
        _service = new ScoreMapService(_repositoryMock.Object, _cacheRefServiceMock.Object, _partialUMServiceMock.Object);
    }

    [Fact]
    public async Task GetBetTypes_ShouldReturnLiveAndDeadBetTypes()
    {
        // Arrange
        var language = "en";
        var siteName = "test";
        var betTypes = new List<ScoreMapBetTypeEntity>
            {
                new() { TypeId = 1, FirstId = "101", InfoId = "1,2", EvenType = true },
                new() { TypeId = 2, FirstId = "102", InfoId = "3,4", EvenType = false },
                new() { TypeId = BetTypeConstants._1X2, FirstId = "103", InfoId = "5,6", EvenType = true }
            };

        _partialUMServiceMock.Setup(x => x.ThrowIfUM(It.IsAny<string>(), siteName)).Returns(Task.CompletedTask);
        _repositoryMock.Setup(x => x.FetchAsync<ScoreMapBetTypeEntity>(It.IsAny<GetScoreMapBetTypeCriteria>(), string.Empty, default)).ReturnsAsync(betTypes);
        _cacheRefServiceMock.Setup(x => x.GetBetTypeName(It.IsAny<int>(), language)).Returns("BetType");

        // Act
        var result = await _service.GetBetTypes(language, siteName);

        // Assert
        Assert.Equal(2, result.Length);
        var liveBetTypes = result[0];
        var deadBetTypes = result[1];

        Assert.Equal(2, liveBetTypes.Count());
        Assert.Single(deadBetTypes);
        Assert.Contains(liveBetTypes, bt => bt.BetType == 1);
        Assert.Contains(liveBetTypes, bt => bt.BetType == BetTypeConstants._1X2);
        Assert.Contains(deadBetTypes, bt => bt.BetType == 2);
    }

    [Fact]
    public async Task GetLeagues_ShouldReturnLeaguesWithNames()
    {
        // Arrange
        var custId = 123;
        var language = "en";
        var siteName = "test";
        var leagues = new List<League>
            {
                new() { LeagueId = 1 },
                new() { LeagueId = 2 }
            };

        _partialUMServiceMock.Setup(x => x.ThrowIfUM(It.IsAny<string>(), siteName)).Returns(Task.CompletedTask);
        _repositoryMock.Setup(x => x.FetchAsync<League>(It.IsAny<GetScoreMapLeagueCriteria>(), string.Empty, default)).ReturnsAsync(leagues);
        _cacheRefServiceMock.Setup(x => x.GetLeagueName(1, language)).Returns("League 1");
        _cacheRefServiceMock.Setup(x => x.GetLeagueName(2, language)).Returns("League 2");

        // Act
        var result = await _service.GetLeagues(custId, language, siteName);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, l => l.LeagueId == 1 && l.LeagueName == "League 1");
        Assert.Contains(result, l => l.LeagueId == 2 && l.LeagueName == "League 2");
    }

    [Fact]
    public async Task GetMatches_ShouldReturnMatchesWithTeamNames()
    {
        // Arrange
        var param = new GetScoreMapMatchesParam(
            CustId: 123,
            LeagueId: 1,
            Language:"en"
        );

        var matches = new List<Match>
        {
            new() { MatchId = 1, HomeId = 10, AwayId = 20 },
            new() { MatchId = 2, HomeId = 30, AwayId = 40 }
        };

        _repositoryMock.Setup(x => x.FetchAsync<Match>(It.IsAny<GetScoreMapMatchCriteria>(), string.Empty, default)).ReturnsAsync(matches);
        _cacheRefServiceMock.Setup(x => x.GetTeamName(10, param.Language)).Returns("Home Team 1");
        _cacheRefServiceMock.Setup(x => x.GetTeamName(20, param.Language)).Returns("Away Team 1");
        _cacheRefServiceMock.Setup(x => x.GetTeamName(30, param.Language)).Returns("Home Team 2");
        _cacheRefServiceMock.Setup(x => x.GetTeamName(40, param.Language)).Returns("Away Team 2");

        // Act
        var result = await _service.GetMatches(param);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, m => m.MatchId == 1 && m.HomeName == "Home Team 1" && m.AwayName == "Away Team 1");
        Assert.Contains(result, m => m.MatchId == 2 && m.HomeName == "Home Team 2" && m.AwayName == "Away Team 2");
    }

    [Fact]
    public async Task GetResults_ShouldReturnResultsForSuperUser()
    {
        // Arrange
        var param = new GetScoreMapResultParam(
            CustLevel: 4,
            CustId: 123,
            MatchIds: "1,2",
            LiveBetTypes: "1",
            DeadBetTypes: "2",
            Language: "en"
        );

        var results = new List<ScoreMapResultEntity>
        {
            new() { MatchId = 1, Home = 10, Away = 20, HT = 1, FT = 2 }
        };

        var matches = new List<Match>
        {
            new() { MatchId = 1, HomeId = 10, AwayId = 20 },
            new() { MatchId = 2, HomeId = 30, AwayId = 40 }
        };

        _repositoryMock.Setup(x => x.FetchMultipleAsync<ScoreMapResultEntity, Match>(It.IsAny<ICriteria>(), string.Empty, default))
            .ReturnsAsync((results, matches));

        _cacheRefServiceMock.Setup(x => x.GetTeamName(10, param.Language)).Returns("Home Team 1");
        _cacheRefServiceMock.Setup(x => x.GetTeamName(20, param.Language)).Returns("Away Team 1");
        _cacheRefServiceMock.Setup(x => x.GetTeamName(30, param.Language)).Returns("Home Team 2");
        _cacheRefServiceMock.Setup(x => x.GetTeamName(40, param.Language)).Returns("Away Team 2");

        // Act
        var result = await _service.GetResults(param);

        // Assert
        Assert.Equal(2, result.Count());

        var match1 = result.First(r => r.MatchId == 1);
        Assert.Equal("Home Team 1", match1.HomeName);
        Assert.Equal("Away Team 1", match1.AwayName);

        var match2 = result.First(r => r.MatchId == 2);
        Assert.Equal("Home Team 2", match2.HomeName);
        Assert.Equal("Away Team 2", match2.AwayName);
    }

    [Fact]
    public async Task GetResults_ShouldReturnResultsForMasterUser()
    {
        // Arrange
        var param = new GetScoreMapResultParam(
            CustLevel: 3,
            CustId: 123,
            MatchIds: "1",
            LiveBetTypes: "1",
            DeadBetTypes: "2",
            Language: "en"
        );

        var results = new List<ScoreMapResultEntity>
        {
            new() { MatchId = 1, Home = 10, Away = 20, HT = 1, FT = 2 }
        };

        var matches = new List<Match>
        {
            new() { MatchId = 1, HomeId = 10, AwayId = 20 }
        };

        _repositoryMock.Setup(x => x.FetchMultipleAsync<ScoreMapResultEntity, Match>(It.IsAny<ICriteria>(), string.Empty, default))
            .ReturnsAsync((results, matches));

        _cacheRefServiceMock.Setup(x => x.GetTeamName(10, param.Language)).Returns("Home Team 1");
        _cacheRefServiceMock.Setup(x => x.GetTeamName(20, param.Language)).Returns("Away Team 1");

        // Act
        var result = await _service.GetResults(param);

        // Assert
        Assert.Single(result);
        var match1 = result.First();
        Assert.Equal("Home Team 1", match1.HomeName);
        Assert.Equal("Away Team 1", match1.AwayName);
    }

    [Fact]
    public async Task GetResults_ShouldReturnResultsForAgentUser()
    {
        // Arrange
        var param = new GetScoreMapResultParam(
            CustLevel: 2,
            CustId: 123,
            MatchIds: "1",
            LiveBetTypes: "1",
            DeadBetTypes: "2",
            Language: "en"
        );

        var results = new List<ScoreMapResultEntity>
        {
            new() { MatchId = 1, Home = 10, Away = 20, HT = 1, FT = 2 }
        };

        var matches = new List<Match>
        {
            new() { MatchId = 1, HomeId = 10, AwayId = 20 }
        };

        _repositoryMock.Setup(x => x.FetchMultipleAsync<ScoreMapResultEntity, Match>(It.IsAny<ICriteria>(), string.Empty, default))
            .ReturnsAsync((results, matches));

        _cacheRefServiceMock.Setup(x => x.GetTeamName(10, param.Language)).Returns("Home Team 1");
        _cacheRefServiceMock.Setup(x => x.GetTeamName(20, param.Language)).Returns("Away Team 1");

        // Act
        var result = await _service.GetResults(param);

        // Assert
        Assert.Single(result);
        var match1 = result.First();
        Assert.Equal("Home Team 1", match1.HomeName);
        Assert.Equal("Away Team 1", match1.AwayName);
    }
}
