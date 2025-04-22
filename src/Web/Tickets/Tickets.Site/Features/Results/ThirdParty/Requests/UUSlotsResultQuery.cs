using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class UUSlotsResultQuery : Result3rdRequest, IRequest<ThirdPartyProductViewModel>
{
}
