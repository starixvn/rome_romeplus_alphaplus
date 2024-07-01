using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.WinLoss;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.WinLoss;

public class WinLossWeekSelectionQuery : WinLossBetListQuery, IRequest<WinLossWeekSelectionViewModel>
{
    public string BreadcrumbsJson { get; set; }
}