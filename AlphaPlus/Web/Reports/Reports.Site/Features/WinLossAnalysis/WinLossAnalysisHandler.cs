using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLossAnalysis;
using AlphaPlus.Reports.Site.Features.Shared;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using MediatR;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Reports.Site.Features.WinLossAnalysis;

public class WinLossAnalysisHandler(
    IProductService productService,
    IOptions<AppSettings> options,
    IWinLossAnalysisService winLossAnalysisService,
    IWinLossAnalysisWorkbook workbook) :
        BaseHandler(productService, options),
        IRequestHandler<WinLossAnalysisQuery, WinLossAnalysisViewModel>,
        IRequestHandler<WinLossAnalysisExcelQuery, ExcelFileViewModel>
{
    public async Task<WinLossAnalysisViewModel> Handle(WinLossAnalysisQuery message, CancellationToken cancellationToken)
    {
        var productViewModel = BuildProductViewModel(message, message.UserSelectedProductIds);
        var reportData = await GetWinLossAnalysis(message, productViewModel.UserSelectedProductIds);
        var (reportTitle, reportTypeName) = GetReportTitleAndTypeName(message.ReportTypeId);

        return new WinLossAnalysisViewModel(message, reportData, productViewModel, reportTitle, reportTypeName);
    }

    public async Task<ExcelFileViewModel> Handle(WinLossAnalysisExcelQuery message, CancellationToken cancellationToken)
    {
        var validUserSelectedProductIds = FilterProductIds(message, message.UserSelectedProductIds);
        var reportData = await GetWinLossAnalysis(message, validUserSelectedProductIds.JoinWithComma());
        var (reportTitle, reportTypeName) = GetReportTitleAndTypeName(message.ReportTypeId);

        return workbook.BuildExcelFile(
            message,
            reportData,
            reportTitle,
            reportTypeName,
            productService.ConcatProductNames(validUserSelectedProductIds));
    }

    private Task<ReportData<WinLossAnalysisModel>> GetWinLossAnalysis(
        WinLossAnalysisQuery message,
        string userSelectedProductIds)
    {
        var winLossAnalysisParam = new WinLossAnalysisParam(
            message.ViewerId,
            message.ViewerLevel,
            message.Site,
            message.ReportTypeId,
            message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
            message.ToDate,
            userSelectedProductIds);

        return winLossAnalysisService.GetWinLossAnalysis(winLossAnalysisParam);
    }

    private static (string, string) GetReportTitleAndTypeName(int reportTypeId)
        => reportTypeId switch
        {
            WinLossAnalysisConstants.Daily => (Resources.DailyWinLossAnalysis, Resources.Daily),
            WinLossAnalysisConstants.Monthly => (Resources.MonthlyWinLossAnalysis, Resources.Monthly),
            WinLossAnalysisConstants.Sport => (Resources.SportWinLossAnalysis, Resources.Sport),
            _ => (string.Empty, string.Empty)
        };
}