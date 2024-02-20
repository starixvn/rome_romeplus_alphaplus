using AlphaPlus.Results.Criteria.Products;
using AlphaPlus.Results.Models.Entities;
using Fanex.Data.Repository;
using Fanex.Result.Core.Sportsbook.DTO;

namespace AlphaPlus.Results.DataAccesses;

public interface IParlayResultDataAccess
{
    Task<IList<Parlay>> GetParlayResults(GetParlayResultCriteria criteria);
}

public class ParlayResultDataAccess : IParlayResultDataAccess
{
    private readonly IDynamicRepository repository;

    public ParlayResultDataAccess(IDynamicRepository repository)
        => this.repository = repository;

    public async Task<IList<Parlay>> GetParlayResults(GetParlayResultCriteria criteria)
    {
        var (sportsbooks, gameDetails) = await repository.FetchMultipleAsync<SportsbookEntity, GameDetail>(criteria);

        var results = GetParlays(sportsbooks);
        AddGameDetailsToResults(gameDetails, results);

        return results;
    }

    private static List<Parlay> GetParlays(IEnumerable<SportsbookEntity> sportsbookList)
    {
        var results = new List<Parlay>();
        var leagueId = 0;
        var sportId = 0;
        var sportbooks = new List<Sportsbook>();

        foreach (var entity in sportsbookList)
        {
            var newLeagueId = entity.LeagueId;
            var newsportId = entity.SportType;

            if (leagueId != newLeagueId && leagueId > 0)
            {
                results.Add(new Parlay
                {
                    SportId = sportId,
                    LeagueId = leagueId,
                    Sportbook = sportbooks,
                });
                sportbooks = new List<Sportsbook>();
            }

            sportbooks.Add(BindSportbook(entity));

            leagueId = newLeagueId;
            sportId = newsportId;
        }

        results.Add(new Parlay
        {
            SportId = sportId,
            LeagueId = leagueId,
            Sportbook = sportbooks
        });

        return results;
    }

    private static Sportsbook BindSportbook(SportsbookEntity entity)
    {
        return new Sportsbook
        {
            AwayId = entity.AwayId,
            HomeId = entity.HomeId,
            BetStatus = entity.BetStatus ?? string.Empty,
            LeagueId = entity.LeagueId,
            MatchId = entity.MatchId,
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
            Bettype = entity.Bettype,
            BetID = entity.BetCheck,
            LiveHomeScore = entity.LiveHomeScore,
            LiveAwayScore = entity.LiveAwayScore
        };
    }

    private static void AddGameDetailsToResults(IEnumerable<GameDetail> gameDetails, List<Parlay> results)
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
