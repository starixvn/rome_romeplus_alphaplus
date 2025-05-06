using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class HotDogResultQuery : Result3rdRequest, IRequest<ThirdPartyProductViewModel>
{
}
