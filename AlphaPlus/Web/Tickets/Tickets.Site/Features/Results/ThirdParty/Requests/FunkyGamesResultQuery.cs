using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class FunkyGamesResultQuery : Result3rdQuery, IRequest<ThirdPartyProductViewModel>
{
}
