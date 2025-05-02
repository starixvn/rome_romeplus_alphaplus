using AlphaPlus.BetsForecast.Domain.TotalBets;
using AlphaPlus.BetsForecast.Infrastructure.Shared;
using AlphaPlus.BetsForecast.Infrastructure.TotalBets;
using AlphaPlus.BetsForecast.Infrastructure.TotalBets.Criteria;
using AlphaPlus.BetsForecast.Infrastructure.TotalBets.Params;
using AlphaPlus.PartialUM.Services;
using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.UnitTests.TotalBets;

[Trait("Category", "BetsForecast")]
public class TotalBetsServiceTest
{
    private readonly Mock<IDynamicRepository> _repositoryMock;
    private readonly Mock<ICacheRefService> _cacheRefServiceMock;
    private readonly Mock<IPartialUMService> _partialUMServiceMock;
    private readonly TotalBetsService _service;
    private readonly GetTotalBetsParam _defaultParam;

    public TotalBetsServiceTest()
    {
        _repositoryMock = new Mock<IDynamicRepository>();
        _cacheRefServiceMock = new Mock<ICacheRefService>();
        _partialUMServiceMock = new Mock<IPartialUMService>();

        _service = new TotalBetsService(
            _repositoryMock.Object,
            _cacheRefServiceMock.Object,
            _partialUMServiceMock.Object);

        _defaultParam = new GetTotalBetsParam(
            CustLevel: 4,
            CustId: 123,
            Language: "en-US",
            SiteName: "TestSite"
        );
    }

    [Fact]
    public async Task GetHDPOULive_Should_Return_Data_With_Names_Populated()
    {
        // Arrange
        var testData = new List<HDPOULive>
        {
            new HDPOULive { LeagueId = 1, HomeId = 10, AwayId = 20 }
        };

        SetupCommonMocks(testData);

        // Act
        var result = await _service.GetHDPOULive(_defaultParam);

        // Assert
        Assert.Single(result);
        VerifyCommonInteractions<HDPOULive>();
    }

    [Fact]
    public async Task GetMoneyLine_Should_Return_Data_With_Names_Populated()
    {
        // Arrange
        var testData = new List<MoneyLine>
        {
            new MoneyLine { LeagueId = 1, HomeId = 10, AwayId = 20 }
        };

        SetupCommonMocks(testData);

        // Act
        var result = await _service.GetMoneyLine(_defaultParam);

        // Assert
        Assert.Single(result);
        VerifyCommonInteractions<MoneyLine>();
    }

    [Fact]
    public async Task GetOE1X2DND_Should_Return_Data_With_Names_Populated()
    {
        // Arrange
        var testData = new List<OE1X2DND>
        {
            new OE1X2DND { LeagueId = 1, HomeId = 10, AwayId = 20 }
        };

        SetupCommonMocks(testData);

        // Act
        var result = await _service.GetOE1X2DND(_defaultParam);

        // Assert
        Assert.Single(result);
        VerifyCommonInteractions<OE1X2DND>();
    }

    [Fact]
    public async Task GetTotalGoal_Should_Return_Data_With_Names_Populated()
    {
        // Arrange
        var testData = new List<TotalGoal>
        {
            new TotalGoal { LeagueId = 1, HomeId = 10, AwayId = 20 }
        };

        SetupCommonMocks(testData);

        // Act
        var result = await _service.GetTotalGoal(_defaultParam);

        // Assert
        Assert.Single(result);
        VerifyCommonInteractions<TotalGoal>();
    }

    [Fact]
    public async Task GetOutright_Should_Return_Data_With_Names_Populated()
    {
        // Arrange
        var testData = new List<Outright>
        {
            new Outright { LeagueId = 1, TeamId = 10 }
        };

        _repositoryMock
            .Setup(r => r.FetchAsync<Outright>(It.IsAny<GetOutrightCriteria>(), string.Empty, default))
            .ReturnsAsync(testData);

        _cacheRefServiceMock
            .Setup(c => c.GetLeagueName(1, _defaultParam.Language))
            .Returns("Test League");

        _cacheRefServiceMock
            .Setup(c => c.GetTeamName(10, _defaultParam.Language))
            .Returns("Test Team");

        // Act
        var result = await _service.GetOutright(_defaultParam);

        // Assert
        Assert.Single(result);
        var item = result.First();
        Assert.Equal("Test League", item.LeagueName);
        Assert.Equal("Test Team", item.TeamName);

        _partialUMServiceMock.Verify(
            p => p.ThrowIfUM(It.IsAny<string>(), _defaultParam.SiteName),
            Times.Once);
    }

    [Fact]
    public async Task GetHalfTimeFullTime_Should_Return_Data_With_Names_Populated()
    {
        // Arrange
        var testData = new List<HalfTimeFullTime>
        {
            new HalfTimeFullTime { LeagueId = 1, HomeId = 10, AwayId = 20 }
        };

        SetupCommonMocks(testData);

        // Act
        var result = await _service.GetHalfTimeFullTime(_defaultParam);

        // Assert
        Assert.Single(result);
        VerifyCommonInteractions<HalfTimeFullTime>();
    }

    [Fact]
    public async Task GetFirstGoalLastGoal_Should_Return_Data_With_Names_Populated()
    {
        // Arrange
        var testData = new List<FirstGoalLastGoal>
        {
            new FirstGoalLastGoal { LeagueId = 1, HomeId = 10, AwayId = 20 }
        };

        SetupCommonMocks(testData);

        // Act
        var result = await _service.GetFirstGoalLastGoal(_defaultParam);

        // Assert
        Assert.Single(result);
        VerifyCommonInteractions<FirstGoalLastGoal>();
    }

    [Fact]
    public async Task GetHomeDrawAwayNoBet_Should_Return_Data_With_Names_Populated()
    {
        // Arrange
        var testData = new List<HomeDrawAwayNoBet>
        {
            new HomeDrawAwayNoBet { LeagueId = 1, HomeId = 10, AwayId = 20 }
        };

        SetupCommonMocks(testData);

        // Act
        var result = await _service.GetHomeDrawAwayNoBet(_defaultParam);

        // Assert
        Assert.Single(result);
        VerifyCommonInteractions<HomeDrawAwayNoBet>();
    }

    [Fact]
    public async Task GetNumberGame_Should_Return_Data_With_League_Name_Populated()
    {
        // Arrange
        var testData = new List<NumberGame>
        {
            new NumberGame { LeagueId = 1 }
        };

        _repositoryMock
            .Setup(r => r.FetchAsync<NumberGame>(It.IsAny<GetNumberGameCriteria>(), string.Empty, default))
            .ReturnsAsync(testData);

        _cacheRefServiceMock
            .Setup(c => c.GetLeagueName(1, _defaultParam.Language))
            .Returns("Test League");

        // Act
        var result = await _service.GetNumberGame(_defaultParam);

        // Assert
        Assert.Single(result);
        var item = result.First();
        Assert.Equal("Test League", item.LeagueName);

        _partialUMServiceMock.Verify(
            p => p.ThrowIfUM(It.IsAny<string>(), _defaultParam.SiteName),
            Times.Once);
    }

    [Fact]
    public async Task GetTotalBets_Should_Return_Null_LeagueName_When_LeagueId_Is_Zero()
    {
        // Arrange
        var testData = new List<HDPOULive>
            {
                new HDPOULive { LeagueId = 0, HomeId = 10, AwayId = 20 }
            };

        _repositoryMock
            .Setup(r => r.FetchAsync<HDPOULive>(It.IsAny<ICriteria>(), string.Empty, default))
            .ReturnsAsync(testData);

        _cacheRefServiceMock
            .Setup(c => c.GetTeamName(10, _defaultParam.Language))
            .Returns("Home Team");

        _cacheRefServiceMock
            .Setup(c => c.GetTeamName(20, _defaultParam.Language))
            .Returns("Away Team");

        // Act
        var result = await _service.GetHDPOULive(_defaultParam);

        // Assert
        Assert.Single(result);
        var item = result.First();
        Assert.Null(item.LeagueName);
        Assert.Equal("Home Team", item.HomeName);
        Assert.Equal("Away Team", item.AwayName);
    }

    private void SetupCommonMocks<T>(List<T> testData) where T : TotalBetsMatch
    {
        _repositoryMock
            .Setup(r => r.FetchAsync<T>(It.IsAny<ICriteria>(), string.Empty, default))
            .ReturnsAsync(testData);

        _cacheRefServiceMock
            .Setup(c => c.GetLeagueName(1, _defaultParam.Language))
            .Returns("Test League");

        _cacheRefServiceMock
            .Setup(c => c.GetTeamName(10, _defaultParam.Language))
            .Returns("Home Team");

        _cacheRefServiceMock
            .Setup(c => c.GetTeamName(20, _defaultParam.Language))
            .Returns("Away Team");
    }

    private void VerifyCommonInteractions<T>() where T : TotalBetsMatch
    {
        _partialUMServiceMock.Verify(
            p => p.ThrowIfUM(It.IsAny<string>(), _defaultParam.SiteName),
            Times.Once);

        _repositoryMock.Verify(
            r => r.FetchAsync<T>(It.IsAny<ICriteria>(), string.Empty, default),
            Times.Once);

        _cacheRefServiceMock.Verify(
            c => c.GetLeagueName(1, _defaultParam.Language),
            Times.Once);

        _cacheRefServiceMock.Verify(
            c => c.GetTeamName(10, _defaultParam.Language),
            Times.Once);

        _cacheRefServiceMock.Verify(
            c => c.GetTeamName(20, _defaultParam.Language),
            Times.Once);
    }
}
