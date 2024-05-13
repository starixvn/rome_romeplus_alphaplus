using AlphaPlus.Tickets.Site.Features.Results.Keno;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.Lottery;

public class LotteryResultQuery : KenoLotteryTableGameResultQuery, IRequest<LotteryResultViewModel>
{
}
