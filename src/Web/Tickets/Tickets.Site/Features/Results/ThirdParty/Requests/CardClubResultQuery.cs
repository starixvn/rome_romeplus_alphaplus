using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class CardClubResultQuery : Result3rdRequest, IRequest<ThirdPartyProductViewModel>
{
}
