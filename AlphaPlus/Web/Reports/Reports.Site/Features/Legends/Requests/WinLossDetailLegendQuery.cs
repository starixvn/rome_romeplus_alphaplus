using MediatR;

namespace AlphaPlus.Reports.Site.Features.Legends;

public class WinLossDetailLegendQuery : WinLossLegendQuery, IRequest<LegendViewModel>
{
}
