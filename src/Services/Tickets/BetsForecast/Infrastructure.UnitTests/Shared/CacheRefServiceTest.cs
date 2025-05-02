using AlphaPlus.BetsForecast.Infrastructure.Shared;
using Fanex.RemotingRef;

namespace AlphaPlus.BetsForecast.Infrastructure.UnitTests.Shared;

[Trait("Category", "BetsForecast")]
public class CacheRefServiceTest
{
    private readonly Mock<IRemotingRefService> remotingRefServiceMock;
    private readonly CacheRefService service;

    public CacheRefServiceTest()
    {
        remotingRefServiceMock = new Mock<IRemotingRefService>();
        service = new CacheRefService(remotingRefServiceMock.Object);
    }

    [Fact]
    public void GetLeagueName_ShouldCallRemotingRefService()
    {
        // Arrange
        const int leagueId = 123;
        const string language = "en-US";
        const string expectedLeagueName = "Premier League";

        remotingRefServiceMock
            .Setup(x => x.GetLeagueName(leagueId.ToString(), language))
            .Returns(expectedLeagueName);

        // Act
        var result = service.GetLeagueName(leagueId, language);

        // Assert
        Assert.Equal(expectedLeagueName, result);
        remotingRefServiceMock.Verify(x => x.GetLeagueName(leagueId.ToString(), language), Times.Once);
    }

    [Fact]
    public void GetTeamName_ShouldCallRemotingRefService()
    {
        // Arrange
        const int teamId = 456;
        const string language = "en-US";
        const string expectedTeamName = "Manchester United";

        remotingRefServiceMock
            .Setup(x => x.GetTeamName(teamId.ToString(), language))
            .Returns(expectedTeamName);

        // Act
        var result = service.GetTeamName(teamId, language);

        // Assert
        Assert.Equal(expectedTeamName, result);
        remotingRefServiceMock.Verify(x => x.GetTeamName(teamId.ToString(), language), Times.Once);
    }

    [Fact]
    public void GetBetTypeName_WithIntParameter_ShouldCallRemotingRefService()
    {
        // Arrange
        const int betType = 789;
        const string language = "en-US";
        const string expectedBetTypeName = "Money Line";

        remotingRefServiceMock
            .Setup(x => x.GetBetTypeName(betType.ToString(), language))
            .Returns(expectedBetTypeName);

        // Act
        var result = service.GetBetTypeName(betType, language);

        // Assert
        Assert.Equal(expectedBetTypeName, result);
        remotingRefServiceMock.Verify(x => x.GetBetTypeName(betType.ToString(), language), Times.Once);
    }

    [Fact]
    public void GetBetTypeName_WithStringParameter_ShouldCallRemotingRefService()
    {
        // Arrange
        const string betType = "789";
        const string language = "en-US";
        const string expectedBetTypeName = "Money Line";

        remotingRefServiceMock
            .Setup(x => x.GetBetTypeName(betType, language))
            .Returns(expectedBetTypeName);

        // Act
        var result = service.GetBetTypeName(betType, language);

        // Assert
        Assert.Equal(expectedBetTypeName, result);
        remotingRefServiceMock.Verify(x => x.GetBetTypeName(betType, language), Times.Once);
    }
}