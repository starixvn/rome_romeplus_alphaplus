using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.WinLossDetail;

public interface IWinLossDetailService
{
    Task<IEnumerable<WinLossDetailWeekModel>> GetWeekData(WinLossDetailWeekParam param);

    Task<ReportData<WinLossDetailModel>> GetWinLossDetail(WinLossDetailParam param);
}

public class WinLossDetailService(
    IDynamicRepository repository,
    IReportDateService reportDateService,
    IPartialUMService partialUMService) : IWinLossDetailService
{
    public async Task<IEnumerable<WinLossDetailWeekModel>> GetWeekData(WinLossDetailWeekParam param)
    {
        var reportDate = await reportDateService.GetBalanceUpDayAndMoveBetsDay();
        var criteria = new WinLossDetailWeekDataCriteria(
            param.ViewerId,
            param.FromDate,
            param.ToDate,
            reportDate.BalanceUpDay,
            reportDate.MoveBetsDay,
            param.ProductIds,
            param.ProductDetailsXml,
            param.Username);

        return await repository.FetchAsync<WinLossDetailWeekModel>(criteria);
    }

    public async Task<ReportData<WinLossDetailModel>> GetWinLossDetail(WinLossDetailParam param)
    {
        var reportDate = await reportDateService.GetBalanceUpDayAndMoveBetsDay();
        var criteria = new WinLossDetailCriteria(
            param.CustId,
            param.FromDate,
            param.ToDate,
            reportDate.BalanceUpDay,
            reportDate.MoveBetsDay,
            param.ProductIds,
            param.CustLevel,
            param.ProductDetailsXml,
            param.Username);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var total = new WinLossDetailModel();
        var rawdata = await repository.FetchAsync<WinLossDetailModel>(criteria);
        foreach (var item in rawdata)
        {
            total += item;
        }

        return new ReportData<WinLossDetailModel>(rawdata, total, criteria.TotalRecords);
    }
}