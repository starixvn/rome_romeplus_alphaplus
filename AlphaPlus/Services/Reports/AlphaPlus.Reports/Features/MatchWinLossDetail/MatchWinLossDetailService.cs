using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.Products;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Constants;
using Fanex.Caching;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.MatchWinLossDetail;

public interface IMatchWinLossDetailService
{
    Task<int[]> GetProducts();

    Task<IEnumerable<League>> GetLeagues(GetLeaguesParam param);

    Task<MatchWinLossDetailReport> GetMatchWinLossDetail(MatchWinLossDetailParam param);
}

public class MatchWinLossDetailService(
    IDynamicRepository repository,
    ICacheService cacheService,
    ILeagueService leagueService,
    IReportDateService reportDateService,
    ICacheRefService cacheRefService,
    IPartialUMService partialUMService) : IMatchWinLossDetailService
{
    private const int ProductsCacheInMinutes = 5;
    private const string ProductsCacheKey = "AlphaPlus.Reports.MatchWinLossDetail.Products";

    private readonly IDynamicRepository repository = repository;
    private readonly ICacheService cacheService = cacheService;
    private readonly ILeagueService leagueService = leagueService;
    private readonly IReportDateService reportDateService = reportDateService;
    private readonly ICacheRefService cacheRefService = cacheRefService;
    private readonly IPartialUMService partialUMService = partialUMService;

    public Task<int[]> GetProducts()
        => cacheService.GetOrSetAsync(
        ProductsCacheKey,
        async () => (await repository.FetchAsync<Product>(new ProductsForMatchWinLossDetailCriteria())).Select(p => p.Id).ToArray(),
        new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(ProductsCacheInMinutes)));

    public async Task<IEnumerable<League>> GetLeagues(GetLeaguesParam param)
    {
        var moveBetsDay = await reportDateService.GetMoveBetsDay();
        var leagues = await repository.FetchAsync<League>(new GetLeaguesCriteria(
            param.CustId,
            param.ProductIds,
            param.FromDate,
            param.ToDate,
            moveBetsDay,
            param.LanguageKey,
            param.IsShowAll));

        foreach (var item in leagues)
        {
            item.Name = leagueService.GetLeagueName(item.LeagueId, item.LeagueType, item.LeagueGroupId);
        }

        return leagues;
    }

    public async Task<MatchWinLossDetailReport> GetMatchWinLossDetail(MatchWinLossDetailParam param)
    {
        var moveBetsDay = await reportDateService.GetMoveBetsDay();
        var criteria = new MatchWinLossDetailCriteria(
            param.ViewerId,
            param.FromDate,
            param.ToDate,
            moveBetsDay,
            param.ProductIds,
            param.LeagueIds,
            param.League3rdIds,
            param.ViewerLevelId);

        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);
        var rawdata = await repository.FetchAsync<MatchWinLossDetailEntity>(criteria);

        return BuildMatchWinLossDetailReport(rawdata);
    }

    private MatchWinLossDetailReport BuildMatchWinLossDetailReport(IEnumerable<MatchWinLossDetailEntity> rawdata)
    {
        MatchWinLossDetailEntity mixParlayTotal = null;
        MatchWinLossDetailEntity systemParlay = null;
        var parlayMatchWinLossDetails = new List<BetTypeWinLossModel>();
        var normalMatchWinLossDetails = new Dictionary<string, IList<MatchWinLossDetailEntity>>();

        var normalMatchesTotal = new MatchWinLossDetailEntity();
        var grandTotal = new MatchWinLossDetailEntity();

        foreach (var item in rawdata)
        {
            if (item.IsParlay)
            {
                if (BetTypeConstants.IsMixParlay(item.BetType))
                {
                    if (mixParlayTotal == null)
                    {
                        mixParlayTotal = item;
                    }
                    else
                    {
                        mixParlayTotal += item;
                    }
                }

                if (item.BetType == BetTypeConstants.CombineMixParlay)
                {
                    if (systemParlay == null)
                    {
                        systemParlay = item;
                    }
                    else
                    {
                        systemParlay += item;
                    }
                }

                if (IsParlayMatchWinLossDetail(item.BetType))
                {
                    parlayMatchWinLossDetails.Add(new BetTypeWinLossModel(item, cacheRefService.GetBetTypeName(item.BetType)));
                }
            }
            else
            {
                var groupId = BuildGroupId(item, leagueService.GetLeagueName(item.LeagueId, item.LeagueType, item.LeagueGroupId));
                if (!normalMatchWinLossDetails.TryGetValue(groupId, out var normalMatchs))
                {
                    normalMatchs = new List<MatchWinLossDetailEntity>();
                    normalMatchWinLossDetails.Add(groupId, normalMatchs);
                }

                normalMatchs.Add(item);
                normalMatchesTotal += item;
            }

            grandTotal += item;
        }

        return new MatchWinLossDetailReport(
            rawdata.Any(),
            BuildNormalMatchWinLossDetails(normalMatchWinLossDetails).ToList(),
            new BetTypeWinLossModel(normalMatchesTotal),
            BuildMixParlayOrSystemParlay(mixParlayTotal),
            BuildMixParlayOrSystemParlay(systemParlay),
            parlayMatchWinLossDetails,
            new BetTypeWinLossModel(grandTotal));
    }

    private static bool IsParlayMatchWinLossDetail(int betType)
        => (BetTypeConstants.LeapGamingMin <= betType && betType <= BetTypeConstants.LeapGamingMax) ||
            (BetTypeConstants.Happy5LinkedGamesMin <= betType && betType <= BetTypeConstants.Happy5LinkedGamesMax);

    private static string BuildGroupId(MatchWinLossDetailEntity data, string leagueName)
    {
        if (BetTypeConstants.IsOutright(data.BetType))
        {
            return $"{leagueName}_Match{data.MatchId}";
        }

        if (BetTypeConstants.IsNumberGame(data.BetType) || BetTypeConstants.IsHappy5(data.BetType))
        {
            return $"{leagueName}_MatchCode{data.MatchCode}";
        }

        if (BetTypeConstants.IsVirtualHorseRacing(data.BetType))
        {
            return DateTime.TryParse(data.ShowTime, out var eventTime)
                ? $"{leagueName}_ShowTime{data.ShowTime}"
                : leagueName;
        }

        return $"{leagueName}_MatchId{data.MatchId}";
    }

    private BetTypeWinLossModel BuildMixParlayOrSystemParlay(MatchWinLossDetailEntity entity)
        => entity == null
            ? null
            : new BetTypeWinLossModel(entity, cacheRefService.GetBetTypeName(entity.BetType));


    private IEnumerable<MatchWinLossDetailModel> BuildNormalMatchWinLossDetails(
        IDictionary<string, IList<MatchWinLossDetailEntity>> normalMatchWinLossDetails)
    {
        foreach (var (_, normalMatches) in normalMatchWinLossDetails)
        {
            var firstWinLoss = normalMatches[0];
            var total = new MatchWinLossDetailEntity();
            var betTypeWinLoss = new List<BetTypeWinLossModel>();

            foreach (var item in normalMatches)
            {
                var betTypeWinLossModel = new BetTypeWinLossModel(item, cacheRefService.GetBetTypeName(item.BetType));

                total += item;
                betTypeWinLoss.Add(betTypeWinLossModel);
            }

            yield return new MatchWinLossDetailModel(
                firstWinLoss,
                leagueService.GetLeagueName(firstWinLoss.LeagueId, firstWinLoss.LeagueType, firstWinLoss.LeagueGroupId),
                betTypeWinLoss,
                new BetTypeWinLossModel(total),
                GetTeamName(firstWinLoss.HomeId, firstWinLoss.LeagueType),
                GetTeamName(firstWinLoss.AwayId, firstWinLoss.LeagueType),
                firstWinLoss.LeagueType);
        }
    }

    private string GetTeamName(int teamId, byte leagueType)
        => leagueType == LeagueType.ThirdParty
        ? cacheRefService.GetTeamName3rd(teamId)
        : cacheRefService.GetTeamName(teamId);
}
