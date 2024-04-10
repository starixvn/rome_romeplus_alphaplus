using AlphaPlus.Reports.Features.Consolidation;
using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Reports.Site.Features.Consolidation;

public class ConsolidationBaseHandler(
    IProductService productService,
    IOptions<AppSettings> options,
    IEnteredResultService enteredResultService,
    IConsolidationService consolidationService) : BaseHandler(productService, options)
{
    protected async Task<(
        ProductMultiSelectionViewModel,
        IEnumerable<ConsolidationCurrency>,
        EnteredResultSport,
        IEnumerable<ConsolidationByUsername>)> GetConsolidationByUsername(ConsolidationQuery message)
    {
        var (productViewModel, productDetailXml, currencies, enteredResult) = await GetConsolidationFilter(message);
        var reportData = await GetConsolidationByUsername(
            message,
            productViewModel.UserSelectedProductIds,
            productDetailXml,
            currencies);

        return (productViewModel, currencies, enteredResult, reportData.ToList());
    }

    protected async Task<(
        IEnumerable<ConsolidationByUsername>,
        string,
        IEnumerable<ConsolidationCurrency>)> GetConsolidationByUsernameExcel(ConsolidationExcelQuery message)
    {
        var (validUserSelectedProductIds, productDetailXml, productNames, currencies) = await GetConsolidationFilterExcel(message);
        var reportData = await GetConsolidationByUsername(
            message,
            validUserSelectedProductIds.JoinWithComma(),
            productDetailXml,
            currencies);

        return (reportData.ToList(), productNames, currencies);
    }

    protected async Task<(
        ProductMultiSelectionViewModel,
        IEnumerable<ConsolidationCurrency>,
        EnteredResultSport,
        IEnumerable<ConsolidationByGroup>)> GetConsolidationByDateOrProduct(ConsolidationQuery message)
    {
        var (productViewModel, productDetailXml, currencies, enteredResult) = await GetConsolidationFilter(message);
        var reportData = await GetConsolidationByDateOrProduct(
            message,
            productViewModel.UserSelectedProductIds,
            productDetailXml,
            currencies);

        return (productViewModel, currencies, enteredResult, reportData.ToList());
    }

    protected async Task<(
        IEnumerable<ConsolidationByGroup>,
        string,
        IEnumerable<ConsolidationCurrency>)> GetConsolidationByDateOrProductExcel(ConsolidationExcelQuery message)
    {
        var (validUserSelectedProductIds, productDetailXml, productNames, currencies) = await GetConsolidationFilterExcel(message);
        var reportData = await GetConsolidationByDateOrProduct(
            message,
            validUserSelectedProductIds.JoinWithComma(),
            productDetailXml,
            currencies);

        return (reportData, productNames, currencies);
    }

    protected Task<IEnumerable<ConsolidationByGroup>> GetConsolidationByDateOrProduct(
        ConsolidationQuery message,
        string validUserSelectedProductIds,
        string productDetailXml,
        IEnumerable<ConsolidationCurrency> currencies)
    {
        var param = new ConsolidationParam(
            message.ViewerId,
            message.UserSelectedCurrencyId,
            message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
            message.ToDate,
            validUserSelectedProductIds,
            productDetailXml,
            message.ReportType,
            appSettings.SettingStatus,
            message.Site,
            currencies);

        return consolidationService.GetConsolidationByDateOrProduct(param);
    }

    protected Task<IEnumerable<ConsolidationByUsername>> GetConsolidationByUsername(
        ConsolidationQuery message,
        string validUserSelectedProductIds,
        string productDetailXml,
        IEnumerable<ConsolidationCurrency> currencies)
    {
        var param = new ConsolidationParam(
            message.ViewerId,
            message.UserSelectedCurrencyId,
            message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
            message.ToDate,
            validUserSelectedProductIds,
            productDetailXml,
            message.ReportType,
            appSettings.SettingStatus,
            message.Site,
            currencies);

        return consolidationService.GetConsolidationByUsername(param);
    }

    private async Task<(
        ProductMultiSelectionViewModel,
        string,
        IEnumerable<ConsolidationCurrency>,
        EnteredResultSport)> GetConsolidationFilter(ConsolidationQuery message)
    {
        var enteredResultTask = enteredResultService.GetTotalEnteredResults(message.Site);
        var productsTask = consolidationService.GetProducts(message.ViewerId, message.MinDate, appSettings.SettingStatus);
        var currenciesTask = consolidationService.GetCurrencies(message.ViewerId);
        await Task.WhenAll(enteredResultTask, productsTask, currenciesTask);

        var (productViewModel, productDetailXml) = BuildProductViewModel(message, message.UserSelectedProductIds, message.UserSelectedResourceIds, await productsTask);

        return (productViewModel, productDetailXml, await currenciesTask, await enteredResultTask);
    }

    protected async Task<(string, string, string, IEnumerable<ConsolidationCurrency>)> GetConsolidationFilterExcel(ConsolidationExcelQuery message)
    {
        var productsTask = consolidationService.GetProducts(message.ViewerId, message.MinDate, appSettings.SettingStatus);
        var currenciesTask = consolidationService.GetCurrencies(message.ViewerId);
        await Task.WhenAll(productsTask, currenciesTask);

        var (validUserSelectedProductIds, productDetailXml) = FilterProductAndProductDetailIds(
            message,
            message.UserSelectedProductIds,
            message.UserSelectedResourceIds,
            await productsTask);
        var productNames = productService.ConcatProductNames(validUserSelectedProductIds);

        return (validUserSelectedProductIds.JoinWithComma(), productDetailXml, productNames, await currenciesTask);
    }
}