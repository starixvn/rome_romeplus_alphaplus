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
    IPartialUMService partialUMService,
    AllowedSitesToShareLoad allowedSitesToShareLoad) : IWinLossDetailService
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

        var ip = allowedSitesToShareLoad.GetIP(param.SiteName);

        return await repository.FetchAsync<WinLossDetailWeekModel>(criteria, ip);
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
            param.CustLevelId,
            param.ProductDetailsXml,
            param.Username);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var total = new WinLossDetailModel();
        var ip = allowedSitesToShareLoad.GetIP(param.SiteName);
        var rawdata = await repository.FetchAsync<WinLossDetailModel>(criteria, ip);
        foreach (var item in rawdata)
        {
            total += item;
        }

        return new ReportData<WinLossDetailModel>(rawdata, total, criteria.TotalRecords);
    }
}