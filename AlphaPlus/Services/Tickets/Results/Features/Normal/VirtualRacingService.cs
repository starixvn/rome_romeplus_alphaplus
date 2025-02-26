using AlphaPlus.Results.Features.Shared;
using Fanex.Data.Repository;
using Fanex.Result.Core.Racing.DTO;

namespace AlphaPlus.Results.Features.Normal;

public interface IVirtualRacingService
{
    Task<IEnumerable<Racing>> GetResults(VirtualRacingParam param);

    Task<IEnumerable<Racing>> GetResultDetails(int matchId);
}

public class VirtualRacingService(IDynamicRepository repository, IResultsService resultsService) : IVirtualRacingService
{
    public async Task<IEnumerable<Racing>> GetResults(VirtualRacingParam param)
    {
        var moveBetsDate = await resultsService.GetMoveBetsDate();

        return await BuildVirtualRacing(new GetVirtualRacingResultCriteria(
            param.FromDate, param.SportType, param.LeagueId, param.IsShowAll, moveBetsDate));
    }

    public Task<IEnumerable<Racing>> GetResultDetails(int matchId)
        => BuildVirtualRacing(new GetRacingResultDetailsCriteria(matchId));

    private async Task<IEnumerable<Racing>> BuildVirtualRacing(ICriteria criteria)
    {
        var racingList = await repository.FetchAsync<Racing>(criteria);

        return racingList.Select(item =>
        {
            item.SCROfMatch = GetSRCValue(item.SCROfMatch);
            return item;
        });
    }

    private static string GetSRCValue(string src)
        => !string.IsNullOrWhiteSpace(src) && src.EndsWith(",", StringComparison.Ordinal)
        ? src.Remove(src.Length - 1)
        : src;
}