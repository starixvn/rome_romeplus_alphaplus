using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class CGResultQuery : Result3rdQuery, IRequest<ThirdPartyProductViewModel>
{
}
