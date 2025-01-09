using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class PlayStarResultQuery : Result3rdQuery, IRequest<ThirdPartyProductViewModel>
{
}
