using AlphaPlus.Results.Features.Normal;
using Fanex.Result.Core.Base;

namespace AlphaPlus.Tickets.Site.Features.Results.Normal;

public class BetBuilderResultService(
    ISportsbookService sportsbookService,
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
        var results = await sportsbookService.GetBetBuilder(message.MatchId, (int)message.BetId);

        var resultSetting = new ResultSetting
        {
            SportType = (SportType)message.SportId,
            IsOutright = false,
            IsParlayPopUp = false,
            IsPopUp = true,
            IsPopupOpenFromBetList = true,
            LanguageCode = message.Language,
            VisibleMatchCodeIDColumn = false,
            HighlightWinnerTeam = true
        };

        return new ResultDetailModel(resultEngineAdapter.HtmlParser([results], resultSetting).Replace("original-title", " title "));
    }
}