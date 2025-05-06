using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class AdvantPlayResultQuery : Result3rdRequest, IRequest<ThirdPartyProductViewModel>
{
}
