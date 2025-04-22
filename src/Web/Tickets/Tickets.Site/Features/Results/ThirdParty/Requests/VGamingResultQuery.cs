using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class VGamingResultQuery : Result3rdRequest, IRequest<ThirdPartyProductViewModel>
{
}
