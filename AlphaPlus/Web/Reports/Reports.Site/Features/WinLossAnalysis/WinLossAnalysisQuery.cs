using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Utils;
using AlphaPlus.Reports.Features.WinLossAnalysis;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.WinLossAnalysis;

public class WinLossAnalysisQuery : DefaultDateRangeQuery, IRequest<WinLossAnalysisViewModel>
{
    public WinLossAnalysisQuery()
        => MinDate = SystemTime.Today.AddMonths(-18).FirstDayOfMonth();

    public string UserSelectedProductIds { get; set; } = "*";

    public bool IsStackMode { get; set; }

    public string SortPropertyName { get; set; } = nameof(WinLossAnalysisModel.RollupKey);

    public byte DescendingSort { get; set; }

    public int ReportTypeId { get; set; } = WinLossAnalysisConstants.Daily;

    public bool IsSetDefaultStackMode => "*" == UserSelectedProductIds;
}