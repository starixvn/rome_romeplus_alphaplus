using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.BetLists.Shared.Services;
using AlphaPlus.IPInfo.Services;
using AlphaPlus.PartialUM.Services;
using AlphaPlus.Utilities.IP;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.DailyStatement;

public interface IDailyStatementService
{
    Task<BetListData> GetBetList(DailyStatementParam param);
}

public class DailyStatementService(
    IReportDateService reportDateService,
    IPartialUMService partialUMService,
    IDynamicRepository repository,
    IIPInfoService ipInfoService,
    IPService ipService) : BetListServiceBase(repository, ipInfoService, ipService), IDailyStatementService
{
    private readonly IPartialUMService partialUMService = partialUMService;

    public async Task<BetListData> GetBetList(DailyStatementParam param)
    {
        var reportDate = await reportDateService.GetBalanceUpDayAndMoveBetsDay();
        var criteria = param.WinLossDate < reportDate.BalanceUpDay
            ? new GetAllBetListByDayOldCriteria(param.CustId, param.WinLossDate, param.TransType)
            : new GetAllBetListByDayCriteria(param.CustId, param.WinLossDate, reportDate.MoveBetsDay, param.TransType);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        return await GetBetList(criteria, param.DisplayVNIP);
    }
}