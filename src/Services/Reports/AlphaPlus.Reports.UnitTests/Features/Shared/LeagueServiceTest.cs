using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Models;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Reports.UnitTests.Features.Shared;

[Trait("Category", "Shared")]
public class LeagueServiceTest
{
    private readonly Mock<ICacheRefService> cacheRefServiceMock;
    private Mock<IOptions<AppSettings>> optionsMock;
    private LeagueService service;
    private AppSettings appSettings;

    public LeagueServiceTest()
    {
        cacheRefServiceMock = new Mock<ICacheRefService>();
        optionsMock = new Mock<IOptions<AppSettings>>();
        appSettings = new AppSettings { SiteLabel = "777" };

        optionsMock.Setup(x => x.Value).Returns(appSettings);

        service = new LeagueService(cacheRefServiceMock.Object, optionsMock.Object);
    }

    [Fact]
    public void GetLeagueName_ShouldCallGetLeagueName3rd_WhenLeagueTypeIsLeague3rd()
    {
        // Arrange
        const int leagueId = 123;
        const byte leagueType = 1; // Assuming LeagueType.League3rd is 1
        const int leagueGroupId = 456;
        const string expectedLeagueName = "Premier League";

        cacheRefServiceMock
            .Setup(x => x.GetLeagueName3rd(leagueId))
            .Returns(expectedLeagueName);

        // Act
        var result = service.GetLeagueName(leagueId, leagueType, leagueGroupId);

        // Assert
        Assert.Equal(expectedLeagueName, result);
        cacheRefServiceMock.Verify(x => x.GetLeagueName3rd(leagueId), Times.Once);
        cacheRefServiceMock.Verify(x => x.GetLeagueName(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        cacheRefServiceMock.Verify(x => x.GetLeagueName(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void GetLeagueName_ShouldCallGetLeagueNameWithGroupId_WhenIsSiteLabel777IsTrue()
    {
        // Arrange
        const int leagueId = 123;
        const byte leagueType = 3; // Not League3rd
        const int leagueGroupId = 456;
        const string expectedLeagueName = "Premier League";

        cacheRefServiceMock
            .Setup(x => x.GetLeagueName(leagueId, leagueGroupId))
            .Returns(expectedLeagueName);

        // Act
        var result = service.GetLeagueName(leagueId, leagueType, leagueGroupId);

        // Assert
        Assert.Equal(expectedLeagueName, result);
        cacheRefServiceMock.Verify(x => x.GetLeagueName(leagueId, leagueGroupId), Times.Once);
        cacheRefServiceMock.Verify(x => x.GetLeagueName3rd(It.IsAny<int>()), Times.Never);
        cacheRefServiceMock.Verify(x => x.GetLeagueName(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void GetLeagueName_ShouldCallGetLeagueName_WhenIsSiteLabel777IsFalse()
    {
        // Arrange
        const int leagueId = 123;
        const byte leagueType = 3; // Not League3rd
        const int leagueGroupId = 456;
        const string expectedLeagueName = "Premier League";

        optionsMock = new Mock<IOptions<AppSettings>>();
        appSettings = new AppSettings { SiteLabel = "ABC" };
        optionsMock.Setup(x => x.Value).Returns(appSettings);
        service = new LeagueService(cacheRefServiceMock.Object, optionsMock.Object);

        cacheRefServiceMock
            .Setup(x => x.GetLeagueName(leagueId))
            .Returns(expectedLeagueName);

        // Act
        var result = service.GetLeagueName(leagueId, leagueType, leagueGroupId);

        // Assert
        Assert.Equal(expectedLeagueName, result);
        cacheRefServiceMock.Verify(x => x.GetLeagueName(leagueId), Times.Once);
        cacheRefServiceMock.Verify(x => x.GetLeagueName3rd(It.IsAny<int>()), Times.Never);
        cacheRefServiceMock.Verify(x => x.GetLeagueName(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }
}