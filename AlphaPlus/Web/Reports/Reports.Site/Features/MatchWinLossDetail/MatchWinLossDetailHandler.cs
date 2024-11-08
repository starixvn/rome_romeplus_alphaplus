using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.MatchWinLossDetail;
using AlphaPlus.Reports.Features.Products;
using AlphaPlus.Reports.Features.Shared.Constants;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Reports.Site.Features.MatchWinLossDetail;

public class MatchWinLossDetailHandler(
    IProductService productService,
    IOptions<AppSettings> options,
    IMatchWinLossDetailService matchWinLossDetailService,
    IMatchWinLossDetailWorkbook workbook) :
        BaseHandler(productService, options),
        IRequestHandler<MatchWinLossDetailGetLeagueQuery, LeagueViewModel>,
        IRequestHandler<MatchWinLossDetailQuery, MatchWinLossDetailViewModel>,
        IRequestHandler<MatchWinLossDetailExcelQuery, ExcelFileViewModel>
{
    private readonly IMatchWinLossDetailService matchWinLossDetailService = matchWinLossDetailService;
    private readonly IMatchWinLossDetailWorkbook workbook = workbook;

    public Task<LeagueViewModel> Handle(MatchWinLossDetailGetLeagueQuery message, CancellationToken cancellationToken)
    {
        var param = new GetLeaguesParam(
            message.ViewerId,
            message.Products,
            message.FromDate,
            message.ToDate,
            message.Language,
            message.LanguageKey,
            appSettings.LocalTest);

        return BuildLeagueViewModel(param, string.Empty, true);
    }

    public async Task<MatchWinLossDetailViewModel> Handle(MatchWinLossDetailQuery message, CancellationToken cancellationToken)
    {
        var fromDate = message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo);

        var offeredProductIds = (await matchWinLossDetailService.GetProducts()).ToList();
        var productViewModel = await BuildProductViewModel(
            message,
            message.UserSelectedProductIds,
            (await GetProducts(message)).Where(p => offeredProductIds.Contains(p.Id)));

        var leagueParam = new GetLeaguesParam(
            message.ViewerId,
            productViewModel.UserSelectedProductIds,
            fromDate,
            message.ToDate,
            message.Language,
            message.LanguageKey,
            appSettings.LocalTest);
        var leagueViewModel = await BuildLeagueViewModel(leagueParam, message.UserSelectedLeagueIds, message.IsUserSelectsAllLeagues);

        var param = new MatchWinLossDetailParam(
            message.ViewerId,
            message.ViewerLevel.Value,
            fromDate,
            message.ToDate,
            productViewModel.UserSelectedProductIds,
            BuildLeagueIds(leagueViewModel.UserSelectedLeagueIds).JoinWithComma(),
            message.Site);
        var reportData = await matchWinLossDetailService.GetMatchWinLossDetail(param);

        return new MatchWinLossDetailViewModel(message, productViewModel, leagueViewModel, reportData);
    }

    public async Task<ExcelFileViewModel> Handle(MatchWinLossDetailExcelQuery message, CancellationToken cancellationToken)
    {
        var fromDate = message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo);
        var productIds = await FilterProductIds(
            message,
            message.UserSelectedProductIds,
            (await matchWinLossDetailService.GetProducts()).ToList());
        var productIdsString = productIds.JoinWithComma();

        var leagueParam = new GetLeaguesParam(
            message.ViewerId,
            productIdsString,
            fromDate,
            message.ToDate,
            message.Language,
            message.LanguageKey,
            appSettings.LocalTest);

        var leagueIds = message.IsUserSelectsAllLeagues
            ? string.Empty
            : BuildLeagueIds(await FilterLeagueIds(leagueParam, message.UserSelectedLeagueIds)).JoinWithComma();
        var param = new MatchWinLossDetailParam(
            message.ViewerId,
            message.ViewerLevel.Value,
            fromDate,
            message.ToDate,
            productIdsString,
            leagueIds,
            message.Site);
        var reportData = await matchWinLossDetailService.GetMatchWinLossDetail(param);

        return workbook.BuildExcelFile(message, reportData, productService.ConcatProductNames(productIds));
    }

    private async Task<LeagueViewModel> BuildLeagueViewModel(
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

        var validUserSelectedLeagueIdsCount = validUserSelectedLeagueIds.Count;
        var isSelectedAllLeagues = isUserSelectsAllLeagues || (validUserSelectedLeagueIdsCount > 0 && validUserSelectedLeagueIdsCount == leagues.Count);

        return new LeagueViewModel(
            leagues,
            isSelectedAllLeagues ? [] : validUserSelectedLeagueIds,
            isSelectedAllLeagues,
            param.Language);
    }

    private async Task<IEnumerable<string>> FilterLeagueIds(GetLeaguesParam param, string userSelectedLeagueIds)
    {
        var userSelectedLeagueIdList = userSelectedLeagueIds.ToList();

        return (await matchWinLossDetailService.GetLeagues(param))
            .Where(l => userSelectedLeagueIdList.Contains(l.Id))
            .Select(l => l.Id);
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