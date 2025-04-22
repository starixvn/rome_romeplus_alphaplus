using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class SportsLotteryDetailResultQuery : Result3rdRequest, IRequest<ThirdPartyProductViewModel>
{
}