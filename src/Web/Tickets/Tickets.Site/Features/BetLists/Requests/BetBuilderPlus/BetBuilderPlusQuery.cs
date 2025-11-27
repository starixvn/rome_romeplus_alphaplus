using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.BetBuilderPlus;

public class BetBuilderPlusQuery : BetListBaseQuery, IRequest<string>
{
    public long TransId { get; set; }

    public string TransDesc { get; set; }

    public decimal Hdp1 { get; set; }

    public decimal Hdp2 { get; set; }

    public string BetCheck { get; set; }

    public string BetTeam { get; set; }

    public string Currency { get; set; }
    
    public decimal Stake { get; set; }
}