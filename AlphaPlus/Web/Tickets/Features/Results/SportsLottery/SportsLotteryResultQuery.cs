using AlphaPlus.Tickets.Site.Features.Results.ThirdParty;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.SportsLottery;

public class SportsLotteryResultQuery : Result3rdQuery, IRequest<SportsLotteryResultViewModel>
{
}