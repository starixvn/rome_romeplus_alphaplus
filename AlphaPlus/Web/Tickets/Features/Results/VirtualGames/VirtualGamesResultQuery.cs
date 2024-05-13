using AlphaPlus.Tickets.Site.Features.Results.Keno;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.VirtualGames;

public class VirtualGamesResultQuery : KenoLotteryTableGameResultQuery, IRequest<VirtualGamesResultViewModel>
{
}