using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class Live22ResultQuery : Result3rdRequest, IRequest<ThirdPartyProductViewModel>
{
}
