using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class FACHAIResultQuery : Result3rdRequest, IRequest<ThirdPartyProductViewModel>
{
}
