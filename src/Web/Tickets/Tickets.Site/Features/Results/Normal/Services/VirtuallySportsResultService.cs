using AlphaPlus.Results.Features.Normal;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using Fanex.Result.Core.Base;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Tickets.Site.Features.Results.Normal;

public class VirtuallySportsResultService(
    IOptions<AppSettings> options,
    IVirtualSportsService virtualSportsService,
    IResultEngineAdapter resultEngineAdapter) : IProductResultService
{
    public async Task<string> GetResult(NormalResultQuery message)
    {
        var virtualSports = await virtualSportsService.GetResults(new SportsbookParam(
            message.ResultDate, message.SportId, message.LeagueId, options.Value.LocalTest, message.SiteName));
        var resultSetting = new ResultSetting
        {
            SportType = (SportType)message.SportId,
            LanguageCode = message.Language
        };

        return resultEngineAdapter.HtmlParser(virtualSports.ToList(), resultSetting);
    }

    public async Task<MemoryStream> ExportExcel(ExcelNormalResultQuery message)
    {
        var racingList = await virtualSportsService.GetResults(new SportsbookParam(
            message.ResultDate, message.SportId, message.LeagueId, options.Value.LocalTest, message.SiteName));
        var resultSetting = new ResultSetting
        {
            SportType = (SportType)message.SportId,
            LanguageCode = message.Language,
            ExcelTitle = message.ExcelTitle,
            VisibleMatchCodeIDColumn = true,
            ExcelTemplatePath = message.TemplatePath
        };

        return resultEngineAdapter.ExcelParser(racingList.ToList(), resultSetting);
    }

    public async Task<ResultDetailModel> GetResultDetails(NormalResultDetailsQuery message)
    {
        var racingList = await virtualSportsService.GetResultDetails(message.MatchId);
        var resultSetting = new ResultSetting
        {
            SportType = (SportType)message.SportId,            
            IsPopUp = true,            
            LanguageCode = message.Language            
        };

        return new ResultDetailModel(resultEngineAdapter.HtmlParser(racingList.ToList(), resultSetting));
    }
}