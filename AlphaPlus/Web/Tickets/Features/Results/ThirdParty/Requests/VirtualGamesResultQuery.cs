using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class VirtualGamesResultQuery : KenoLotteryTableGameResultQuery, IRequest<VirtualGamesResultViewModel>
{
}