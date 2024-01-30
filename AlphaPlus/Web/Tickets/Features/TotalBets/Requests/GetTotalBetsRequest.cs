using AlphaPlus.Tickets.Site.Features.Shared.Requests;

namespace AlphaPlus.Tickets.Site.Features.TotalBets.Requests;

public class GetTotalBetsRequest : RequestBase
{
    public bool Partial { get; init; }
}
