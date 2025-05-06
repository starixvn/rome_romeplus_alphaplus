using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class PPLiveCasinoResultQuery : Result3rdRequest, IRequest<ThirdPartyProductViewModel>
{
}