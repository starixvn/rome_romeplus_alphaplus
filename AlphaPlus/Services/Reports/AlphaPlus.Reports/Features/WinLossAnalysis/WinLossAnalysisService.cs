using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.WinLossAnalysis;

public interface IWinLossAnalysisService
{
    Task<ReportData<WinLossAnalysisModel>> GetWinLossAnalysis(WinLossAnalysisParam param);
}

public class WinLossAnalysisService(
    IReportDateService reportDateService,
    IDynamicRepository repository,
    ICacheRefService cacheRefService,
    IPartialUMService partialUMService) : IWinLossAnalysisService
{
    private readonly IReportDateService reportDateService = reportDateService;
    private readonly IDynamicRepository repository = repository;
    private readonly ICacheRefService cacheRefService = cacheRefService;
    private readonly IPartialUMService partialUMService = partialUMService;

    public async Task<ReportData<WinLossAnalysisModel>> GetWinLossAnalysis(WinLossAnalysisParam param)
    {
        var reportDate = await reportDateService.GetBalanceUpDayAndMoveBetsDay();
        var criteria = new WinLossAnalysisCriteria(
            param.ViewerId,
            param.ReportType,
            param.FromDate,
            param.ToDate,
            reportDate.BalanceUpDay,
            reportDate.MoveBetsDay,
            param.ProductIds,
            param.ViewerLevelId);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var total = new WinLossAnalysisModel();
        var rawdata = await repository.FetchAsync<WinLossAnalysisModel>(criteria);
        var isSport = param.ReportType == WinLossAnalysisConstants.Product;

        foreach (var item in rawdata)
        {
            total += item;

            if (isSport)
            {
                var productId = int.Parse(item.RollupKey);
                item.RollupKeyId = productId;
                item.RollupKey = cacheRefService.GetProductName(productId);
            }
        }

        if (isSport)
        {
            rawdata = rawdata.OrderBy(wl => wl.RollupKeyId).ToList();
        }

        return new ReportData<WinLossAnalysisModel>(rawdata, total);
    }
}