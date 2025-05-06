using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class SabaKenoResultQuery : Result3rdRequest, IRequest<ThirdPartyProductViewModel>
{
}
