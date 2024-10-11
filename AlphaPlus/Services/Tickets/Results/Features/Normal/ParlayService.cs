using Fanex.Data.Repository;
using Fanex.Result.Core.Sportsbook.DTO;

namespace AlphaPlus.Results.Features.Normal;

public interface IParlayService
{
    Task<IList<Parlay>> GetResults(long refNo);
}

public class ParlayService(IDynamicRepository repository) : IParlayService
{
    public async Task<IList<Parlay>> GetResults(long refNo)
    {
        var (sportsbooks, gameDetails) = await repository.FetchMultipleAsync<SportsbookEntity, GameDetail>(
            new GetParlayResultCriteria(refNo)
        );

        var results = GetParlays(sportsbooks);
        AddGameDetailsToResults(gameDetails, results);

        return results;
    }

    private static List<Parlay> GetParlays(IEnumerable<SportsbookEntity> sportsbookList)
    {
        var sportType = 0;
        var leagueGroupId = 0;
        var leagueId = 0;
        var results = new List<Parlay>();
        var sportbooks = new List<Sportsbook>();

        foreach (var entity in sportsbookList)
        {
            var newSportType = entity.SportType;
            var newLeagueGroupId = entity.LeagueGroupId;
            var newLeagueId = entity.LeagueId;

            if (leagueId != newLeagueId && leagueId > 0)
            {
                results.Add(new Parlay
                {
                    SportId = sportType,
                    LeagueGroupId = leagueGroupId,
                    LeagueId = leagueId,
                    Sportbook = sportbooks
                });
                sportbooks = new List<Sportsbook>();
            }

            sportbooks.Add(BindSportbook(entity));

            sportType = newSportType;
            leagueGroupId = newLeagueGroupId;
            leagueId = newLeagueId;
        }

        results.Add(new Parlay
        {
            SportId = sportType,
            LeagueGroupId = leagueGroupId,
            LeagueId = leagueId,
            Sportbook = sportbooks
        });

        return results;
    }

    private static Sportsbook BindSportbook(SportsbookEntity entity)
        => new Sportsbook
        {
            AwayId = entity.AwayId,
            HomeId = entity.HomeId,
            BetStatus = entity.BetStatus ?? string.Empty,
            LeagueId = entity.LeagueId,
            MatchId = entity.MatchId,
            LeagueGroupId = entity.LeagueGroupId,
            EventStatus = entity.EventStatus ?? string.Empty,
            FinalAwayScore = entity.FinalAwayScore,
            HtAwayScore = entity.HtAwayScore,
            Color = entity.Lcolor ?? string.Empty,
            FinalHomeScore = entity.FinalHomeScore,
            HtHomeScore = entity.HtHomeScore,
            EventDate = entity.EventDate,
            IsNeutral = entity.IsNeutral,
            KickOffTime = entity.KickOffTime,
            MatchCode = entity.MatchCode ?? string.Empty,
            GoalRecord = entity.GoalRecord ?? string.Empty,
            IsHTCal = entity.IsHTCal,
            Calculate = entity.Calculate,
            SettlementStatus = entity.SettlementStatus,
            NoHTScore = entity.NoHTScore,
            RefNo = string.Empty,
            GameDetails = new List<GameDetail>(),
            Bettype = entity.BetType,
            BetID = entity.BetCheck,
            LiveHomeScore = entity.LiveHomeScore,
            LiveAwayScore = entity.LiveAwayScore,
            MainType = entity.MainType,
            SubType = entity.SubType,
            SessionTime = entity.SessionTime
        };

    private static void AddGameDetailsToResults(IEnumerable<GameDetail> gameDetails, IEnumerable<Parlay> results)
    {
        if (gameDetails.Any())
        {
            foreach (var result in results)
            {
                foreach (var sb in result.Sportbook)
                {
                    sb.GameDetails.AddRange(gameDetails.Where(i => i.MatchId == sb.MatchId));
                }
            }
        }
    }
}