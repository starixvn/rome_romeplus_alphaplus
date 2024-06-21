using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class SabaxDResultQuery : Result3rdQuery, IRequest<ThirdPartyProductViewModel>
{
}