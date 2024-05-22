using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class SportsLotteryResultQuery : Result3rdQuery, IRequest<SportsLotteryResultViewModel>
{
}