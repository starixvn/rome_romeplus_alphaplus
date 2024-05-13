using AlphaPlus.Results.Features.Normal;
using Fanex.Result.Core.Base;

namespace AlphaPlus.Tickets.Site.Features.Results.Normal;

public class MetricGamingResultService(
    IMetricGamingService metricGamingService,
    IResultEngineAdapter resultEngineAdapter) : IProductResultService
{
    public Task<string> GetResult(NormalResultQuery message)
    {
        throw new NotImplementedException();
    }

    public Task<MemoryStream> ExportExcel(ExcelNormalResultQuery message)
    {
        throw new NotImplementedException();
    }

    public async Task<ResultDetailModel> GetResultDetails(NormalResultDetailsQuery message)
    {
        var results = await metricGamingService.GetResults(message.BetId);

        var resultSetting = new ResultSetting
        {
            SportType = SportType.Soccer,
            IsOutright = false,
            IsParlayPopUp = false,
            IsPopUp = true,
            LanguageCode = "en-US",
            VisibleMatchCodeIDColumn = false,
            VisiblePopUpTitle = true,
            HighlightWinnerTeam = true
        };

        return new ResultDetailModel(resultEngineAdapter.HtmlParser(results.ToList(), resultSetting));
    }
}