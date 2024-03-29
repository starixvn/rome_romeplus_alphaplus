using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.Legends;

public class MatchWinLossDetailLegendQuery : BaseQuery, IRequest<LegendViewModel>
{
}