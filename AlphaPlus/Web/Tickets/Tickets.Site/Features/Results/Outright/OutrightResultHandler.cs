using System.Globalization;
using AlphaPlus.Core.Models;
using AlphaPlus.Results.Features.Outright;
using AlphaPlus.Results.Features.Shared;
using AlphaPlus.Shared.AppResources.Betlist.Site;
using AlphaPlus.Tickets.Site.Features.Results.Normal;
using AlphaPlus.Tickets.Site.Features.Shared;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using Fanex.Result.Core.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using OutrightDTO = Fanex.Result.Core.Sportsbook.DTO.Outright;

namespace AlphaPlus.Tickets.Site.Features.Results.Outright;

public class OutrightResultHandler(
    IWebHostEnvironment webHostEnvironment,
    IOptions<AppSettings> options,
    ICacheRefService cacheRefService,
    IResultsService resultsService,
    IOutrightService outrightService,
    IResultEngineAdapter resultEngineAdatper) :
        IRequestHandler<OutrightResultQuery, OutrightResultViewModel>,
        IRequestHandler<ExcelOutrightResultQuery, ExcelFileViewModel>
{
    private const string ExcelTemplateDir = "App_Data/ExcelTemplates/Fanex.Result/";

    public async Task<OutrightResultViewModel> Handle(OutrightResultQuery message, CancellationToken cancellationToken)
    {
        var balanceUpDate = await resultsService.GetBalanceUpDate();
        IEnumerable<SelectListItem> sports = [];
        IEnumerable<SelectListItem> leagues = [];
        var htmlResults = string.Empty;

        if (message.FromDate >= balanceUpDate)
        {
            var outrights = (await outrightService.GetOutrights(new OutrightParam(
                message.FromDate,
                message.ToDate,
                options.Value.LocalTest,
                message.SportType,
                message.LeagueId,
                message.SiteName))).ToList();

            if (outrights.Any())
            {
                message.SportType = message.SportType > 0 ? message.SportType : outrights[0].SportType;
                (sports, leagues) = GetSportsAndLeagues(outrights, message.SportType, message.LeagueId);

                var resultSetting = new ResultSetting
                {
                    LanguageCode = message.Language,
                    IsOutright = true,
                    IsPopUp = false,
                    VisibleMatchCodeIDColumn = false,
                    HighlightWinnerTeam = true
                };

                htmlResults = resultEngineAdatper.HtmlParser(
                    outrightService.FilterOutrights(outrights, message.SportType, message.LeagueId).ToList(),
                    resultSetting);
            }
        }

        return new OutrightResultViewModel(
            message.FromDate,
            message.ToDate,
            message.SportType,
            sports,
            message.LeagueId,
            leagues,
            htmlResults,
            balanceUpDate);
    }

    private (IEnumerable<SelectListItem>, IEnumerable<SelectListItem>) GetSportsAndLeagues(
        IEnumerable<OutrightDTO> outrights,
        int selectedSportType,
        int selectedLeagueId)
    {
        var matchedSportType = false;
        var sports = new List<SelectListItem>();
        var leagues = new List<SelectListItem>();
        var outrightsGroup = outrights.GroupBy(outright => outright.SportType);

        foreach (var group in outrightsGroup)
        {
            var key = group.Key;
            matchedSportType = key == selectedSportType;

            sports.Add(new SelectListItem
            {
                Value = key.ToString(),
                Text = cacheRefService.GetSportName(key),
                Selected = matchedSportType
            });

            if (matchedSportType)
            {
                leagues.AddRange(
                    group.Select(outright => outright.LeagueId).Distinct()
                    .Select(league => new SelectListItem
                    {
                        Value = league.ToString(),
                        Text = cacheRefService.GetLeagueName(league),
                        Selected = league == selectedLeagueId
                    }));
            }
        }

        if (sports.Any())
        {
            leagues.Insert(0, new SelectListItem(Resources.ShowAllLeagues, string.Empty));
        }

        return (sports, leagues);
    }

    public async Task<ExcelFileViewModel> Handle(ExcelOutrightResultQuery message, CancellationToken cancellationToken)
    {
        string leagueName = message.LeagueId == 0
            ? Resources.ShowAllLeaguesExcelTitle
            : cacheRefService.GetLeagueName(message.LeagueId);
        var excelTitle = string.Format(
            "{0} - {1} -> {2} | {3} - {4}",
            Resources.OutrightResults.ToUpper(CultureInfo.CurrentCulture),
            message.FromDate.ToShortDateString(),
            message.ToDate.ToShortDateString(),
            cacheRefService.GetSportName(message.SportType),
            leagueName);
        var balanceUpDate = await resultsService.GetBalanceUpDate();
        var outrights = message.FromDate >= balanceUpDate
            ? await outrightService.GetOutrights(new OutrightParam(
                message.FromDate,
                message.ToDate,
                options.Value.LocalTest,
                message.SportType,
                message.LeagueId,
                message.SiteName))
            : [];
        outrights = outrightService.FilterOutrights(outrights, message.SportType, message.LeagueId);

        var resultSetting = new ResultSetting
        {
            LanguageCode = message.Language,
            IsOutright = true,
            IsPopUp = false,
            HighlightWinnerTeam = true,
            ExcelTitle = excelTitle,
            ExcelTemplatePath = Path.Combine(webHostEnvironment.ContentRootPath, ExcelTemplateDir)
        };
        var excelResults = resultEngineAdatper.ExcelParser(outrights.ToList(), resultSetting);

        return new ExcelFileViewModel("OutrightResults", excelResults.ToArray());
    }
}