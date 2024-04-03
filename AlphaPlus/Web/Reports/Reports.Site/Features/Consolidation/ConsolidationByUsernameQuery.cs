using MediatR;

namespace AlphaPlus.Reports.Site.Features.Consolidation;

public class ConsolidationByUsernameQuery : ConsolidationQuery, IRequest<ConsolidationByUsernameViewModel>
{
}
