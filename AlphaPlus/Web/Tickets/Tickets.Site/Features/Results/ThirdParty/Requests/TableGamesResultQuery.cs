using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class TableGamesResultQuery : KenoLotteryTableGameResultQuery, IRequest<KenoLotteryTableGameResultViewModel>
{
}
