using AlphaPlus.Results.Features.Normal;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using Fanex.Result.Core.Base;
using Fanex.Result.Core.Happy5.DTO;
using Fanex.Result.Core.NumberGame.DTO;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Tickets.Site.Features.Results.Normal;

public class Happy5ResultService(
    IOptions<AppSettings> options,
    INumberGameService numberGameService,
    IResultEngineAdapter resultEngineAdapter) : IProductResultService
{
    public async Task<string> GetResult(NormalResultQuery message)
    {
        var resultSetting = new ResultSetting
        {
            SportType = (SportType)message.SportId,
            LanguageCode = message.Language,
            VisibleDetailedLink = true,
            VisibleFreeHeader = true,
            VisibleMatchCodeIDColumn = false,
            HighlightWinnerTeam = true
        };
        var data = await GetHappy5(message);
        return resultEngineAdapter.HtmlParser(data.ToList(), resultSetting);
    }

    public async Task<MemoryStream> ExportExcel(ExcelNormalResultQuery message)
    {
        var resultSetting = new ResultSetting
        {
            SportType = (SportType)message.SportId,
            LanguageCode = message.Language,
            ExcelTitle = message.ExcelTitle,
            ExcelTemplatePath = message.TemplatePath,
            VisibleMatchCodeIDColumn = false,
            HighlightWinnerTeam = true
        };

        return resultEngineAdapter.ExcelParser(await GetHappy5(message), resultSetting);
    }

    private async Task<IEnumerable<Happy5>> GetHappy5(NormalResultQuery message)
    {
        var numberGames = await numberGameService.GetResults(new NumberGameParam(
            message.ResultDate,
            message.SportId,
            message.LeagueId,
            message.Language,
            options.Value.LocalTest));

        return numberGames.Select(BuildHappy5);
    }

    public async Task<ResultDetailModel> GetResultDetails(NormalResultDetailsQuery message)
    {
        var numberGames = await numberGameService.GetResultDetails(message.MatchId);
        var happy5List = numberGames.Select(BuildHappy5);
        var resultSetting = new ResultSetting
        {
            SportType = SportType.Happy5,
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

        return new ResultDetailModel(resultEngineAdapter.HtmlParser(happy5List.ToList(), resultSetting));
    }

    private static Happy5 BuildHappy5(NumberGame numberGame)
        => new Happy5
        {
            Ball = numberGame.Ball,
            EventDate = numberGame.EventDate,
            EventStatus = numberGame.EventStatus,
            IsPending = numberGame.IsPending,
            KickOffTime = numberGame.KickOffTime,
            LeagueId = numberGame.LeagueId,
            MatchCode = numberGame.MatchCode,
            MatchId = numberGame.MatchId,
            Seq = numberGame.Seg
        };
}