namespace AlphaPlus.Results.Features.Normal;

using AlphaPlus.Results.Features.Shared;
using Fanex.Data.Repository;
using Fanex.Result.Core.VirtualRacing.DTO;

public interface IVirtualRacing3rdService
{
    Task<IEnumerable<VirtualRacing>> GetResults(VirtualRacingParam param);

    Task<IEnumerable<VirtualRacing>> GetResultDetails(VirtualRacingDetailParam param);
}

public class VirtualRacing3rdService(IDynamicRepository repository, IResultsService resultsService) : IVirtualRacing3rdService
{
    public async Task<IEnumerable<VirtualRacing>> GetResults(VirtualRacingParam param)
    {
        var moveBetsDate = await resultsService.GetMoveBetsDate();
        var criteria = new GetVirtualRacing3rdResultCriteria(
            param.FromDate, param.SportType, param.LeagueId, moveBetsDate);

        return await FetchAndProcessResults(criteria);
    }

    public async Task<IEnumerable<VirtualRacing>> GetResultDetails(VirtualRacingDetailParam param)
    {
        var moveBetsDate = await resultsService.GetMoveBetsDate();
        var balanceUpDay = await resultsService.GetBalanceUpDate();

        return await FetchAndProcessResults(new GetVirtualRacing3rdResultDetailsCriteria(param.eventId, param.eventDate, moveBetsDate, balanceUpDay));
    }
        

    private async Task<IEnumerable<VirtualRacing>> FetchAndProcessResults(ICriteria criteria)
    {
        var rawData = await repository.FetchMultipleAsync<VirtualRacing, VirtualRacingDetail>(criteria);
        var results = rawData.Item1 ?? new List<VirtualRacing>();
        var gameDetails = rawData.Item2 ?? new List<VirtualRacingDetail>();

        AddGameDetailsToResults(gameDetails, results);

        return results;
    }

    private static void AddGameDetailsToResults(IEnumerable<VirtualRacingDetail> gameDetails, IEnumerable<VirtualRacing> results)
    {
        foreach (var result in results)
        {
            result.Details.AddRange(gameDetails.Where(i => i.EventId == result.EventId));
        }
    }
}