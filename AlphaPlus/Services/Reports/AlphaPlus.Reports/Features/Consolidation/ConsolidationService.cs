using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Constants;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Shared.Models;
using Fanex.Caching;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.Consolidation;

public interface IConsolidationService
{
    Task<IEnumerable<ConsolidationCurrency>> GetCurrencies(int custId);

    Task<IEnumerable<Product>> GetProducts(int custId, DateTime minDate, byte settingStatus);

    Task<IEnumerable<ConsolidationByUsername>> GetConsolidationByUsername(GetConsolidationParam param);

    Task<IEnumerable<ConsolidationByDate>> GetConsolidationByDate(GetConsolidationParam param);

    Task<IEnumerable<ConsolidationByProduct>> GetConsolidationByProduct(GetConsolidationParam param);
}

public class ConsolidationService(
    IDynamicRepository repository,
    IReportDateService reportDateService,
    ICacheService cacheService,
    ICacheRefService cacheRefService,
    IPartialUMService partialUMService) : IConsolidationService
{
    private const int ProductsCacheTimeInMinutes = 2;
    public const byte ReportTypeDaily = 2;
    public const byte ReportTypeProduct = 3;

    public Task<IEnumerable<ConsolidationCurrency>> GetCurrencies(int custId)
        => repository.FetchAsync<ConsolidationCurrency>(new GetConsolidationCurrencyCriteria(custId));

    public Task<IEnumerable<Product>> GetProducts(int custId, DateTime minDate, byte settingStatus)
        => cacheService
            .GetOrSetAsync(
            $"AlphaPlus.Reports.Consolidation-{custId}",
            async () => (await repository.FetchAsync<ConsolidationProduct>(new GetConsolidationProductsCriteria(custId, settingStatus)))
                .Where(product => product.ProductId != ProductIdConstants.Cashout && IsRemainSupportProduct(product, minDate))
                .Select(product => new Product
                {
                    Id = product.ProductId,
                    Name = cacheRefService.GetProductName(product.ProductId),
                    Type = product.Type,
                    TypeName = product.Type == 0 ? string.Empty : cacheRefService.GetProductName(product.Type)
                }),
            new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(ProductsCacheTimeInMinutes)));

    private static bool IsRemainSupportProduct(ConsolidationProduct product, DateTime minReportDate)
        => product.IsShow
        && (!product.EndOfSupportDate.HasValue || minReportDate <= product.EndOfSupportDate.Value);

    public async Task<IEnumerable<ConsolidationByUsername>> GetConsolidationByUsername(GetConsolidationParam param)
    {
        var rawdata = await GetConsolidation<ConsolidationByUsername>(param, ReportTypeProduct);

        if (rawdata.Any())
        {
            var memberValue = UserLevel.Member.Value;
            var smaCustIds = rawdata
                .Where(c => c.CustLevel != memberValue)
                .Select(c => c.CustId)
                .Distinct()
                .JoinWithComma();
            var ptList = await repository.FetchAsync<SMAPositionTaking>(new GetSMAPositionTakingCriteria(smaCustIds, param.SettingStatus));

            return from consolidation in rawdata
                   join pt in ptList
                        on new { consolidation.CustId, ProductId = consolidation.BreakDown }
                        equals new { pt.CustId, pt.ProductId } into joinData
                   from joinRecord in joinData.DefaultIfEmpty()
                   orderby consolidation.Username
                   select new ConsolidationByUsername
                   {
                       BreakDown = consolidation.CustId,
                       CustId = consolidation.CustId,
                       Username = consolidation.Username,
                       CurrencyId = consolidation.CurrencyId,
                       CustLevel = consolidation.CustLevel,
                       Turnover = consolidation.Turnover,
                       GrossComm = consolidation.GrossComm,
                       MemberWinLoss = consolidation.MemberWinLoss,
                       MemberComm = consolidation.MemberComm,
                       MemberTotal = consolidation.MemberTotal,
                       OwnerTurnover = consolidation.OwnerTurnover,
                       OwnerWinLoss = consolidation.OwnerWinLoss,
                       OwnerComm = consolidation.OwnerComm,
                       OwnerTotal = consolidation.OwnerTotal,
                       Company = consolidation.Company,
                       BetCount = consolidation.BetCount,
                       ProductName = cacheRefService.GetProductName(consolidation.BreakDown),
                       PositionTaking = joinRecord == null ? 0 : joinRecord.ValuePT
                   };
        }

        return [];
    }

    public Task<IEnumerable<ConsolidationByDate>> GetConsolidationByDate(GetConsolidationParam param)
        => GetConsolidation<ConsolidationByDate>(param, ReportTypeDaily);

    public Task<IEnumerable<ConsolidationByProduct>> GetConsolidationByProduct(GetConsolidationParam param)
        => GetConsolidation<ConsolidationByProduct>(param, ReportTypeProduct);

    private async Task<IEnumerable<T>> GetConsolidation<T>(GetConsolidationParam param, byte reportType)
    {
        var reportDate = await reportDateService.GetBalanceUpDayAndMoveBetsDay();
        var criteria = new GetConsolidationCriteria(
            param.CustId,
            param.Currency,
            param.FromDate,
            param.ToDate,
            reportType,
            reportDate.BalanceUpDay,
            reportDate.MoveBetsDay,
            param.ProductIds,
            param.ProductDetailsXml);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        return await repository.FetchAsync<T>(criteria);
    }
}