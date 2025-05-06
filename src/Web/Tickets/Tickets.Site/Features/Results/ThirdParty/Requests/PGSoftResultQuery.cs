using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class PGSoftResultQuery : Result3rdRequest, IRequest<ThirdPartyProductViewModel>
{
}
