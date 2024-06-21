using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class CardClubResultQuery : Result3rdQuery, IRequest<ThirdPartyProductViewModel>
{
}
