using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.WinLoss;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.WinLoss;

public class WinLossWeeksQuery : WinLossNewQuery, IRequest<IEnumerable<WinLossWeekViewModel>>
{
}
