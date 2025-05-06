using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class AdvantPlayMiniResultQuery : Result3rdRequest, IRequest<ThirdPartyProductViewModel>
{
}
