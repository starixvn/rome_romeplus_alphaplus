using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.Legends;

public class LegendQuery : DefaultDateRangeQuery, IRequest<LegendViewModel>
{
    public bool IsShowGrossComm { get; init; }

    public bool IsMobileFriendly { get; init; }
}
