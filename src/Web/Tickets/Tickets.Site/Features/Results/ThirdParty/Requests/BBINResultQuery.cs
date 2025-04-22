
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class BBINResultQuery : Result3rdRequest, IRequest<ThirdPartyProductViewModel>
{
}
