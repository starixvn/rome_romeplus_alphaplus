using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class Soccer5ResultQuery : Result3rdQuery, IRequest<ThirdPartyProductViewModel>
{
    public string MatchId { get; set; }

    public string Balls { get; set; }

    public int BetTypeId { get; set; }

    public int SportTypeId { get; set; }
}