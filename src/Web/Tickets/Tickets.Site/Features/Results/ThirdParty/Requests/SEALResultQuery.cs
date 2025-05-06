using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class SEALResultQuery : Result3rdRequest, IRequest<ThirdPartyProductViewModel>
{
}
