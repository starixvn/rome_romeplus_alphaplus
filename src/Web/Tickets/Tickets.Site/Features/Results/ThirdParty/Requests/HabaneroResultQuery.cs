using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class HabaneroResultQuery : Result3rdRequest, IRequest<ThirdPartyProductViewModel>
{
}
