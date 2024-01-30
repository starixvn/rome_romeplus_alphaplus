using AlphaPlus.Core.Models;
using AlphaPlus.Tickets.Site.Features.Shared.Requests;

namespace AlphaPlus.Tickets.Site.Features.TotalBets.Requests;

public class GetTotalBetsRequest(IUserSession userSession) : RequestBase(userSession)
{
    public bool Partial { get; init; }
}
