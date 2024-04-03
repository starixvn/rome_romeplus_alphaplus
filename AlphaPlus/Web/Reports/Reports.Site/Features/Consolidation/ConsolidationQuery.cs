using AlphaPlus.Reports.Site.Features.Shared.Requests;

namespace AlphaPlus.Reports.Site.Features.Consolidation;

public class ConsolidationQuery : DefaultDateRangeQuery
{
    public int UserSelectedCurrencyId { get; set; }

    public string UserSelectedProductIds { get; set; } = "*";

    public string UserSelectedResourceIds { get; set; } = "*";

    public bool IsStackMode { get; set; }

    public bool IsSetDefaultStackMode => "*" == UserSelectedProductIds;

    public override bool IsValid() => Viewer.IsShowConsolidatedReport && base.IsValid();
}