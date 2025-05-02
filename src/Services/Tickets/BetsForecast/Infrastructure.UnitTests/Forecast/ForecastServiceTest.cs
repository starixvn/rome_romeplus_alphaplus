using AlphaPlus.BetsForecast.Domain.Forecast;
using AlphaPlus.BetsForecast.Infrastructure.Forecast;
using AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;
using AlphaPlus.BetsForecast.Infrastructure.Forecast.Params;
using AlphaPlus.BetsForecast.Infrastructure.Shared;
using AlphaPlus.PartialUM.Services;
using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.UnitTests.Forecast;

[Trait("Category", "BetsForecast")]
public class ForecastServiceTest
{
    private readonly Mock<IDynamicRepository> _mockRepository;
    private readonly Mock<ICacheRefService> _mockCacheRefService;
    private readonly Mock<IPartialUMService> _mockPartialUMService;
    private readonly ForecastService _forecastService;

    public ForecastServiceTest()
    {
        _mockRepository = new Mock<IDynamicRepository>();
        _mockCacheRefService = new Mock<ICacheRefService>();
        _mockPartialUMService = new Mock<IPartialUMService>();
        _forecastService = new ForecastService(
            _mockRepository.Object,
            _mockCacheRefService.Object,
            _mockPartialUMService.Object);
    }

    [Fact]
    public async Task GetRunningLeagues_ForSuperLevel_CallsCorrectCriteria()
    {
        // Arrange
        var param = new GetRunningLeaguesParam(
            CustLevel: 4,
            CustId: 123,
            BetTypes: "1,2",
            Language: "en-US",
            SiteName: "TestSite"
        );

        var leagues = new List<League>
        {
            new League { LeagueId = 1 },
            new League { LeagueId = 2 }
        };

        _mockRepository.Setup(r => r.FetchAsync<League>(It.IsAny<GetRunningLeagueSuperCriteria>(), string.Empty, default))
            .ReturnsAsync(leagues);

        _mockCacheRefService.Setup(c => c.GetLeagueName(It.IsAny<int>(), It.IsAny<string>()))
            .Returns("League Name");

        // Act
        var result = await _forecastService.GetRunningLeagues(param);

        // Assert
        _mockRepository.Verify(r => r.FetchAsync<League>(It.IsAny<GetRunningLeagueSuperCriteria>(), string.Empty, default), Times.Once);
        Assert.Equal(leagues.Count, result.Count());
        foreach (var league in result)
        {
            Assert.Equal("League Name", league.LeagueName);
        }
    }

    [Fact]
    public async Task GetRunningLeagues_ForMasterLevel_CallsCorrectCriteria()
    {
        // Arrange
        var param = new GetRunningLeaguesParam(
            CustLevel: 3,
            CustId: 123,
            BetTypes: "1,2",
            Language: "en-US",
            SiteName: "TestSite"
        );

        var leagues = new List<League>
        {
            new League { LeagueId = 1 },
            new League { LeagueId = 2 }
        };

        _mockRepository.Setup(r => r.FetchAsync<League>(It.IsAny<GetRunningLeagueMasterCriteria>(), string.Empty, default))
            .ReturnsAsync(leagues);

        // Act
        var result = await _forecastService.GetRunningLeagues(param);

        // Assert
        _mockRepository.Verify(r => r.FetchAsync<League>(It.IsAny<GetRunningLeagueMasterCriteria>(), string.Empty, default), Times.Once);
    }

    [Fact]
    public async Task GetRunningLeagues_ForAgentLevel_CallsCorrectCriteria()
    {
        // Arrange
        var param = new GetRunningLeaguesParam(
            CustLevel: 2,
            CustId: 123,
            BetTypes: "1,2",
            Language: "en-US",
            SiteName: "TestSite"
        );

        var leagues = new List<League>
        {
            new League { LeagueId = 1 },
            new League { LeagueId = 2 }
        };

        _mockRepository.Setup(r => r.FetchAsync<League>(It.IsAny<GetRunningLeagueAgentCriteria>(), string.Empty, default))
            .ReturnsAsync(leagues);

        // Act
        var result = await _forecastService.GetRunningLeagues(param);

        // Assert
        _mockRepository.Verify(r => r.FetchAsync<League>(It.IsAny<GetRunningLeagueAgentCriteria>(), string.Empty, default), Times.Once);
    }

    [Fact]
    public async Task GetHDPOULive_ForSuperLevel_CallsCorrectCriteria()
    {
        // Arrange
        var param = new GetForecastParam(
            CustLevel: 4,
            CustId: 123,
            LeagueId: 456,
            Language: "en-US"
        );

        var forecast = new List<ForecastHDPOULive>
        {
            new ForecastHDPOULive { HomeId = 1, AwayId = 2 }
        };

        _mockRepository.Setup(r => r.FetchAsync<ForecastHDPOULive>(It.IsAny<GetHDPOULiveSuperCriteria>(), string.Empty, default))
            .ReturnsAsync(forecast);

        // Act
        var result = await _forecastService.GetHDPOULive(param);

        // Assert
        _mockRepository.Verify(r => r.FetchAsync<ForecastHDPOULive>(It.IsAny<GetHDPOULiveSuperCriteria>(), string.Empty, default), Times.Once);
        Assert.Equal(forecast.Count, result.Count());
    }

    [Fact]
    public async Task GetAH_WithHandicapBetType_ReturnsCorrectData()
    {
        // Arrange
        var param = new GetAHParam(
            CustLevel: 2,
            CustId: 456,
            MatchId: 123,
            BetType: BetTypeConstants.Handicap,
            Score: 2,
            Language: "en-US"
        );

        var match = new ForecastMatch
        {
            HomeId = 1,
            HomeName = "Home",
            AwayId = 2,
            AwayName = "Away",
            LiveHomeScore = 2,
            LiveAwayScore = 1
        };

        var hdpData = new List<HDPOU>
        {
            new HDPOU { Offset = 0, AL = 50, HL = 40, MYO = 10 }
        };

        _mockRepository.Setup(r => r.GetAsync<ForecastMatch>(It.IsAny<GetMatchCriteria>(), string.Empty, default))
            .ReturnsAsync(match);

        _mockRepository.Setup(r => r.FetchAsync<HDPOU>(It.IsAny<GetHDPCriteria>(), string.Empty, default))
            .ReturnsAsync(hdpData);

        _mockCacheRefService.Setup(c => c.GetTeamName(It.IsAny<int>(), It.IsAny<string>()))
            .Returns("Team Name");

        _mockCacheRefService.Setup(c => c.GetBetTypeName(It.IsAny<int>(), It.IsAny<string>()))
            .Returns("Handicap");

        // Act
        var result = await _forecastService.GetAH(param);

        // Assert
        Assert.Equal(param.MatchId, result.MatchId);
        Assert.Equal(match.Event, result.EventName);
        Assert.Equal($"{match.LiveHomeScore} : {match.LiveAwayScore}", result.EventScore);
        Assert.True(result.HasDataMatch);
        Assert.Equal(param.BetType, result.BetType);
        Assert.Equal("Handicap", result.BetTypeName);
        Assert.Equal(param.Score, result.CurrentScore);
        Assert.True(result.HasData);
        Assert.Equal(5, result.HDPOU.Count());
    }

    [Fact]
    public async Task Get1X2_ByLeague_ReturnsCorrectData()
    {
        // Arrange
        var param = new GetForecastParam(
            CustLevel: 4,
            CustId: 123,
            LeagueId: 456,
            Language: "en-US"
        );

        var forecast = new List<Forecast1X2>
        {
            new Forecast1X2 { HomeId = 1, AwayId = 2 }
        };

        _mockRepository.Setup(r => r.FetchAsync<Forecast1X2>(It.IsAny<Get1X2ByLeagueCriteria>(), string.Empty, default))
            .ReturnsAsync(forecast);

        // Act
        var result = await _forecastService.Get1X2(param);

        // Assert
        _mockRepository.Verify(r => r.FetchAsync<Forecast1X2>(It.IsAny<Get1X2ByLeagueCriteria>(), string.Empty, default), Times.Once);
        Assert.Equal(forecast.Count, result.Count());
    }

    [Fact]
    public async Task Get1X2_ByMatch_ReturnsCorrectData()
    {
        // Arrange
        var param = new Get1X2ByMatchParam(
            CustLevel: 4,
            CustId: 123,
            MatchId: 456,
            BetType: 1,
            Language: "en-US"
        );

        var forecast = new List<Forecast1X2>
        {
            new Forecast1X2 { HomeId = 1, AwayId = 2 }
        };

        _mockRepository.Setup(r => r.FetchAsync<Forecast1X2>(It.IsAny<Get1X2ByMatchCriteria>(), string.Empty, default))
            .ReturnsAsync(forecast);

        // Act
        var result = await _forecastService.Get1X2(param);

        // Assert
        _mockRepository.Verify(r => r.FetchAsync<Forecast1X2>(It.IsAny<Get1X2ByMatchCriteria>(), string.Empty, default), Times.Once);
        Assert.Equal(forecast.Count, result.Count());
    }
}
