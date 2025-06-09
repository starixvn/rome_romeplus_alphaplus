using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.Products;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLossDetail.Criteria;
using AlphaPlus.Reports.Features.WinLossDetail.Models;
using Fanex.Data.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace AlphaPlus.Reports.Features.WinLossDetail;

public interface IWinLossDetailService
{
    Task<IEnumerable<WinLossDetailWeekModel>> GetWeekData(WinLossDetailWeekParam param);

    Task<ReportData<WinLossDetailModel>> GetWinLossDetail(WinLossDetailParam param);

    Task<IEnumerable<Product>> GetProduct(int custId, DateTime minReportDate, string language);

    Task<CashFeeModel> GetCashFee(CashFeeParam param);
}

public class WinLossDetailService(
    IDynamicRepository repository,
    IReportDateService reportDateService,
    IPartialUMService partialUMService,
    AllowedSitesToShareLoad allowedSitesToShareLoad,
    IMemoryCache cacheService,
    ICacheRefService cacheRefService) : IWinLossDetailService
{
    private const string ProductsCacheKey = "Alpha.Reports.Services.WinLossDetailProduct";
    private const int ExpiredTimeOfferedProductMinutes = 5;
    private readonly IDynamicRepository repository = repository;
    private readonly IReportDateService reportDateService = reportDateService;
    private readonly IPartialUMService partialUMService = partialUMService;
    private readonly AllowedSitesToShareLoad allowedSitesToShareLoad = allowedSitesToShareLoad;
    private readonly IMemoryCache cacheService = cacheService;
    private readonly ICacheRefService cacheRefService = cacheRefService;

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
            param.Username,
            param.IsReward,
            param.LevelType);

        var ip = allowedSitesToShareLoad.GetIP(param.SiteName);

        return await repository.FetchAsync<WinLossDetailWeekModel>(criteria, ip);
    }

    public async Task<ReportData<WinLossDetailModel>> GetWinLossDetail(WinLossDetailParam param)
    {
        var criteria = await GetWinLossDetailCriteria(param);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var ip = allowedSitesToShareLoad.GetIP(param.SiteName);
        var (rawData, totals) = await repository.FetchMultipleAsync<WinLossDetailModel, WinLossDetailModel>(criteria, ip);

        return new ReportData<WinLossDetailModel>(rawData, totals.FirstOrDefault(), criteria.TotalRecords);
    }

    private async Task<WinLossDetailMemberCriteria> GetWinLossDetailCriteria(WinLossDetailParam param)
    {
        var reportDate = await reportDateService.GetBalanceUpDayAndMoveBetsDay();

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
                param.IsReward);
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
                param.LevelType);
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
            param.LevelType);
    }

    public async Task<IEnumerable<Product>> GetProduct(int custId, DateTime minReportDate, string language)
    {
        var products = await cacheService.GetOrCreateAsync(
            $"{ProductsCacheKey}-{custId}-{language}",
            async cacheEntry =>
            {
                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromMinutes(ExpiredTimeOfferedProductMinutes));
                var products = await repository.FetchAsync<Product>(new WinLossDetailExtraProductCriteria(custId));

                return products
                    .Where(product => !product.IsHide)
                    .Select(product =>
                    {
                        product.Name = cacheRefService.GetProductName(product.Id);
                        return product;
                    });
            });

        return products.Where(i => IsRemainSupportProduct(i, minReportDate));
    }

    private static bool IsRemainSupportProduct(Product product, DateTime? minReportDate)
        => !minReportDate.HasValue
        || !product.EndOfSupportDate.HasValue
        || minReportDate.Value <= product.EndOfSupportDate.Value;

    public async Task<CashFeeModel> GetCashFee(CashFeeParam param)
    {
        var result = new CashFeeModel();
        var grandTotal = new CashFeeItemModel();

        var criteria = new CashBillingCriteria(param.custId, param.fromDate, param.toDate);
        var rawData = await repository.FetchAsync<CashFeeItemModel>(criteria);

        var groups = rawData.GroupBy(x => x.FeeType);

        foreach (var group in groups)
        {
            var groupItem = new CashFeeGroupModel
            {
                GroupName = group.Key
            };

            var total = new CashFeeItemModel
            {
                Type = group.Key
            };

            foreach (var item in group)
            {
                groupItem.FeeItems.Add(item);
                total += item;
                grandTotal += item;
            }

            groupItem.Total = total;
            result.FeeGroups.Add(groupItem);
        }

        result.GrandTotal = grandTotal;

        return result;
    }
}