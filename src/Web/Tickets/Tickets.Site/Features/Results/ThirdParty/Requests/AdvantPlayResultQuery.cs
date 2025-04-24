using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class AdvantPlayResultQuery : Result3rdQuery, IRequest<ThirdPartyProductViewModel>
{
}
