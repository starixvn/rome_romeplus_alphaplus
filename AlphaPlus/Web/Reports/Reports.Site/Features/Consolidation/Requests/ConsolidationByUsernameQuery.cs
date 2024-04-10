using AlphaPlus.Reports.Features.Consolidation;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.Consolidation;

public class ConsolidationByUsernameQuery : ConsolidationQuery, IRequest<ConsolidationByUsernameViewModel>
{
    public ConsolidationByUsernameQuery() : base()
        => ReportType = ConsolidationReportType.ByUsername;
}
