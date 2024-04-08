using AlphaPlus.Reports.Features.Consolidation;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.Consolidation;

public class ConsolidationByGroupQuery
    : ConsolidationQuery, IRequest<ConsolidationByGroupViewModel<ConsolidationByGroup>>
{
}
