using MediatR;

namespace AlphaPlus.Reports.Site.Features.Legends;

public class MatchWinLossDetailLegendQuery : LegendQuery, IRequest<LegendViewModel>
{
}