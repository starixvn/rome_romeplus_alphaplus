using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class FastSpinResultQuery : Result3rdRequest, IRequest<ThirdPartyProductViewModel>
{
}
