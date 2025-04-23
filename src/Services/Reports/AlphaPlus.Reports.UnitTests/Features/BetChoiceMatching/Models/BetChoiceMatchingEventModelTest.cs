using AlphaPlus.Reports.Features.BetChoiceMatching;

namespace AlphaPlus.Reports.UnitTests.Features.BetChoiceMatching;

[Trait("Category", "BetChoiceMatching")]
public class BetChoiceMatchingEventModelTest
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var tickets = new List<BetChoiceMatchingTicketModel>
        {
            new BetChoiceMatchingTicketModel
            {
                LeagueId = 1,
                LeagueGroupId = 2,
                SportType = 3,
                BetType = 4,
                GlobalShowTime = DateTime.Now,
                Hdp1 = 1.5m,
                Hdp2 = 2.5m,
                BetTeam = "TeamA",
                IsLive = true,
                LiveHomeScore = 1,
                LiveAwayScore = 2,
                HomeId = 10,
                AwayId = 20,
                IsRunning = true
            }
        };
        var users = new List<BetChoiceMatchingUserModel>
        {
            new BetChoiceMatchingUserModel(tickets)
        };

        // Act
        var model = new BetChoiceMatchingEventModel(users);

        // Assert
        var firstTicket = tickets.First();
        Assert.Equal(firstTicket.LeagueId, model.LeagueId);
        Assert.Equal(firstTicket.LeagueGroupId, model.LeagueGroupId);
        Assert.Equal(firstTicket.SportType, model.SportType);
        Assert.Equal(firstTicket.BetType, model.BetType);
        Assert.Equal(firstTicket.GlobalShowTime, model.GlobalShowTime);
        Assert.Equal(firstTicket.Hdp1, model.Hdp1);
        Assert.Equal(firstTicket.Hdp2, model.Hdp2);
        Assert.Equal(firstTicket.BetTeam, model.BetTeam);
        Assert.Equal(firstTicket.IsLive, model.IsLive);
        Assert.Equal(firstTicket.LiveHomeScore, model.LiveHomeScore);
        Assert.Equal(firstTicket.LiveAwayScore, model.LiveAwayScore);
        Assert.Equal(firstTicket.HomeId, model.HomeId);
        Assert.Equal(firstTicket.AwayId, model.AwayId);
        Assert.Equal(firstTicket.IsRunning, model.IsRunning);
        Assert.Equal(users, model.Users);
    }
}
