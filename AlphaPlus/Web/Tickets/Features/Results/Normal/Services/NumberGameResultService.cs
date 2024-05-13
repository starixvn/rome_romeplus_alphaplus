using AlphaPlus.Results.Features.Normal;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using Fanex.Result.Core.Base;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Tickets.Site.Features.Results.Normal;

public class NumberGameResultService(
    IOptions<AppSettings> options,
    INumberGameService numberGameService,
    IResultEngineAdapter resultEngineAdapter) : IProductResultService
{
    public async Task<string> GetResult(NormalResultQuery message)
    {
        var numberGames = await numberGameService.GetResults(new NumberGameParam(
            message.ResultDate,
            message.SportId,
            message.LeagueId,
            message.Language,
            options.Value.LocalTest));

        var resultSetting = new ResultSetting
        {
            SportType = (SportType)message.SportId,
            LanguageCode = message.Language,
            VisibleDetailedLink = true,
            VisibleFreeHeader = true,
            VisibleMatchCodeIDColumn = false,
            HighlightWinnerTeam = true
        };

        return resultEngineAdapter.HtmlParser(numberGames.ToList(), resultSetting);
    }

    public async Task<MemoryStream> ExportExcel(ExcelNormalResultQuery message)
    {
        var numberGames = await numberGameService.GetResults(new NumberGameParam(
            message.ResultDate,
            message.SportId,
            message.LeagueId,
            message.Language,
            options.Value.LocalTest));

        var resultSetting = new ResultSetting
        {
            SportType = (SportType)message.SportId,
            LanguageCode = message.Language,
            ExcelTitle = message.ExcelTitle,
            ExcelTemplatePath = message.TemplatePath,
            VisibleMatchCodeIDColumn = false,
            HighlightWinnerTeam = true
        };

        return resultEngineAdapter.ExcelParser(numberGames.ToList(), resultSetting);
    }

    public async Task<ResultDetailModel> GetResultDetails(NormalResultDetailsQuery message)
    {
        var numberGames = await numberGameService.GetResultDetails(message.MatchId);

        var resultSetting = new ResultSetting
        {
            SportType = SportType.NumberGame,
            IsOutright = false,
            IsParlayPopUp = false,
            IsPopUp = true,
            VisiblePopUpTitle = false,
            LanguageCode = message.Language,
            VisibleMatchCodeIDColumn = false,
            VisibleFreeHeader = true,
            VisibleDetailedLink = true,
            HighlightWinnerTeam = true
        };

        return new ResultDetailModel(resultEngineAdapter.HtmlParser(numberGames.ToList(), resultSetting));
    }
}