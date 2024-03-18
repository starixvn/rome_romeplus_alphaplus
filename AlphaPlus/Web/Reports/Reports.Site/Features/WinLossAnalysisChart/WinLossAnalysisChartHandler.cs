using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.WinLossAnalysisChart;
using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.WinLossAnalysisChart;

public class WinLossAnalysisChartHandler(
    IProductService productService,
    IWinLossAnalysisChartService winLossAnalysisChartService) :
        BaseHandler(productService),
        IRequestHandler<WinLossAnalysisChartQuery, WinLossAnalysisChartViewModel>
{
    public async Task<WinLossAnalysisChartViewModel> Handle(WinLossAnalysisChartQuery message, CancellationToken cancellationToken)
    {
        var productViewModel = BuildProductViewModel(message, message.UserSelectedProductIds);
        var param = new WinLossAnalysisChartParam(
            message.ViewerId,
            message.ViewerLevel,
            message.Site,
            message.CurrencyId,
            message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
            message.ToDate,
            productViewModel.UserSelectedProductIds);
        var reportData = await winLossAnalysisChartService.GetWinLossAnalysisChart(param);

        return new WinLossAnalysisChartViewModel(message, reportData, productViewModel);
    }
}