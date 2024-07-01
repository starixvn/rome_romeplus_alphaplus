using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLossDetail;
using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail;

public class WinLossDetailHandler(
    IProductService productService,
    IOptions<AppSettings> options,
    IEnteredResultService enteredResultService,
    IWinLossDetailService winlossDetailService,
    IWinLossDetailWorkbook workbook) :
        BaseHandler(productService, options),
        IRequestHandler<WinLossDetailQuery, WinLossDetailViewModel>,
        IRequestHandler<WinLossDetailExcelQuery, ExcelFileViewModel>
{
    public async Task<WinLossDetailViewModel> Handle(WinLossDetailQuery message, CancellationToken cancellationToken)
    {
        var (productViewModel, productDetailXml) = BuildProductViewModel(message, message.UserSelectedProductIds, message.UserSelectedResourceIds);

        var reportDataTask = GetWinLossDetail(message, productViewModel.UserSelectedProductIds, productDetailXml);
        var enteredResultTask = enteredResultService.GetTotalEnteredResults(message.Site);
        await Task.WhenAll(reportDataTask, enteredResultTask);

        return new WinLossDetailViewModel(
            message,
            productViewModel,
            BuildBreadcrumbViewModel(message),
            await enteredResultTask,
            await reportDataTask);
    }

    public async Task<ExcelFileViewModel> Handle(WinLossDetailExcelQuery message, CancellationToken cancellationToken)
    {
        var (validUserSelectedProductIds, productDetailXml) = FilterProductAndProductDetailIds(
            message,
            message.UserSelectedProductIds,
            message.UserSelectedResourceIds);
        var reportData = await GetWinLossDetail(message, validUserSelectedProductIds.JoinWithComma(), productDetailXml);
        var productNames = productService.ConcatProductNames(validUserSelectedProductIds);

        return workbook.BuildExcelFile(message, reportData, productNames);
    }

    private Task<ReportData<WinLossDetailModel>> GetWinLossDetail(
        WinLossDetailQuery message,
        string validUserSelectedProductIds,
        string productDetailXml)
    {
        var param = new WinLossDetailParam(
            message.CustId,
            message.CustLevel,
            message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
            message.ToDate,
            validUserSelectedProductIds,
            productDetailXml,
            null,
            message.Site);

        return winlossDetailService.GetWinLossDetail(param);
    }
}