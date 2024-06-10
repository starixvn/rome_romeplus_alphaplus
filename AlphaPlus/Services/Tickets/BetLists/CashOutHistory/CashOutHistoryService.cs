using AlphaPlus.BetLists.Shared.Services;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.CashOutHistory;

public interface ICashOutHistoryService
{
    Task<CashOutHistoryModel> GetHistory(CashOutHistoryParam param);
}

public class CashOutHistoryService(
    IReportDateService reportDateService,
    IDynamicRepository repository) : ICashOutHistoryService
{
    public async Task<CashOutHistoryModel> GetHistory(CashOutHistoryParam param)
    {
        var balanceUpDay = await reportDateService.GetBalanceUpDay();
        var criteria = new GetCashOutHistoryCriteria(param.TransId, param.CustId, param.WinLossDate, balanceUpDay);
        var cashOutHistory = await repository.GetAsync<CashOutHistoryModel>(criteria);

        return new CashOutHistoryModel
        {
            TransId = param.TransId,
            CashoutExtraInfo = cashOutHistory != null ? cashOutHistory.CashoutExtraInfo : string.Empty
        };
    }
}