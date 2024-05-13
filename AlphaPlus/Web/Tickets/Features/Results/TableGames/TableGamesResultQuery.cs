using AlphaPlus.Tickets.Site.Features.Results.Keno;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.TableGames;

public class TableGamesResultQuery : KenoLotteryTableGameResultQuery, IRequest<TableGamesResultViewModel>
{
}
