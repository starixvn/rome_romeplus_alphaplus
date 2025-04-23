namespace AlphaPlus.Reports.Features.BetChoiceMatching;

public class BetChoiceMatchingEventModel
{
    public BetChoiceMatchingEventModel(IEnumerable<BetChoiceMatchingUserModel> users)
    {
        var firstTicket = users.First().Tickets.First();
        LeagueId = firstTicket.LeagueId;
        LeagueGroupId = firstTicket.LeagueGroupId;
        SportType = firstTicket.SportType;
        BetType = firstTicket.BetType;
        GlobalShowTime = firstTicket.GlobalShowTime;
        Hdp1 = firstTicket.Hdp1;
        Hdp2 = firstTicket.Hdp2;
        BetTeam = firstTicket.BetTeam;
        IsLive = firstTicket.IsLive;
        LiveHomeScore = firstTicket.LiveHomeScore;
        LiveAwayScore = firstTicket.LiveAwayScore;
        HomeId = firstTicket.HomeId;
        AwayId = firstTicket.AwayId;
        IsRunning = firstTicket.IsRunning;
        Users = users;
    }

    public int LeagueId { get; }

    public int LeagueGroupId { get; }

    public int SportType { get; }

    public int BetType { get; }

    public decimal Hdp1 { get; }

    public decimal Hdp2 { get; }

    public int HomeId { get; }

    public int AwayId { get; }

    public string BetTeam { get; }

    public DateTime GlobalShowTime { get; }

    public bool IsLive { get; }

    public int LiveHomeScore { get; }

    public int LiveAwayScore { get; }

    public bool IsRunning { get; }

    public IEnumerable<BetChoiceMatchingUserModel> Users { get; }
}
