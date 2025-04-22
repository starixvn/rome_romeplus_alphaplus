using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class ONCasinoResultQuery : Result3rdRequest, IRequest<ThirdPartyProductViewModel>
{
}
