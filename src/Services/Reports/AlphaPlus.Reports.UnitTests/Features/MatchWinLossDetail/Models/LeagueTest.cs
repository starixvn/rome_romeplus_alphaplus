using AlphaPlus.Reports.Features.MatchWinLossDetail;

namespace AlphaPlus.Reports.UnitTests.Features.MatchWinLossDetail;

[Trait("Category", "MatchWinLossDetail")]
public class LeagueTest
{
    [Fact]
    public void League_Constructor_InitializesProperties()
    {
        // Act
        var league = new League();

        // Assert
        Assert.Equal(0, league.LeagueId);
        Assert.Equal(0, league.LeagueGroupId);
        Assert.Equal(0, league.SportTypeId);
        Assert.Equal(0, league.LeagueType);
        Assert.Null(league.Name);
    }

    [Fact]
    public void Id_ShouldCombine_LeagueIdAndLeagueType()
    {
        // Arrange
        var league = new League
        {
            LeagueId = 123,
            LeagueType = 2
        };

        // Act
        var id = league.Id;

        // Assert
        Assert.Equal("123_2", id);
    }

    [Fact]
    public void Name_Property_CanBeSetAndRetrieved()
    {
        // Arrange
        var league = new League();
        var expectedName = "Premier League";

        // Act
        league.Name = expectedName;

        // Assert
        Assert.Equal(expectedName, league.Name);
    }

    [Theory]
    [InlineData(1, 2, 3, 4, "Soccer")]
    [InlineData(100, 200, 300, 1, "Basketball")]
    [InlineData(0, 0, 0, 0, null)]
    public void League_InitProperties_ShouldWork(int leagueId, int leagueGroupId, int sportTypeId, byte leagueType, string name)
    {
        // Arrange & Act
        var league = new League
        {
            LeagueId = leagueId,
            LeagueGroupId = leagueGroupId,
            SportTypeId = sportTypeId,
            LeagueType = leagueType,
            Name = name
        };

        // Assert
        Assert.Equal(leagueId, league.LeagueId);
        Assert.Equal(leagueGroupId, league.LeagueGroupId);
        Assert.Equal(sportTypeId, league.SportTypeId);
        Assert.Equal(leagueType, league.LeagueType);
        Assert.Equal(name, league.Name);
        Assert.Equal($"{leagueId}_{leagueType}", league.Id);
    }
}