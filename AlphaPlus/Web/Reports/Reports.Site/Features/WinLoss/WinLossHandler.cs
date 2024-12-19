using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Features.Products;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLoss;
using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
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
    private readonly IWinLossService winlossService = winlossService;
    private readonly IEnteredResultService enteredResultService = enteredResultService;
    private readonly IWinLossWorkbook workbook = workbook;

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
            message.Site);

        return winlossService.GetWinLoss(param);
    }
}