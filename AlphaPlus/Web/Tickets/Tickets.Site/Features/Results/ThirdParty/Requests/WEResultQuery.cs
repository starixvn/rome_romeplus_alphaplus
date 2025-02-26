using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class WEResultQuery : Result3rdQuery, IRequest<ThirdPartyProductViewModel>
{
}
