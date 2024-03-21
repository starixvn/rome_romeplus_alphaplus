using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.WinLossAnalysis;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.WinLossAnalysis;

public class WinLossAnalysisQuery : DefaultDateRangeQuery, IRequest<WinLossAnalysisViewModel>
{
    public WinLossAnalysisQuery() : base()
        => MinDate = DateTime.Today.AddMonths(-18).FirstDayOfMonth();

    public string UserSelectedProductIds { get; set; }

    public bool IsStackMode { get; set; }

    public string SortPropertyName { get; set; } = nameof(WinLossAnalysisModel.RollupKey);

    public bool IsDescendingSort { get; set; }

    public int ReportTypeId { get; set; } = WinLossAnalysisConstants.Daily;

    public bool IsSetDefaultStackMode => string.IsNullOrWhiteSpace(UserSelectedProductIds);
}