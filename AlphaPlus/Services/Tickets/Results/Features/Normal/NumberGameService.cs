using Fanex.Data.Repository;
using Fanex.Result.Core.NumberGame.DTO;

namespace AlphaPlus.Results.Features.Normal;

public interface INumberGameService
{
    Task<IEnumerable<NumberGame>> GetResults(NumberGameParam param);

    Task<IEnumerable<NumberGame>> GetResultDetails(int matchId);
}

public class NumberGameService(IDynamicRepository repository) : INumberGameService
{
    public Task<IEnumerable<NumberGame>> GetResults(NumberGameParam param)
        => BuildNumberGameResults(new GetNumberGameResultCriteria(
            param.FromDate, param.SportType, param.LeagueId, param.Language, param.IsShowAll));

    public Task<IEnumerable<NumberGame>> GetResultDetails(int matchId)
        => BuildNumberGameResults(new GetNumberGameResultDetailsCriteria(matchId));

    private async Task<IEnumerable<NumberGame>> BuildNumberGameResults(ICriteria criteria)
    {
        var results = new List<NumberGame>();
        var numberGames = await repository.FetchAsync<NumberGameEntity>(criteria);

        foreach (var entity in numberGames)
        {
            BindNumberGame(results, entity);
        }

        return results;
    }

    private static void BindNumberGame(IList<NumberGame> results, NumberGameEntity entity)
    {
        var existMatch = results.SingleOrDefault(i => i.MatchId == entity.MatchId);

        if (existMatch != null)
        {
            existMatch.Seg.Add(entity.Seg);
            existMatch.Ball.Add(entity.Number);
        }
        else
        {
            results.Add(new NumberGame
            {
                LeagueId = entity.LeagueId,
                MatchId = entity.MatchId,
                EventStatus = entity.EventStatus,
                EventDate = entity.EventDate,
                KickOffTime = entity.KickOffTime,
                MatchCode = entity.MatchCode,
                Seg = new List<int> { entity.Seg },
                Ball = new List<int> { entity.Number }
            });
        }
    }
}