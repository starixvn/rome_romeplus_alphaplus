using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class VirtualGamesDetailResultQuery : Result3rdQuery, IRequest<ThirdPartyProductViewModel>
{
}
