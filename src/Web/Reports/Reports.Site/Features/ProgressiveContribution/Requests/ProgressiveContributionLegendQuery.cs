using AlphaPlus.Reports.Site.Features.Legends;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.ProgressiveContribution;

public class ProgressiveContributionLegendQuery : ProgressiveContributionQuery, IRequest<LegendViewModel>
{
}
