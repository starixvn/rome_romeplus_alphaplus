using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class PPResultQuery : Result3rdQuery, IRequest<ThirdPartyProductViewModel>
{
}
