using AlphaPlus.Reports.Features.MatchWinLossDetail;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Constants;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Reports.Site.Features.MatchWinLossDetail;

public class MatchWinLossDetailHandler(
    IProductService productService,
    IOptions<AppSettings> options,
    IMatchWinLossDetailService matchWinLossDetailService) :
        BaseHandler(productService, options),
        IRequestHandler<GetLeaguesQuery, LeaguesViewModel>,
        IRequestHandler<MatchWinLossDetailQuery, MatchWinLossDetailViewModel>
{
    public Task<LeaguesViewModel> Handle(GetLeaguesQuery message, CancellationToken cancellationToken)
    {
        var param = new GetLeaguesParam(
            message.ViewerId,
            message.Products,
            message.FromDate,
            message.ToDate,
            message.LanguageKey,
            appSettings.LocalTest);

        return BuildLeaguesViewModel(param, string.Empty, true);
    }

    public async Task<MatchWinLossDetailViewModel> Handle(MatchWinLossDetailQuery message, CancellationToken cancellationToken)
    {
        var fromDate = message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo);

        var offeredProductIds = (await matchWinLossDetailService.GetProducts()).ToList();
        var productViewModel = BuildProductViewModel(
            message,
            message.UserSelectedProductIds,
            GetProducts(message).Where(p => offeredProductIds.Contains(p.Id)));

        var leagueParam = new GetLeaguesParam(
            message.ViewerId,
            productViewModel.UserSelectedProductIds,
            fromDate,
            message.ToDate,
            message.LanguageKey,
            options.Value.LocalTest);
        var leaguesViewModel = await BuildLeaguesViewModel(leagueParam, message.UserSelectedLeagueIds, message.IsUserSelectsAllLeagues);

        var param = new MatchWinLossDetailParam(
            message.ViewerId,
            message.ViewerLevel,
            fromDate,
            message.ToDate,
            productViewModel.UserSelectedProductIds,
            BuildLeagueIds(leaguesViewModel.UserSelectedLeagueIds).JoinWithComma(),
            message.Site);
        var reportData = await matchWinLossDetailService.GetMatchWinLossDetail(param);

        throw new NotImplementedException();
    }

    // private BetTypeWinLossViewModel GetMixParlay(IEnumerable<MatchWinLossDetailModel> reportData)
    // {
    //     var mixParlayData = reportData.FirstOrDefault(data => data.IsParlay &&
    //         (data.BetType == BetTypeIdConstants.MixParlay || data.BetType == BetTypeIdConstants.MixParlayBetRadarVirtualSports));

    //     return mixParlayData == null
    //         ? null
    //         : new BetTypeWinLossViewModel(mixParlayData, cacheRefService.GetBetType(mixParlayData.BetType));
    // }

    private async Task<LeaguesViewModel> BuildLeaguesViewModel(
        GetLeaguesParam param,
        string userSelectedLeagueIds = "",
        bool isUserSelectsAllLeagues = false)
    {
        var leagues = new List<SelectListItem>();
        var validUserSelectedLeagueIds = new List<string>();

        var userSelectedLeagueIdList = isUserSelectsAllLeagues
            ? []
            : userSelectedLeagueIds.ToList();
        var allLeagues = await matchWinLossDetailService.GetLeagues(param);
        foreach (var item in allLeagues)
        {
            var isSelected = isUserSelectsAllLeagues || userSelectedLeagueIdList.Contains(item.Id);
            leagues.Add(new SelectListItem(item.Name, item.Id, isSelected));
            if (isSelected)
            {
                validUserSelectedLeagueIds.Add(item.Id);
            }
        }

        return new LeaguesViewModel(
            leagues,
            validUserSelectedLeagueIds,
            isUserSelectsAllLeagues || validUserSelectedLeagueIds.Count == leagues.Count);
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