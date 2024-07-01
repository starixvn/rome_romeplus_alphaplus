using AlphaPlus.BetLists.LastBets;
using AlphaPlus.Results.Features.Normal;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using Fanex.Result.Core.Base;
using Fanex.Result.Core.Sportsbook.DTO;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Tickets.Site.Features.Results.Normal;

public class SportsbookResultService(
    IOptions<AppSettings> options,
    ISportsbookService sportsbookService,
    IResultEngineAdapter resultEngineAdapter) : IProductResultService
{
    private readonly HashSet<SportType> SportTypeSupportDetails = new HashSet<SportType>()
    {
        SportType.Soccer,
        SportType.Tennis,
        SportType.BasketBall,
        SportType.VirtualBasketBall,
        SportType.ESport,
        SportType.Badminton,
        SportType.Cricket,
        SportType.VolleyBall
    };

    public async Task<string> GetResult(NormalResultQuery message)
    {
        var results = await GetResults(message);

        var sportType = message.SportId > 0 ? (SportType)message.SportId : SportType.Others;
        var resultSetting = new ResultSetting
        {
            SportType = sportType,
            LanguageCode = message.Language,
            VisibleDetailedLink = SportTypeSupportDetails.Contains(sportType),
            VisibleFreeHeader = true,
            VisibleMatchCodeIDColumn = false,
            HighlightWinnerTeam = true,
            SupportHideSabaText = options.Value.IsSiteLabel777
        };

        return resultEngineAdapter.HtmlParser(results.ToList(), resultSetting).Replace("original-title", " title ");
    }

    public async Task<MemoryStream> ExportExcel(ExcelNormalResultQuery message)
    {
        var results = await GetResults(message);
        var exportExcelSetting = new ResultSetting
        {
            SportType = (SportType)message.SportId,
            LanguageCode = message.Language,
            ExcelTitle = message.ExcelTitle,
            ExcelTemplatePath = message.TemplatePath,
            VisibleMatchCodeIDColumn = false,
            HighlightWinnerTeam = true
        };

        return resultEngineAdapter.ExcelParser(results.ToList(), exportExcelSetting);
    }

    private Task<IEnumerable<Sportsbook>> GetResults(NormalResultQuery message)
    {
        var param = new SportsbookParam(
            message.ResultDate, message.SportId, message.LeagueId, options.Value.LocalTest, message.SiteName);

        return sportsbookService.GetResults(param);
    }

    public async Task<ResultDetailModel> GetResultDetails(NormalResultDetailsQuery message)
    {
        var sportsbook = await sportsbookService.GetResultDetails(new SportsbookDetailsParam(
            message.IsViewResultFromBetList,
            message.BetTypeId,
            message.MatchId,
            message.BetId,
            message.TransDesc,
            message.Hdp2
        ));

        if (sportsbook == null)
        {
            return null;
        }
        var sportsbooks = new List<Sportsbook> { sportsbook };
        var resultSetting = new ResultSetting
        {
            SportType = (SportType)message.SportId,
            IsOutright = false,
            IsParlayPopUp = false,
            IsPopUp = true,
            IsPopupOpenFromBetList = message.IsViewResultFromBetList,
            LanguageCode = message.Language,
            VisibleMatchCodeIDColumn = false,
            HighlightWinnerTeam = true
        };
        var html = message.IsViewResultFromBetList
            ? resultEngineAdapter.HtmlParser(sportsbooks, resultSetting).Replace("original-title", " title ")
            : resultEngineAdapter.HtmlGameDetailParser(sportsbook, resultSetting).Replace("original-title", " title ");

        return new ResultDetailModel(html, sportsbooks);
    }
}