using AlphaPlus.Results.Features.Shared;
using Fanex.Data.Repository;
using Fanex.Result.Core.Bitcoin.DTO;

namespace AlphaPlus.Results.Features.Normal;

public interface IBitcoinService
{
    Task<IEnumerable<Bitcoin>> GetResults(DateTime fromDate, int leagueId);

    Task<IEnumerable<Bitcoin>> GetResultDetails(int matchId);
}

public class BitcoinService(IDynamicRepository repository, IResultsService resultsService) : IBitcoinService
{
    public async Task<IEnumerable<Bitcoin>> GetResults(DateTime fromDate, int leagueId)
    {
        var moveBetsDate = await resultsService.GetMoveBetsDate();
        var (bitcoins, gameDetails) = await repository.FetchMultipleAsync<Bitcoin, GameDetail>(
            new GetBitcoinResultCriteria(fromDate, leagueId, moveBetsDate));

        return BuildBitcoins(bitcoins, gameDetails);
    }

    public async Task<IEnumerable<Bitcoin>> GetResultDetails(int matchId)
    {
        var (bitcoins, gameDetails) = await repository.FetchMultipleAsync<Bitcoin, GameDetail>(
            new GetBitcoinResultDetailsCriteria(matchId));

        return BuildBitcoins(bitcoins, gameDetails);
    }

    private static IEnumerable<Bitcoin> BuildBitcoins(IEnumerable<Bitcoin> bitcoins, IEnumerable<GameDetail> gameDetails)
    {
        foreach (var item in bitcoins)
        {
            var gameDetailList = gameDetails.Where(i => i.Match3rdId == item.Match3rdId);

            if (gameDetailList != null)
            {
                item.GameResults = gameDetailList.ToList();
            }
        }

        return bitcoins;
    }
}