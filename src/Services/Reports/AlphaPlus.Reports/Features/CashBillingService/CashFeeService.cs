using AlphaPlus.Reports.Features.CashBillingService.Criteria;
using AlphaPlus.Reports.Features.CashBillingService.Models;
using AlphaPlus.Reports.Features.Products;
using AlphaPlus.Reports.Features.Shared;
using Fanex.Data.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace AlphaPlus.Reports.Features.CashBillingService;

public interface ICashFeeService
{
    Task<IEnumerable<Product>> GetExtraProduct(int custId, string cashServiceSites, DateTime minReportDate, string language);

    Task<CashFeeModel> GetCashFee(CashFeeParam param);
}

public class CashFeeService(
IDynamicRepository repository,
IReportDateService reportDateService,
IMemoryCache cacheService,
ICacheRefService cacheRefService) : ICashFeeService
{
    private const string ExtraProductsCacheKey = "Alpha.Reports.Services.ExtraProducts";
    private const int ExpiredTimeOfferedProductMinutes = 5;
    private readonly IDynamicRepository repository = repository;
    private readonly IReportDateService reportDateService = reportDateService;
    private readonly IMemoryCache cacheService = cacheService;
    private readonly ICacheRefService cacheRefService = cacheRefService;

    public async Task<IEnumerable<Product>> GetExtraProduct(int custId, string cashServiceSites, DateTime minReportDate, string language)
    {
        var products = await cacheService.GetOrCreateAsync(
            $"{ExtraProductsCacheKey}-{custId}-{language}",
            async cacheEntry =>
            {
                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromMinutes(ExpiredTimeOfferedProductMinutes));
                var products = await repository.FetchAsync<Product>(new ExtraProductCriteria(custId, cashServiceSites));

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

    public async Task<CashFeeModel> GetCashFee(CashFeeParam param)
    {
        var result = new CashFeeModel();
        var grandTotal = new CashFeeItemModel();

        var reportDate = await reportDateService.GetBalanceUpDayAndMoveBetsDay();

        var criteria = new CashBillingCriteria(param.custId, param.fromDate, param.toDate, reportDate.BalanceUpDay, reportDate.MoveBetsDay);
        var rawData = await repository.FetchAsync<CashFeeItemModel>(criteria);

        var groups = rawData.GroupBy(x => x.FeeType);

        foreach (var group in groups)
        {
            var groupItem = new CashFeeGroupModel
            {
                GroupName = cacheRefService.GetResourceDataByRefIdAndResourceId("CashServiceBilling", group.Key)
            };

            var total = new CashFeeItemModel
            {
                DisplayType = cacheRefService.GetResourceDataByRefIdAndResourceId("CashServiceBilling", group.Key)
            };

            foreach (var item in group)
            {
                item.DisplayType = cacheRefService.GetResourceDataByRefIdAndResourceId("CashServiceBilling", item.Type);
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

    private static bool IsRemainSupportProduct(Product product, DateTime? minReportDate)
   => !minReportDate.HasValue
   || !product.EndOfSupportDate.HasValue
   || minReportDate.Value <= product.EndOfSupportDate.Value;


}
