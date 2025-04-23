using AlphaPlus.Reports.Features.Consolidation;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.Consolidation;

public class ConsolidationByProductQuery : ConsolidationQuery, IRequest<ConsolidationByProductViewModel>
{
    public ConsolidationByProductQuery()
        => ReportType = ConsolidationReportType.ByProduct;

    public override bool IsValid()
        => ReportType == ConsolidationReportType.ByProduct && base.IsValid();
}
