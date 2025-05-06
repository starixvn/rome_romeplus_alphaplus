using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class SpadeGamingResultQuery : Result3rdRequest, IRequest<ThirdPartyProductViewModel>
{
}
