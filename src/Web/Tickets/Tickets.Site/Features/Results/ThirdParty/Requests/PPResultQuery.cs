using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class PPResultQuery : Result3rdRequest, IRequest<ThirdPartyProductViewModel>
{
}
