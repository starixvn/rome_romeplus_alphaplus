using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.BetBuilderPlus;

public class BetBuilderPlusQuery : BetListBaseQuery, IRequest<string>
{
    public long TransId { get; set; }

    public string TransDesc { get; set; }
}