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

        var total = new WinLossModel();
        var rawdata = await repository.FetchAsync<WinLossModel>(criteria);
        foreach (var item in rawdata)
        {
            total += item;
        }

        return new ReportData<WinLossModel>(rawdata, total);
    }
}