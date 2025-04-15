using MediatR;

namespace AlphaPlus.Reports.Site.Features.Consolidation;

public class ConsolidationDetailQuery : ConsolidationQuery, IRequest<ConsolidationDetailViewModel>
{
    public ConsolidationDetailQuery()
        => IsProtectedMode = Viewer.IsProtectedMode;

    public bool IsProtectedMode { get; }

    public string ConsolidationProductIds { get; set; }

    public string ConsolidationResourceIds { get; set; }

    public byte LevelType { get; set; }

    public DateTime ConsolidationFromDate { get; set; }

    public DateTime ConsolidationToDate { get; set; }

    public int ConsolidationUserSelectedDateRangeOption { get; set; }

    public override bool IsValid() => Viewer.IsShowConsolidatedReport && CanViewReport();

    public bool CanViewReport()
        => CustId > 0
        && ViewerId > 0
        && !string.IsNullOrWhiteSpace(CustName)
        && CheckDateRangeConstraint();
}
