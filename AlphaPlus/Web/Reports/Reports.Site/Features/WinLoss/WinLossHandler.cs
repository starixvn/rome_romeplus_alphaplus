using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLoss;
using AlphaPlus.Reports.Site.Features.Shared;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using MediatR;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Reports.Site.Features.WinLoss;

public class WinLossHandler(
    IProductService productService,
    IOptions<AppSettings> options,
    IWinLossService winlossService,
    IEnteredResultService enteredResultService,
    IWinLossWorkbook workbook) :
        BaseHandler(productService, options),
        IRequestHandler<WinLossQuery, WinLossViewModel>,
        IRequestHandler<WinLossExcelQuery, ExcelFileViewModel>
{
    public async Task<WinLossViewModel> Handle(WinLossQuery message, CancellationToken cancellationToken)
    {
        var (productViewModel, productDetailXml) = BuildProductViewModel(message, message.UserSelectedProductIds, message.UserSelectedResourceIds);

        var reportDataTask = GetWinLoss(message, productViewModel.UserSelectedProductIds, productDetailXml);
        var enteredResultTask = enteredResultService.GetTotalEnteredResults(message.Site);
        await Task.WhenAll(reportDataTask, enteredResultTask);

        return new WinLossViewModel(
            message,
            productViewModel,
            BuildBreadcrumbViewModel(message),
            await enteredResultTask,
            await reportDataTask);
    }

    public async Task<ExcelFileViewModel> Handle(WinLossExcelQuery message, CancellationToken cancellationToken)
    {
        var (validUserSelectedProductIds, productDetailXml) = FilterProductAndProductDetailIds(
            message,
            message.UserSelectedProductIds,
            message.UserSelectedResourceIds);
        var reportData = await GetWinLoss(message, validUserSelectedProductIds.JoinWithComma(), productDetailXml);
        var productNames = productService.ConcatProductNames(validUserSelectedProductIds);

        return workbook.BuildExcelFile(message, reportData, productNames);
    }

    private Task<ReportData<WinLossModel>> GetWinLoss(
        WinLossQuery message,
        string validUserSelectedProductIds,
        string productDetailXml)
    {
        var param = new WinLossParam(
            message.CustId,
            message.CustLevel,
            message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
            message.ToDate,
            validUserSelectedProductIds,
            productDetailXml,
            message.Site);

        return winlossService.GetWinLoss(param);
    }
}