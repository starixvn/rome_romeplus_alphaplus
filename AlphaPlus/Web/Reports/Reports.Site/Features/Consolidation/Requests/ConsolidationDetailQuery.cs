using MediatR;

namespace AlphaPlus.Reports.Site.Features.Consolidation;

public class ConsolidationDetailQuery : ConsolidationQuery, IRequest<ConsolidationDetailViewModel>
{
    public ConsolidationDetailQuery()
        => IsProtectedMode = Viewer.IsProtectedMode;

    public bool IsProtectedMode { get; }

    public string ConsolidationProductIds { get; set; }

    public string ConsolidationResourceIds { get; set; }

    public DateTime ConsolidationFromDate { get; set; }

    public DateTime ConsolidationToDate { get; set; }

    public int ConsolidationUserSelectedDateRangeOption { get; set; }
}
