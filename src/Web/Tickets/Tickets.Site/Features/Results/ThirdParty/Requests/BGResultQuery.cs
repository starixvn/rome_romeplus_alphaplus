using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class BGResultQuery : Result3rdRequest, IRequest<ThirdPartyProductViewModel>
{
}
