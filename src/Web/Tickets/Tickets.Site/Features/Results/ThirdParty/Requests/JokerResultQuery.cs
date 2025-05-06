using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class JokerResultQuery : Result3rdRequest, IRequest<ThirdPartyProductViewModel>
{
}
