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
        var criteria = await GetWinLossCriteria(param);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var rawData = await repository.FetchAsync<WinLossModel>(criteria);

        return new ReportData<WinLossModel>(rawData, CalculateTotal(rawData));
    }

    private async Task<ICriteria> GetWinLossCriteria(WinLossParam param)
    {
        var reportDate = await reportDateService.GetBalanceUpDayAndMoveBetsDay();

        if (RoleIdConstants.IsAgentLevel(param.CustLevelId))
        {
            return new WinLossMemberCriteria(
                param.CustId,
                param.FromDate,
                param.ToDate,
                reportDate.BalanceUpDay,
                reportDate.MoveBetsDay,
                param.ProductIds,
                param.ProductDetailsXml);
        }

        if (RoleIdConstants.IsMasterLevel(param.CustLevelId))
        {
            return new WinLossAgentCriteria(
                param.CustId,
                param.FromDate,
                param.ToDate,
                reportDate.BalanceUpDay,
                reportDate.MoveBetsDay,
                param.ProductIds,
                param.ProductDetailsXml,
                param.LevelType);
        }

        return new WinLossMasterCriteria(
            param.CustId,
            param.FromDate,
            param.ToDate,
            reportDate.BalanceUpDay,
            reportDate.MoveBetsDay,
            param.ProductIds,
            param.ProductDetailsXml,
            param.LevelType);
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