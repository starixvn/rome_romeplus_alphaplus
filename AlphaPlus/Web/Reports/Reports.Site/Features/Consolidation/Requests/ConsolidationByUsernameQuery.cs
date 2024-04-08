using AlphaPlus.Reports.Features.Consolidation;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.Consolidation;

public class ConsolidationByUsernameQuery
    : ConsolidationQuery, IRequest<ConsolidationByUsernameViewModel<ConsolidationByUsername>>
{
    public ConsolidationByUsernameQuery() : base()
        => ReportType = ConsolidationReportTypeConstants.Product;
}
