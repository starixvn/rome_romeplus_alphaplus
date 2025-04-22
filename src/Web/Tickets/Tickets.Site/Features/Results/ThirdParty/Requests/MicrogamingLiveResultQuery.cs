using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class MicrogamingLiveResultQuery : Result3rdRequest, IRequest<ThirdPartyProductViewModel>
{
}
