using AlphaPlus.Results.Features.Normal;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using Fanex.Result.Core.Base;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Tickets.Site.Features.Results.Normal;

public class Parlay3rdResultService(
    IOptions<AppSettings> options,
    IParlay3rdService parlayService,
    IResultEngineAdapter resultEngineAdapter) : IProductResultService
{
    public async Task<ResultDetailModel> GetResultDetails(NormalResultDetailsQuery message)
    {
        var parlays = await parlayService.GetResults(message.RefNum);

        var resultSetting = new ResultSetting
        {
            IsPopupOpenFromBetList = true,
            IsParlayPopUp = true,
            IsPopUp = true,
            LanguageCode = message.Language,
            VisibleFreeHeader = true,
        };

        return new ResultDetailModel(resultEngineAdapter.HtmlParser(parlays, resultSetting));
    }

    public Task<string> GetResult(NormalResultQuery message) => throw new NotImplementedException();

    public Task<MemoryStream> ExportExcel(ExcelNormalResultQuery message) => throw new NotImplementedException();
}