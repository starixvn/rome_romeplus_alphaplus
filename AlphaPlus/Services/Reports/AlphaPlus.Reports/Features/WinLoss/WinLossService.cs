using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.WinLoss;

public interface IWinLossService
{
    Task<ReportData<WinLossModel>> GetWinLoss(WinLossParam param);
}

public class WinLossService(
    IDynamicRepository repository,
    IReportDateService reportDateService,
    IPartialUMService partialUMService) : IWinLossService
{
    private readonly IDynamicRepository repository = repository;
    private readonly IReportDateService reportDateService = reportDateService;
    private readonly IPartialUMService partialUMService = partialUMService;

    public async Task<ReportData<WinLossModel>> GetWinLoss(WinLossParam param)
    {
        var reportDate = await reportDateService.GetBalanceUpDayAndMoveBetsDay();
        var criteria = new WinLossCriteria(
            param.CustId,
            param.FromDate,
            param.ToDate,
            reportDate.BalanceUpDay,
            reportDate.MoveBetsDay,
            param.ProductIds,
            param.CustLevelId,
            param.ProductDetailsXml);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var rawData = await repository.FetchAsync<WinLossModel>(criteria);

        return new ReportData<WinLossModel>(rawData, CalculateTotal(rawData));
    }

    private static WinLossModel CalculateTotal(IEnumerable<WinLossModel> rawData)
    {
        var total = new WinLossModel();

        foreach (var item in rawData)
        {
            total += item;
        }

        return total;
    }
}