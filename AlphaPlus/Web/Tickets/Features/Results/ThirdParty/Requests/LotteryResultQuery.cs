using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class LotteryResultQuery : KenoLotteryTableGameResultQuery, IRequest<LotteryResultViewModel>
{
}
