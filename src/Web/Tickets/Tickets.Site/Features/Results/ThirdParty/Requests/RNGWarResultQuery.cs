using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class RNGWarResultQuery : Result3rdRequest, IRequest<ThirdPartyProductViewModel>
{
}
