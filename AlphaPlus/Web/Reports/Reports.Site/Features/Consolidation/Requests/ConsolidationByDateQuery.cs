using AlphaPlus.Reports.Features.Consolidation;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.Consolidation;

public class ConsolidationByDateQuery : ConsolidationQuery, IRequest<ConsolidationByDateViewModel>
{
    public ConsolidationByDateQuery() : base()
        => ReportType = ConsolidationReportType.ByDate;
}