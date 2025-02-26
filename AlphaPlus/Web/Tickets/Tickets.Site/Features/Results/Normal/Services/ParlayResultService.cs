using AlphaPlus.Results.Features.Normal;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using Fanex.Result.Core.Base;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Tickets.Site.Features.Results.Normal;

public class ParlayResultService(
    IOptions<AppSettings> options,
    IParlayService parlayService,
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
        var parlays = await parlayService.GetResults(message.RefNum);

        var resultSetting = new ResultSetting
        {
            IsPopupOpenFromBetList = message.IsViewResultFromBetList,
            IsOutright = false,
            IsParlayPopUp = true,
            IsPopUp = true,
            VisiblePopUpTitle = false,
            LanguageCode = message.Language,
            VisibleMatchCodeIDColumn = false,
            VisibleFreeHeader = true,
            VisibleDetailedLink = true,
            HighlightWinnerTeam = true,
            SupportHideSabaText = options.Value.IsSiteLabel777
        };

        return new ResultDetailModel(resultEngineAdapter.HtmlParser(parlays, resultSetting));
    }
}