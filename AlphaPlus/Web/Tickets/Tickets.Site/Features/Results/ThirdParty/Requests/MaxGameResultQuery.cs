using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class MaxGameResultQuery : Result3rdQuery, IRequest<ThirdPartyProductViewModel>
{
}