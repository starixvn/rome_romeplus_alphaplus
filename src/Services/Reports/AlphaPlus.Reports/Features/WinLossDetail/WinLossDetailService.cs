using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.WinLossDetail;

using Criteria;

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
        var suffix = allowedSitesToShareLoad.GetSuffixBySiteName(param.SiteName);

        var criteria = new WinLossDetailWeekDataCriteria(
            param.ViewerId,
            param.FromDate,
            param.ToDate,
            reportDate.BalanceUpDay,
            reportDate.MoveBetsDay,
            param.ProductIds,
            param.ProductDetailsXml,
            param.Username,
            param.IsReward,
            param.LevelType,
            suffix);

        return await repository.FetchAsync<WinLossDetailWeekModel>(criteria);
    }

    public async Task<ReportData<WinLossDetailModel>> GetWinLossDetail(WinLossDetailParam param)
    {
        var criteria = await GetWinLossDetailCriteria(param);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var ip = allowedSitesToShareLoad.GetSuffixBySiteName(param.SiteName);
        var (rawData, totals) = await repository.FetchMultipleAsync<WinLossDetailModel, WinLossDetailModel>(criteria, ip);

        return new ReportData<WinLossDetailModel>(rawData, totals.FirstOrDefault(), criteria.TotalRecords);
    }

    private async Task<WinLossDetailMemberCriteria> GetWinLossDetailCriteria(WinLossDetailParam param)
    {
        var reportDate = await reportDateService.GetBalanceUpDayAndMoveBetsDay();
        var spSuffix = allowedSitesToShareLoad.GetSuffixBySiteName(param.SiteName);
        if (RoleIdConstants.IsAgentLevel(param.CustLevelId))
        {
            return new WinLossDetailMemberCriteria(
                param.CustId,
                param.FromDate,
                param.ToDate,
                reportDate.BalanceUpDay,
                reportDate.MoveBetsDay,
                param.ProductIds,
                param.ProductDetailsXml,
                param.Username,
                param.IsReward,
                spSuffix);
        }

        if (RoleIdConstants.IsMasterLevel(param.CustLevelId))
        {
            return new WinLossDetailAgentCriteria(
                param.CustId,
                param.FromDate,
                param.ToDate,
                reportDate.BalanceUpDay,
                reportDate.MoveBetsDay,
                param.ProductIds,
                param.ProductDetailsXml,
                param.Username,
                param.IsReward,
                param.LevelType,
                spSuffix);
        }

        return new WinLossDetailMasterCriteria(
            param.CustId,
            param.FromDate,
            param.ToDate,
            reportDate.BalanceUpDay,
            reportDate.MoveBetsDay,
            param.ProductIds,
            param.ProductDetailsXml,
            param.Username,
            param.IsReward,
            param.LevelType,
            spSuffix);
    }
}