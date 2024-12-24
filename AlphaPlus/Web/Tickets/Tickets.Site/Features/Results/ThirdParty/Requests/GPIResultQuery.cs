using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class GPIResultQuery : Result3rdQuery, IRequest<ThirdPartyProductViewModel>
{
}
