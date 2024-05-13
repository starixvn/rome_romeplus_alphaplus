using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.Keno;

public class KenoResultQuery : KenoLotteryTableGameResultQuery, IRequest<KenoResultViewModel>
{
}