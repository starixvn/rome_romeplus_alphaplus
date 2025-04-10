using AlphaPlus.Results.Features.Normal;
using Fanex.Result.Core.Base;

namespace AlphaPlus.Tickets.Site.Features.Results.Normal;

public class VirtualRacing3rdResultService(
    IVirtualRacing3rdService virtualRacingService,
    IResultEngineAdapter resultEngineAdapter) : IProductResultService
{
    public async Task<string> GetResult(NormalResultQuery message)
    {
        var racingList = await virtualRacingService.GetResults(new VirtualRacingParam(
            message.ResultDate, message.SportId, message.LeagueId));
        var resultSetting = new ResultSetting
        {
            SportType = (SportType)message.SportId,
            LanguageCode = message.Language,
            VisibleDetailedLink = true,
            VisibleFreeHeader = true,
            VisibleMatchCodeIDColumn = false,
            HighlightWinnerTeam = true
        };

        return resultEngineAdapter.HtmlParser(racingList.ToList(), resultSetting);
    }

    public async Task<MemoryStream> ExportExcel(ExcelNormalResultQuery message)
    {
        var racingList = await virtualRacingService.GetResults(new VirtualRacingParam(
            message.ResultDate, message.SportId, message.LeagueId));
        var resultSetting = new ResultSetting
        {
            SportType = (SportType)message.SportId,
            LanguageCode = message.Language,
            ExcelTitle = message.ExcelTitle,
            ExcelTemplatePath = message.TemplatePath,
            VisibleMatchCodeIDColumn = false,
            HighlightWinnerTeam = true
        };

        return resultEngineAdapter.ExcelParser(racingList.ToList(), resultSetting);
    }

    public async Task<ResultDetailModel> GetResultDetails(NormalResultDetailsQuery message)
    {
        var racingList = await virtualRacingService.GetResultDetails(new VirtualRacingDetailParam(message.ResultDate, message.MatchId));
        var resultSetting = new ResultSetting
        {
            SportType = (SportType)message.SportId,
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

        return new ResultDetailModel(resultEngineAdapter.HtmlParser(racingList.ToList(), resultSetting));
    }
}