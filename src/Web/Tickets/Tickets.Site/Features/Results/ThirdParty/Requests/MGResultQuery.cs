using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class MGResultQuery : Result3rdRequest, IRequest<ThirdPartyProductViewModel>
{
}
