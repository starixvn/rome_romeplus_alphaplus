using AlphaPlus.Tickets.Site.Features.Shared.Requests;

namespace AlphaPlus.Tickets.Site.Features.TotalBets.Requests;

public record GetTotalBetsRequest(bool Partial) : RequestBase();
