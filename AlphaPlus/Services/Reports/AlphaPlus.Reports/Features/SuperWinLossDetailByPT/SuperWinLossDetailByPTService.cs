using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.SuperWinLossDetailByPT;

public interface ISuperWinLossDetailByPTService
{
    Task<(IEnumerable<SuperWinLossDetailByPTModel>, SuperWinLossDetailByPTEntity, IEnumerable<PositionTakingModel>)> GetSuperWinLossDetailByPT(SuperWinLossDetailByPTParam param);
}

public class SuperWinLossDetailByPTService(
    IDynamicRepository repository,
    IReportDateService reportDateService,
    ICacheRefService cacheRefService,
    IPartialUMService partialUMService) : ISuperWinLossDetailByPTService
{
    private readonly IDynamicRepository repository = repository;
    private readonly IReportDateService reportDateService = reportDateService;
    private readonly ICacheRefService cacheRefService = cacheRefService;
    private readonly IPartialUMService partialUMService = partialUMService;

    public async Task<(IEnumerable<SuperWinLossDetailByPTModel>, SuperWinLossDetailByPTEntity, IEnumerable<PositionTakingModel>)> GetSuperWinLossDetailByPT(SuperWinLossDetailByPTParam param)
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

        var grandTotal = new SuperWinLossDetailByPTEntity();
        var rawDataTask = repository.FetchAsync<SuperWinLossDetailByPTEntity>(criteria);
        var ptListTask = GetSuperPT(param.CustId, param.SettingStatus);
        await Task.WhenAll(rawDataTask, ptListTask);

        var rawData = await rawDataTask;
        var ptList = await ptListTask;

        if (rawData.Any())
        {
            var reportData = rawData.Join(
                ptList,
                winloss => winloss.ProductId,
                pt => pt.ProductId,
                (winloss, pt) =>
                {
                    winloss.SuperPT = pt.ValuePT;
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
            .ToList();

            return (reportData, grandTotal, ptList);
        }

        return ([], grandTotal, ptList);
    }

    private Task<IEnumerable<PositionTakingModel>> GetSuperPT(int superId, byte settingStatus)
        => repository.FetchAsync<PositionTakingModel>(new SuperPTCriteria(superId.ToString(), settingStatus));
}