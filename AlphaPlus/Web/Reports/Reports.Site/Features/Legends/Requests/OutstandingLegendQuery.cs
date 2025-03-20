using MediatR;

namespace AlphaPlus.Reports.Site.Features.Legends;

public class OutstandingLegendQuery : LegendQuery, IRequest<LegendViewModel>
{
}