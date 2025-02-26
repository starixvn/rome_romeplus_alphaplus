using AlphaPlus.Results.Features.Normal;
using Fanex.Result.Core.Base;

namespace AlphaPlus.Tickets.Site.Features.Results.Normal;

public class BitcoinResultService(
    IBitcoinService bitcoinService,
    IResultEngineAdapter resultEngineAdapter) : IProductResultService
{
    public async Task<string> GetResult(NormalResultQuery message)
    {
        var bitcoins = await bitcoinService.GetResults(message.ResultDate, message.LeagueId);

        return resultEngineAdapter.HtmlParser(bitcoins.ToList(), BuildSetting(message.Language));
    }

    public async Task<MemoryStream> ExportExcel(ExcelNormalResultQuery message)
    {
        var bitcoins = await bitcoinService.GetResults(message.ResultDate, message.LeagueId);

        return resultEngineAdapter.ExcelParser(bitcoins.ToList(), BuildSetting(message.Language));
    }

    public async Task<ResultDetailModel> GetResultDetails(NormalResultDetailsQuery message)
    {
        var bitcoins = await bitcoinService.GetResultDetails(message.MatchId);

        return new ResultDetailModel(
            resultEngineAdapter.HtmlParser(bitcoins.ToList(),
            BuildSetting(message.Language, true)));
    }

    private static ResultSetting BuildSetting(string language, bool isPopup = false)
        => new ResultSetting
        {
            SportType = SportType.Bitcoin,
            IsOutright = false,
            IsParlayPopUp = false,
            IsPopUp = isPopup,
            LanguageCode = language,
            VisibleMatchCodeIDColumn = true
        };
}