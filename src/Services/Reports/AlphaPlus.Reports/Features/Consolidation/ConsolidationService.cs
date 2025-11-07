using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.Products;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Shared.Models;
using Fanex.Data.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace AlphaPlus.Reports.Features.Consolidation;

public interface IConsolidationService
{
    Task<IEnumerable<ConsolidationCurrency>> GetCurrencies(int custId, string siteName);

    Task<IEnumerable<Product>> GetProducts(int custId, DateTime minDate, byte settingStatus, string siteName);

    Task<IEnumerable<ConsolidationByUsername>> GetConsolidationByUsername(ConsolidationParam param);

    Task<IEnumerable<ConsolidationByGroup>> GetConsolidationByDateOrProduct(ConsolidationParam param);
}

public class ConsolidationService(
    IDynamicRepository repository,
    IReportDateService reportDateService,
    IMemoryCache cacheService,
    ICacheRefService cacheRefService,
    IPartialUMService partialUMService,
    AllowedSitesToShareLoad allowedSitesToShareLoad) : IConsolidationService
{
    private const int ProductsCacheTimeInMinutes = 2;

    public Task<IEnumerable<ConsolidationCurrency>> GetCurrencies(int custId, string siteName)
        => repository.FetchAsync<ConsolidationCurrency>(new GetConsolidationCurrencyCriteria(custId, allowedSitesToShareLoad.GetSuffixBySiteName(siteName)));

    public Task<IEnumerable<Product>> GetProducts(int custId, DateTime minDate, byte settingStatus, string siteName)
        => cacheService
            .GetOrCreateAsync(
            $"AlphaPlus.Reports.Consolidation-{custId}",
            async cacheEntry =>
            {
                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromMinutes(ProductsCacheTimeInMinutes));
                var spSuffix = allowedSitesToShareLoad.GetSuffixBySiteName(siteName);
                var cashServiceSites = Core.Configurations.AppSettings.CashServiceOfferedSites.JoinWithComma();
                var products = (await repository.FetchAsync<ConsolidationProduct>(new GetConsolidationProductsCriteria(custId, settingStatus, spSuffix)))
                    .Where(product => product.ProductId != ProductIdConstants.Cashout && IsRemainSupportProduct(product, minDate))
                    .Select(product => new Product
                    {
                        Id = product.ProductId,
                        Name = cacheRefService.GetProductName(product.ProductId),
                        Type = product.Type,
                        TypeName = product.Type == 0 ? string.Empty : cacheRefService.GetProductName(product.Type)
                    });

                var productExtras = (await repository.FetchAsync<ConsolidationProduct>(
                        new GetConsolidationExtraProductsCriteria(custId, settingStatus, cashServiceSites, spSuffix)))
                .Where(product => IsRemainSupportProduct(product, minDate))
                .Select(product => new Product
                {
                    Id = product.ProductId,
                    Name = cacheRefService.GetProductName(product.ProductId),
                    Type = product.Type,
                    TypeName = product.Type == 0 ? string.Empty : cacheRefService.GetProductName(product.Type)
                });

                return products.Concat(productExtras);
            });

    private static bool IsRemainSupportProduct(ConsolidationProduct product, DateTime minReportDate)
        => product.IsShow
        && (!product.EndOfSupportDate.HasValue || minReportDate <= product.EndOfSupportDate.Value);

    public async Task<IEnumerable<ConsolidationByUsername>> GetConsolidationByUsername(ConsolidationParam param)
    {
        var rawdata = await GetConsolidation(param);

        if (rawdata.Any())
        {
            var smaCustIds = rawdata
                .Where(c => RoleIdConstants.IsNotMemberLevel(c.CustLevel))
                .Select(c => c.CustId)
                .Distinct()
                .JoinWithComma();

            var cashServicesSite = Core.Configurations.AppSettings.CashServiceOfferedSites.JoinWithComma();

            var ptList = await repository.FetchAsync<PositionTakingModel>(new GetSMAPositionTakingCriteria(smaCustIds, param.SettingStatus, cashServicesSite));
            var UpdatePTValueHandler = (ConsolidationEntity entity, decimal ptValue) =>
            {
                entity.PositionTaking = ptValue;
                return entity;
            };

            rawdata = from consolidation in rawdata
                      join pt in ptList
                          on new { consolidation.CustId, ProductId = consolidation.BreakDown }
                          equals new { pt.CustId, ProductId = pt.ProductId.ToString() } into joinData
                      from joinRecord in joinData.DefaultIfEmpty()
                      orderby consolidation.Username
                      select UpdatePTValueHandler(consolidation, joinRecord == null ? 0 : joinRecord.ValuePT);

            return BuildConsolidationByUsername(rawdata, param.Currencies);
        }

        return [];
    }

    private IEnumerable<ConsolidationByUsername> BuildConsolidationByUsername(
        IEnumerable<ConsolidationEntity> rawdata,
        IEnumerable<ConsolidationCurrency> currencies)
    {
        var currencyHash = new Dictionary<int, string>();
        foreach (var item in currencies)
        {
            currencyHash[item.Currency] = item.CurrencyName;
        }

        return rawdata.GroupBy(
            consolidation => consolidation.CustId,
            (custId, winloss) =>
            {
                ConsolidationEntity total = null;
                var sortedWinLoss = winloss.OrderByDescending(w => w.PositionTaking).ToList();
                var rewardAmount = winloss.FirstOrDefault().RewardAmount;

                foreach (var item in winloss)
                {
                    total = total ?? new ConsolidationEntity
                    {
                        BreakDown = item.BreakDown,
                        PositionTaking = item.PositionTaking,
                        CustId = item.CustId,
                        CustLevel = item.CustLevel,
                        Username = item.Username,
                        CurrencyId = item.CurrencyId,
                        CurrencyName = currencyHash.TryGetValue(item.CurrencyId, out var currencyName) ? currencyName : string.Empty,
                        RewardAmount = item.RewardAmount,
                        RewardCurrency = item.RewardCurrency
                    };

                    total += item;
                }

                total.RewardAmount = rewardAmount;

                return new ConsolidationByUsername(total, BuildConsolidationByPTs(sortedWinLoss).ToList());
            });
    }

    private IEnumerable<ConsolidationByPT> BuildConsolidationByPTs(IEnumerable<ConsolidationEntity> rawdata)
        => rawdata.GroupBy(
            consolidation => consolidation.PositionTaking,
            (pt, winloss) =>
            {
                ConsolidationEntity total = null;
                var sortedWinLoss = winloss.OrderByDescending(w => w.Turnover).ToList();

                foreach (var item in sortedWinLoss)
                {
                    item.ProductName = cacheRefService.GetProductName(item.BreakDown);
                    total = total ?? new ConsolidationEntity
                    {
                        BreakDown = item.BreakDown,
                        PositionTaking = item.PositionTaking,
                        CustId = item.CustId,
                        CustLevel = item.CustLevel,
                        Username = item.Username,
                        CurrencyId = item.CurrencyId,
                        RewardAmount = item.RewardAmount,
                        RewardCurrency = item.RewardCurrency
                    };

                    total += item;
                }

                return new ConsolidationByPT(total, sortedWinLoss);
            });

    public async Task<IEnumerable<ConsolidationByGroup>> GetConsolidationByDateOrProduct(ConsolidationParam param)
    {
        var rawdata = await GetConsolidation(param);

        if (rawdata.Any())
        {
            var isDaily = param.ReportType == ConsolidationReportType.ByDate;
            rawdata = SetCurrencies(rawdata, param.Currencies);

            return rawdata.GroupBy(
                data => data.BreakDown,
                (breakdown, winloss) => new ConsolidationByGroup(
                    isDaily
                        ? DateTime.Parse(breakdown).ToShortDateWithSlash()
                        : cacheRefService.GetProductName(breakdown),
                    winloss));
        }

        return [];
    }

    private async Task<IEnumerable<ConsolidationEntity>> GetConsolidation(ConsolidationParam param)
    {
        var reportDate = await reportDateService.GetBalanceUpDayAndMoveBetsDay();
        var spSuffix = allowedSitesToShareLoad.GetSuffixBySiteName(param.SiteName);

        var criteria = new GetConsolidationCriteria(
            param.CustId,
            param.Currency,
            param.FromDate,
            param.ToDate,
            ConsolidationReportType.MapReportType(param.ReportType),
            reportDate.BalanceUpDay,
            reportDate.MoveBetsDay,
            param.ProductIds,
            param.ProductDetailsXml,
            spSuffix);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        return await repository.FetchAsync<ConsolidationEntity>(criteria);
    }

    private static IEnumerable<ConsolidationEntity> SetCurrencies(
        IEnumerable<ConsolidationEntity> rawdata,
        IEnumerable<ConsolidationCurrency> currencies)
    {
        var currencyHash = new Dictionary<int, string>();
        foreach (var item in currencies)
        {
            currencyHash[item.Currency] = item.CurrencyName;
        }

        foreach (var item in rawdata)
        {
            item.CurrencyName = currencyHash.TryGetValue(item.CurrencyId, out var currencyName)
                ? currencyName
                : string.Empty;

            yield return item;
        }
    }
}