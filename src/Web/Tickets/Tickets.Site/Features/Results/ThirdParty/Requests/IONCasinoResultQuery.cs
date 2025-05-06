using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class IONCasinoResultQuery : Result3rdRequest, IRequest<ThirdPartyProductViewModel>
{
}
