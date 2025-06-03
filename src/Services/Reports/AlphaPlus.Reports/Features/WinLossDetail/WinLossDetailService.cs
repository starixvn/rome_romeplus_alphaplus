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

    Task<IEnumerable<Product>> GetProduct(int custId);

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

    public async Task<IEnumerable<Product>> GetProduct(int custId)
            => await cacheService.GetOrCreateAsync(
                $"{ProductsCacheKey}.{custId}",
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

    public async Task<CashFeeModel> GetCashFee(CashFeeParam param)
    {
        var result = new CashFeeModel();
        var grandTotal = new CashFeeItemModel
        {
            TransactionType = "Grand Total",
            Fee = 10,
            Count = 20,
            Agent = -15,
            Company = 15
        };

        var groupData = new CashFeeGroupModel
        {
            GroupName = "Transaction Fee",
            FeeItems = new List<CashFeeItemModel>
            {
                new CashFeeItemModel
                {
                    TransactionType = "Manual Deposit Transaction",
                    Fee = 0,
                    Count = 10,
                    Agent = -1,
                    Company = 1

                },
                new CashFeeItemModel
                {
                    TransactionType = "Promotion approval",
                    Fee = 1,
                    Count = 10,
                    Agent = -2,
                    Company = 2
                }
            },
            Total = new CashFeeItemModel
            {
                TransactionType = "Transaction Fee",
                Fee = 10,
                Count = 15,
                Agent = -5,
                Company = 5
            }
        };

        var groupData1 = new CashFeeGroupModel
        {
            GroupName = "Cash Service Fee",
            FeeItems = new List<CashFeeItemModel>
            {
                new CashFeeItemModel
                {
                    TransactionType = "Manual Deposit Transaction",
                    Fee = 0,
                    Count = 10,
                    Agent = -1,
                    Company = 1

                },
                new CashFeeItemModel
                {
                    TransactionType = "Promotion approval",
                    Fee = 1,
                    Count = 10,
                    Agent = -2,
                    Company = 2
                }
            },
            Total = new CashFeeItemModel
            {
                TransactionType = "Cash Service Fee",
                Fee = 10,
                Count = 15,
                Agent = -5,
                Company = 5
            }
        };

        //var criteria = new CashBillingCriteria();
        //var rawData = await repository.FetchAsync<CashFeeItemModel>(criteria);

        //var groups = rawData.GroupBy(x => x.TransactionType);

        //foreach (var group in groups)
        //{
        //    var groupItem = new CashFeeGroupModel
        //    {
        //        GroupName = group.Key
        //    };

        //    var total = new CashFeeItemModel();

        //    foreach (var item in group)
        //    {
        //        groupItem.FeeItems.Add(item);
        //        total += item;
        //        grandTotal += item;
        //    }

        //    result.FeeGroups.Add(groupItem);
        //}

        result.FeeGroups.AddRange(new[] { groupData, groupData1 });

        result.GrandTotal = grandTotal;

        return result;
    }
}