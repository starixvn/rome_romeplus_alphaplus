using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.Legends;

public class WinLossLegendQuery : DefaultDateRangeQuery, IRequest<LegendViewModel>
{
    public bool ShowDormantAccountsNote { get; set; }
}