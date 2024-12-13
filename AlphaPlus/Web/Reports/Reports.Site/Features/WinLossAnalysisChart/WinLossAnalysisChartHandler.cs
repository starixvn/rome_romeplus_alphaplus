using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Utils;
using AlphaPlus.Reports.Features.Products;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLossAnalysisChart;
using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Reports.Site.Features.WinLossAnalysisChart;

public class WinLossAnalysisChartHandler(
    IProductService productService,
    IOptions<AppSettings> options,
    IWinLossAnalysisChartService winLossAnalysisChartService) :
        BaseHandler(productService, options),
        IRequestHandler<WinLossAnalysisChartQuery, WinLossAnalysisChartViewModel>
{
    private readonly IWinLossAnalysisChartService winLossAnalysisChartService = winLossAnalysisChartService;

    public async Task<WinLossAnalysisChartViewModel> Handle(WinLossAnalysisChartQuery message, CancellationToken cancellationToken)
    {
        var productViewModel = await BuildProductViewModel(message, message.UserSelectedProductIds);

        const int DefaultPreviousMonths = -2;
        var fromDate = message.FromDate == DateTime.MinValue
            ? SystemTime.Today.AddMonths(DefaultPreviousMonths).FirstDayOfMonth()
            : message.FromDate.FirstDayOfMonth();
        var toDate = message.ToDate == DateTime.MinValue || message.ToDate.IsSameMonthWith(SystemTime.Today)
            ? SystemTime.Today
            : message.ToDate.LastDayOfMonth();
        var param = new WinLossAnalysisChartParam(
            message.ViewerId,
            message.ViewerLevel.Value,
            message.Site,
            message.CurrencyId,
            fromDate.GetGreaterDate(message.LimitedDateToShowInfo),
            toDate,
            productViewModel.UserSelectedProductIds);
        var reportData = await winLossAnalysisChartService.GetWinLossAnalysisChart(param);

        return new WinLossAnalysisChartViewModel(message, reportData, productViewModel);
    }
}