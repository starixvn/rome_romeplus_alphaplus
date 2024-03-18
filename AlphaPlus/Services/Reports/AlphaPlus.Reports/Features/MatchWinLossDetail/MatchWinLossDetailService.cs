using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Constants;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Shared.Models;
using Fanex.Caching;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.MatchWinLossDetail;

public interface IMatchWinLossDetailService
{
    Task<IEnumerable<int>> GetProducts();

    Task<IEnumerable<MatchWinLossDetailModel>> GetMatchWinLossDetail(MatchWinLossDetailParam param);
}

public class MatchWinLossDetailService(
    IDynamicRepository repository,
    ICacheService cacheService,
    IReportDateService reportDateService,
    IPartialUMService partialUMService) : IMatchWinLossDetailService
{
    private const int CacheInMinutes = 5;
    private const string CacheKey = "AlphaPlus.Reports.MatchWinLossDetail.Products";

    public Task<IEnumerable<int>> GetProducts()
        => cacheService.GetOrSetAsync(
        CacheKey,
        async () =>
        {
            var products = await repository.FetchAsync<Product>(new ProductsForMatchWinLossDetailCriteria());
            return products.Select(p => p.Id);
        },
        new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CacheInMinutes)));

    public async Task<IEnumerable<MatchWinLossDetailModel>> GetMatchWinLossDetail(MatchWinLossDetailParam param)
    {
        var moveBetsDay = await reportDateService.GetMoveBetsDay();
        var criteria = new MatchWinLossDetailCriteria(
            param.ViewerId,
            param.FromDate,
            param.ToDate,
            moveBetsDay,
            param.ProductIds,
            BuildLeagueIds(param.Leagues).JoinWithComma(),
            param.ViewerLevel);

        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        return await repository.FetchAsync<MatchWinLossDetailModel>(criteria);
    }

    private static IEnumerable<int> BuildLeagueIds(IEnumerable<string> leagues)
    {
        const int ValidLeagueArrayLength = 2;

        foreach (var item in leagues)
        {
            var leagueArray = item.Split('_');
            if (leagueArray.Length == ValidLeagueArrayLength)
            {
                if (int.TryParse(leagueArray[0], out var leagueId)
                && int.TryParse(leagueArray[1], out var leagueType)
                && leagueType == LeagueType.Self)
                {
                    yield return leagueId;
                }
            }
        }
    }
}
