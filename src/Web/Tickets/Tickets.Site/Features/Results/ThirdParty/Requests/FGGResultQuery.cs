using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class FGGResultQuery : Result3rdRequest, IRequest<ThirdPartyProductViewModel>
{
}
