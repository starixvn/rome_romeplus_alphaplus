using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class NextspinResultQuery : Result3rdRequest, IRequest<ThirdPartyProductViewModel>
{
}
