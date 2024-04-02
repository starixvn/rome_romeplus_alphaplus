using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.Shared;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.SuperWinLossDetailByPT;

public interface ISuperWinLossDetailByPTService
{
    Task<(IEnumerable<SuperWinLossDetailByPTModel>, SuperWinLossDetailByPTEntity, IEnumerable<SuperPTModel>)> GetSuperWinLossDetailByPT(SuperWinLossDetailByPTParam param);
}

public class SuperWinLossDetailByPTService(
    IDynamicRepository repository,
    IReportDateService reportDateService,
    ICacheRefService cacheRefService,
    IPartialUMService partialUMService) : ISuperWinLossDetailByPTService
{
    public async Task<(IEnumerable<SuperWinLossDetailByPTModel>, SuperWinLossDetailByPTEntity, IEnumerable<SuperPTModel>)> GetSuperWinLossDetailByPT(SuperWinLossDetailByPTParam param)
    {
        var reportDate = await reportDateService.GetBalanceUpDayAndMoveBetsDay();
        var criteria = new SuperWinLossDetailByPTCriteria(
            param.CustId,
            param.FromDate,
            param.ToDate,
            reportDate.BalanceUpDay,
            reportDate.MoveBetsDay,
            param.ProductIds,
            param.ProductDetailsXml);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var ptListTask = GetSuperPT(param.CustId, param.SettingStatus);
        var winlossTask = repository.FetchAsync<SuperWinLossDetailByPTEntity>(criteria);
        await Task.WhenAll(ptListTask, winlossTask);

        var ptList = await ptListTask;
        var rawdata = await winlossTask;
        var grandTotal = new SuperWinLossDetailByPTEntity();

        var reportData = rawdata.Join(
            ptList,
            winloss => winloss.ProductId,
            pt => pt.ProductId,
            (winloss, pt) =>
            {
                winloss.SuperPT = pt.SuperPT;
                return winloss;
            })
        .GroupBy(
            winloss => winloss.SuperPT,
            (pt, winloss) =>
            {
                var total = new SuperWinLossDetailByPTEntity();
                var winlossSortedList = winloss.OrderByDescending(wl => wl.Turnover).ToList();
                foreach (var item in winlossSortedList)
                {
                    item.ProductName = cacheRefService.GetProductName(item.ProductId);
                    total += item;
                }

                grandTotal += total;

                return new SuperWinLossDetailByPTModel(pt, winlossSortedList, total);
            })
        .OrderByDescending(wl => wl.SuperPT)
        .ToList();

        return (reportData, grandTotal, ptList);
    }

    private Task<IEnumerable<SuperPTModel>> GetSuperPT(int superId, byte settingStatus)
        => repository.FetchAsync<SuperPTModel>(new SuperPTCriteria(superId.ToString(), settingStatus));
}