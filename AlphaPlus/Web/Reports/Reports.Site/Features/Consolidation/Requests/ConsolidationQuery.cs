using AlphaPlus.Reports.Site.Features.Shared.Requests;

namespace AlphaPlus.Reports.Site.Features.Consolidation;

public abstract class ConsolidationQuery : ProductDetailDateRangeQuery
{
    public string ReportType { get; set; }

    public int UserSelectedCurrencyId { get; set; }

    public override bool IsValid() => Viewer.IsShowConsolidatedReport && base.IsValid();
}