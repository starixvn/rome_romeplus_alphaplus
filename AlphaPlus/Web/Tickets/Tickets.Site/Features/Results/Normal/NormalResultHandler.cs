using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Results.Features.Shared;
using AlphaPlus.Tickets.Site.App_GlobalResources;
using AlphaPlus.Tickets.Site.Features.Shared;
using AlphaPlus.Tickets.Site.Features.Shared.Constants;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Tickets.Site.Features.Results.Normal;

public class NormalResultHandler(
    IWebHostEnvironment webHostEnvironment,
    IOptions<AppSettings> options,
    ICacheRefService cacheRefService,
    IResultsService resultsService,
    IResultServiceFactory resultServiceFactory) :
        IRequestHandler<NormalResultQuery, NormalResultViewModel>,
        IRequestHandler<ExcelNormalResultQuery, ExcelFileViewModel>,
        IRequestHandler<NormalResultDetailsQuery, NormalDetailViewModel>
{
    private const string ExcelTemplateDir = "App_Data/ExcelTemplates/Fanex.Result/";

    public async Task<NormalResultViewModel> Handle(NormalResultQuery message, CancellationToken cancellationToken)
    {
        var isShowAll = options.Value.LocalTest;
        var sportParam = new SportParam(message.ResultDate, isShowAll, message.SiteName, message.SportsStatus);
        var sportsTask = BuildSports(sportParam);
        var leaguesTask = message.SportId > 0
            ? BuildLeagues(new LeagueParam(message.ResultDate, message.SportId, isShowAll, message.SiteName))
            : Task.FromResult<IEnumerable<SelectListItem>>([]);
        var resultTask = message.SportId > 0 && message.LeagueId > 0
            ? CreateProductResultService(message).GetResult(message)
            : Task.FromResult<string>(null);
        var balanceUpDateTask = resultsService.GetBalanceUpDate();

        await Task.WhenAll(sportsTask, leaguesTask, resultTask, balanceUpDateTask);

        return new NormalResultViewModel(
            message.ResultDate,
            message.SportId,
            await sportsTask,
            message.LeagueId,
            await leaguesTask,
            await resultTask,
            await balanceUpDateTask);
    }

    private async Task<IEnumerable<SelectListItem>> BuildSports(SportParam param)
    {
        var sports = await resultsService.GetSports(param);

        return sports.Select(sport => new SelectListItem(
            cacheRefService.GetSportName(sport.Id),
            sport.Id.ToString()));
    }

    private async Task<IEnumerable<SelectListItem>> BuildLeagues(LeagueParam param)
    {
        var leagues = await resultsService.GetLeagues(param);

        return leagues.Select(league => new SelectListItem(
            SportTypeConstants.SportListUsingLeague3rd.Contains(league.SportType)
                ? cacheRefService.GetLeague3rdName(league.Id)
                : cacheRefService.GetLeagueName(league.Id),
            league.Id.ToString()));
    }

    public async Task<ExcelFileViewModel> Handle(ExcelNormalResultQuery message, CancellationToken cancellationToken)
    {
        message.TemplatePath = Path.Combine(webHostEnvironment.ContentRootPath, ExcelTemplateDir);

        var leagueName = message.LeagueId == 0
            ? Resources.ShowAllLeaguesExcelTitle
            : GetLeagueName(message.LeagueId, message.SportId);
        var resultDate = message.ResultDate.ToShortDateWithSlash();
        message.ExcelTitle = $"{Resources.Results} - {resultDate} | {cacheRefService.GetSportName(message.SportId)} - {leagueName}";

        var excelResults = await CreateProductResultService(message).ExportExcel(message);

        return new ExcelFileViewModel("Results", excelResults.ToArray());
    }

    private IProductResultService CreateProductResultService(NormalResultQuery message)
        => resultServiceFactory.CreateProductResultService(message.SportId, message.BetTypeId);

    private string GetLeagueName(int leagueId, int leagueGroupId)
        => options.Value.IsSiteLabel777
            ? cacheRefService.GetLeagueName(leagueId, leagueGroupId)
            : cacheRefService.GetLeagueName(leagueId);

    public async Task<NormalDetailViewModel> Handle(NormalResultDetailsQuery message, CancellationToken cancellationToken)
    {
        var leagueId = message.LeagueId;
        var leagueGroupId = 0;
        var html = string.Empty;
        var resultDetails = await CreateProductResultService(message).GetResultDetails(message);

        if (resultDetails != null)
        {
            leagueId = resultDetails.Match.LeagueId;
            leagueGroupId = resultDetails.Match.LeagueGroupId;
            html = resultDetails.Html;
        }

        var sportName = cacheRefService.GetSportName(message.SportId);
        var leagueName = leagueId > 0
            ? GetLeagueName(leagueId, leagueGroupId)
            : cacheRefService.GetBetTypeName(message.BetTypeId);
        var title = !string.IsNullOrWhiteSpace(sportName) && !string.IsNullOrWhiteSpace(leagueName)
            ? $"{sportName} - {leagueName}"
            : $"{sportName} {leagueName}";

        return new NormalDetailViewModel(title, html);
    }
}