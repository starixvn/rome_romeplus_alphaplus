using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class JiliResultQuery : Result3rdRequest, IRequest<ThirdPartyProductViewModel>
{
}
