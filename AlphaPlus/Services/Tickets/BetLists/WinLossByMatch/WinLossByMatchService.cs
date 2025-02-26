using AlphaPlus.BetLists.Shared.Helpers;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.BetLists.Shared.Services;
using AlphaPlus.IPInfo.Services;
using AlphaPlus.PartialUM.Services;
using AlphaPlus.Utilities.IP;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.WinLossByMatch;

public interface IWinLossByMatchService
{
    Task<BetListData> GetBetList(WinLossByMatchParam param);
}

public class WinLossByMatchService(
    IReportDateService reportDateService,
    IPartialUMService partialUMService,
    IDynamicRepository repository,
    IIPInfoService ipInfoService,
    IPService ipService) :
        BetListServiceBase(repository, ipInfoService, ipService),
        IWinLossByMatchService
{
    private readonly IReportDateService reportDateService = reportDateService;
    private readonly IPartialUMService partialUMService = partialUMService;

    public async Task<BetListData> GetBetList(WinLossByMatchParam param)
    {
        ICriteria criteria;
        if (BetTypeHelper.IsOutRight(param.BetType))
        {
            criteria = await GetWinLossOutrightCriteria(param);
        }
        else if (BetTypeHelper.IsHorseRacing(param.BetType))
        {
            criteria = await GetWinLossRacingCriteria(param);
        }
        else if (BetTypeHelper.IsLeapGaming(param.BetType) || BetTypeHelper.IsHappy5LinkedGames(param.BetType))
        {
            criteria = await GetWinLossByBetTypeCriteria(param);
        }
        else
        {
            criteria = await GetWinLossByMatchCriteria(param);
        }

        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        return await GetBetList(criteria, param.DisplayVNIP);
    }

    private async Task<ICriteria> GetWinLossOutrightCriteria(WinLossByMatchParam param)
        => new GetWinLossOutrightCriteria(
            param.CustId,
            param.LeagueId,
            param.Language,
            param.FromDate,
            param.ToDate,
            await reportDateService.GetMoveBetsDay(),
            param.CustLevelId);

    private async Task<ICriteria> GetWinLossRacingCriteria(WinLossByMatchParam param)
        => new GetWinLossRacingCriteria(
            param.CustId,
            param.BetType,
            param.Race,
            param.FromDate,
            param.ToDate,
            await reportDateService.GetMoveBetsDay(),
            param.CustLevelId);

    private async Task<ICriteria> GetWinLossByMatchCriteria(WinLossByMatchParam param)
        => new GetWinLossByMatchCriteria(
            param.CustId,
            param.FromDate,
            param.ToDate,
            await reportDateService.GetMoveBetsDay(),
            param.MatchId,
            BetTypeHelper.GetEventType(param.LeagueType),
            param.BetType,
            param.CustName,
            param.LiveIndicator,
            param.CustLevelId);

    private async Task<ICriteria> GetWinLossByBetTypeCriteria(WinLossByMatchParam param)
        => new GetWinLossByBetTypeCriteria(
            param.CustId,
            param.FromDate,
            param.ToDate,
            await reportDateService.GetMoveBetsDay(),
            param.CustLevelId,
            param.BetType);
}