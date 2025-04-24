using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class KenoResultQuery : KenoLotteryTableGameResultQuery, IRequest<KenoLotteryTableGameResultViewModel>
{
}