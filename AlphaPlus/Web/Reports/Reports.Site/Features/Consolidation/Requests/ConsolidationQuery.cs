using AlphaPlus.Reports.Features.Consolidation;
using AlphaPlus.Reports.Site.Features.Shared.Requests;

namespace AlphaPlus.Reports.Site.Features.Consolidation;

public class ConsolidationQuery : ProductDetailDateRangeQuery
{
    public int UserSelectedCurrencyId { get; set; }

    public byte ReportType { get; set; }

    public override bool IsValid()
        => Viewer.IsShowConsolidatedReport
        && (ReportType == ConsolidationReportTypeConstants.Daily || ReportType == ConsolidationReportTypeConstants.Product)
        && base.IsValid();
}