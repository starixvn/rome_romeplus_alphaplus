using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.CashBillingService;
using AlphaPlus.Reports.Features.CashBillingService.Models;
using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Features.Products;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLoss;
using AlphaPlus.Reports.Site.Features.CashBillingService;
using AlphaPlus.Reports.Site.Features.CashBillingService.Requests;
using AlphaPlus.Reports.Site.Features.Shared;
using AlphaPlus.Reports.Site.Features.Shared.Helpers;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using AlphaPlus.Shared.AppResources.Report;
using MediatR;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Reports.Site.Features.WinLoss;

public class WinLossHandler(
    IProductService productService,
    IOptions<AppSettings> options,
    IWinLossService winlossService,
    IEnteredResultService enteredResultService,
    IWinLossWorkbook workbook,
    ICashFeeService cashFeeService,
    ICashFeeWorkbook cashFeeWorkbook) :
        BaseHandler(productService, options),
        IRequestHandler<WinLossQuery, WinLossViewModel>,
        IRequestHandler<WinLossExcelQuery, ExcelFileViewModel>,
        IRequestHandler<CashFeeGetDataQuery, CashFeeViewModel>,
        IRequestHandler<CashFeeExcelQuery, ExcelFileViewModel>
{
    private readonly IWinLossService winlossService = winlossService;
    private readonly IEnteredResultService enteredResultService = enteredResultService;
    private readonly IWinLossWorkbook workbook = workbook;
    private readonly ICashFeeService cashFeeService = cashFeeService;
    private readonly ICashFeeWorkbook cashFeeWorkbook = cashFeeWorkbook;

    public async Task<WinLossViewModel> Handle(WinLossQuery message, CancellationToken cancellationToken)
    {
        var (productViewModel, productDetailXml) = await BuildProductViewModel(message, message.UserSelectedProductIds, message.UserSelectedResourceIds);

        var reportDataTask = GetWinLoss(message, productViewModel.UserSelectedProductIds, productDetailXml);
        var enteredResultTask = message.IsHistoricReport
            ? Task.FromResult<EnteredResultSport>(null)
            : enteredResultService.GetTotalEnteredResults(message.Site);
        await Task.WhenAll(reportDataTask, enteredResultTask);

        return new WinLossViewModel(
            message,
            productViewModel,
            await enteredResultTask,
            await reportDataTask);
    }

    public async Task<ExcelFileViewModel> Handle(WinLossExcelQuery message, CancellationToken cancellationToken)
    {
        var (validUserSelectedProductIds, productDetailXml, productNames) = await FilterProductAndProductDetailIdsForExcel(
            message,
            message.UserSelectedProductIds,
            message.UserSelectedResourceIds);
        var reportData = await GetWinLoss(message, validUserSelectedProductIds.JoinWithComma(), productDetailXml);

        return workbook.BuildExcelFile(message, reportData, productNames);
    }

    private Task<ReportData<WinLossModel>> GetWinLoss(
        WinLossQuery message,
        string validUserSelectedProductIds,
        string productDetailXml)
    {
        var param = new WinLossParam(
            message.CustId,
            message.CustLevel.Value,
            message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
            message.ToDate,
            validUserSelectedProductIds,
            productDetailXml,
            message.LevelType,
            message.Site);

        return winlossService.GetWinLoss(param);
    }

    public async Task<CashFeeViewModel> Handle(CashFeeGetDataQuery message, CancellationToken cancellationToken)
    {
        var breadcrumbs = BreadcrumbHelper.BuildBreadcrumbs(message).ToList();
        breadcrumbs.Add(new Reports.Features.Shared.Models.Breadcrumb(0, Resources.CashServiceBilling.ToUpper(), 0));

        var cashFeeParam = new CashFeeParam(
            message.CustId,
            message.FromDate,
            message.ToDate);

        var cashFeeModel = await cashFeeService.GetCashFee(cashFeeParam);

        return new CashFeeViewModel(message, cashFeeModel, breadcrumbs);
    }

    protected override async Task<IEnumerable<Product>> GetProducts(BaseQuery message)
    {
        var products = await base.GetProducts(message);

        if (Core.Configurations.AppSettings.IsOfferedCashService(message.Site))
        {
            var productExtras = await cashFeeService.GetExtraProduct(message.CustId, message.MinDate, message.Language);
            return products.Concat(productExtras);
        }

        return products;
    }

    public async Task<ExcelFileViewModel> Handle(CashFeeExcelQuery message, CancellationToken cancellationToken)
    {
        var cashFeeParam = new CashFeeParam(
           message.CustId,
           message.FromDate,
           message.ToDate);

        var cashFeeModel = await cashFeeService.GetCashFee(cashFeeParam);

        var cashFeeExcelParam = new CashFeeExcelParam
        {
            FromDate =  message.FromDate,
            ToDate =  message.ToDate,
            CustName = message.CustName
        };

        return cashFeeWorkbook.BuildCashFeeExcelFile(cashFeeExcelParam, cashFeeModel);
    }
}