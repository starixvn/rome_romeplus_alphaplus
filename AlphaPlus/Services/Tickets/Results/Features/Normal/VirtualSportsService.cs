using AlphaPlus.PartialUM.Services;
using AlphaPlus.Results.Features.Shared;
using Fanex.Data.Repository;
using Fanex.Result.Core.Sportsbook.DTO;

namespace AlphaPlus.Results.Features.Normal;

public interface IVirtualSportsService
{
    Task<IEnumerable<Sportsbook>> GetResults(SportsbookParam param);

    Task<IEnumerable<Sportsbook>> GetResultDetails(int matchId);
}

public class VirtualSportsService(
    IDynamicRepository repository,
    IResultsService resultsService,
    IPartialUMService partialUMService) : IVirtualSportsService
{
    public async Task<IEnumerable<Sportsbook>> GetResults(SportsbookParam param)
    {
        var moveBetsDay = await resultsService.GetMoveBetsDate();
        var criteria = new GetVirtuallySportResultCriteria(
            param.FromDate, param.SportType, param.LeagueId, param.IsShowAll, moveBetsDay);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var (matchs, details) = await repository.FetchMultipleAsync<Sportsbook, GameDetail>(criteria);
        foreach (var match in matchs)
        {
            match.GameDetails = details.Where(i => i.MatchId == match.MatchId).ToList();
        }

        return matchs;
    }

    public async Task<IEnumerable<Sportsbook>> GetResultDetails(int matchId)
    {
        var criteria = new GetVirtuallySportResultDetailsCriteria(matchId);
        var (matchs, details) = await repository.FetchMultipleAsync<Sportsbook, GameDetail>(criteria);        

        foreach(var match in matchs)
        {
            match.GameDetails = details.Select(gd =>
            {
                gd.MatchId = matchId;
                return gd;
            }).ToList();
        }

        return matchs;
    }
}