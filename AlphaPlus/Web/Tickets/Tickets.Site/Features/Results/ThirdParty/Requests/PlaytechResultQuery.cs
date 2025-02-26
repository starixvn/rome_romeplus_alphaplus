using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class PlaytechResultQuery : Result3rdQuery, IRequest<ThirdPartyProductViewModel>
{
}
