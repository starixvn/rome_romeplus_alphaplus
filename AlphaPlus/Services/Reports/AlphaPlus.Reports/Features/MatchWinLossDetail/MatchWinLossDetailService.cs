using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Models;
using Fanex.Caching;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.MatchWinLossDetail;

public interface IMatchWinLossDetailService
{
    Task<IEnumerable<int>> GetProducts();

    Task<IEnumerable<League>> GetLeagues(GetLeaguesParam param);

    Task<ReportData<MatchWinLossDetailModel>> GetMatchWinLossDetail(MatchWinLossDetailParam param);
}

public class MatchWinLossDetailService(
    IDynamicRepository repository,
    ICacheService cacheService,
    ILeagueService leagueService,
    IReportDateService reportDateService,
    ICacheRefService cacheRefService,
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

    public async Task<IEnumerable<League>> GetLeagues(GetLeaguesParam param)
    {
        var moveBetsDay = await reportDateService.GetMoveBetsDay();
        var leagues = await repository.FetchAsync<League>(new GetLeaguesCriteria(
            param.CustId,
            param.ProductIds,
            param.FromDate,
            param.ToDate,
            moveBetsDay,
            param.Language,
            param.IsShowAll));

        foreach (var item in leagues)
        {
            item.Name = leagueService.GetLeagueName(item.LeagueId, item.LeagueType, item.LeagueGroupId);
        }

        return leagues;
    }

    public async Task<ReportData<MatchWinLossDetailModel>> GetMatchWinLossDetail(MatchWinLossDetailParam param)
    {
        var moveBetsDay = await reportDateService.GetMoveBetsDay();
        var criteria = new MatchWinLossDetailCriteria(
            param.ViewerId,
            param.FromDate,
            param.ToDate,
            moveBetsDay,
            param.ProductIds,
            param.LeagueIds,
            param.ViewerLevel);

        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var total = new MatchWinLossDetailModel();
        var rawdata = await repository.FetchAsync<MatchWinLossDetailModel>(criteria);

        BetTypeWinLossModel mixParlay = null;
        BetTypeWinLossModel systemParlay = null;
        var parlayMatchWinLossDetail = new List<BetTypeWinLossModel>();

        foreach (var item in rawdata)
        {
            if (item.IsParlay)
            {
                if (item.BetType == BetTypeConstants.MixParlay || item.BetType == BetTypeConstants.MixParlayBetRadarVirtualSports)
                {
                    mixParlay = new BetTypeWinLossModel(item, cacheRefService.GetBetTypeName(item.BetType));
                }

                if (item.BetType == BetTypeConstants.CombineMixParlay)
                {
                    systemParlay = new BetTypeWinLossModel(item, cacheRefService.GetBetTypeName(item.BetType));
                }

                if (IsParlayMatchWinLossDetail(item.BetType))
                {
                    parlayMatchWinLossDetail.Add(new BetTypeWinLossModel(item, cacheRefService.GetBetTypeName(item.BetType)));
                }
            }

            total += item;
        }

        return new ReportData<MatchWinLossDetailModel>(rawdata, total);
    }

    private static bool IsParlayMatchWinLossDetail(int betType)
        => (BetTypeConstants.LeapGamingMin <= betType && betType <= BetTypeConstants.LeapGamingMax) ||
            (BetTypeConstants.Happy5LinkedGamesMin <= betType && betType <= BetTypeConstants.Happy5LinkedGamesMax);
}
