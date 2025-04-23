using MediatR;

namespace AlphaPlus.Reports.Site.Features.Legends;

public class WinLossLegendQuery : LegendQuery, IRequest<LegendViewModel>
{
    public bool ShowDormantAccountsNote { get; set; }

    public bool IsConsolidation { get; set; }
}