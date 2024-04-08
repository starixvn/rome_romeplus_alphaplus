using AlphaPlus.Reports.Features.Consolidation;
using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using MediatR;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Reports.Site.Features.Consolidation;

public class ConsolidationByUsernameHandler(
    IProductService productService,
    IOptions<AppSettings> options,
    IEnteredResultService enteredResultService,
    IConsolidationService consolidationService,
    IConsolidationByUsernameWorkbook workbook) :
        BaseHandler(productService, options),
        IRequestHandler<ConsolidationByUsernameQuery, ConsolidationByUsernameViewModel<ConsolidationByUsername>>,
        IRequestHandler<ConsolidationByUsernameExcelQuery, ExcelFileViewModel>
{
    public async Task<ConsolidationByUsernameViewModel<ConsolidationByUsername>> Handle(ConsolidationByUsernameQuery message, CancellationToken cancellationToken)
    {
        var enteredResultTask = enteredResultService.GetTotalEnteredResults(message.Site);
        var productsTask = consolidationService.GetProducts(message.ViewerId, message.MinDate, appSettings.SettingStatus);
        var currenciesTask = consolidationService.GetCurrencies(message.ViewerId);
        await Task.WhenAll(enteredResultTask, productsTask, currenciesTask);

        var (productViewModel, productDetailXml) = BuildProductViewModel(message, message.UserSelectedProductIds, message.UserSelectedResourceIds, await productsTask);
        var currencies = await currenciesTask;
        var reportData = await GetConsolidationByUsername(
            message,
            productViewModel.UserSelectedProductIds,
            productDetailXml,
            currencies);

        return new ConsolidationByUsernameViewModel<ConsolidationByUsername>(
            message,
            productViewModel,
            currencies,
            await enteredResultTask,
            reportData.ToList());
    }

    public async Task<ExcelFileViewModel> Handle(ConsolidationByUsernameExcelQuery message, CancellationToken cancellationToken)
    {
        var productsTask = consolidationService.GetProducts(message.ViewerId, message.MinDate, appSettings.SettingStatus);
        var currenciesTask = consolidationService.GetCurrencies(message.ViewerId);
        await Task.WhenAll(productsTask, currenciesTask);

        var currencies = await currenciesTask;
        var (validUserSelectedProductIds, productDetailXml) = FilterProductAndProductDetailIds(
            message,
            message.UserSelectedProductIds,
            message.UserSelectedResourceIds,
            await productsTask);
        var reportData = await GetConsolidationByUsername(
            message,
            validUserSelectedProductIds.JoinWithComma(),
            productDetailXml,
            currencies);
        var productNames = productService.ConcatProductNames(validUserSelectedProductIds);

        return workbook.BuildExcelFile(message, reportData, productNames, currencies);
    }

    private Task<IEnumerable<ConsolidationByUsername>> GetConsolidationByUsername(
        ConsolidationByUsernameQuery message,
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
}