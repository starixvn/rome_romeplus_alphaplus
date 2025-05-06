using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class MacauGamesResultQuery : Result3rdRequest, IRequest<ThirdPartyProductViewModel>
{
}
