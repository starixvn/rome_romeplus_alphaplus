using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class LinkedGamesResultQuery : Result3rdRequest, IRequest<ThirdPartyProductViewModel>
{
    public int SportTypeId { get; set; }

    public int BetTypeId { get; set; }
}
