using System.Text.Json;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.WinLoss;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.WinLoss;

public class WinLossWeekSelectionViewModel(
    WinLossWeekSelectionQuery message,
    IEnumerable<WinLossWeekViewModel> weeks,
    BetListData betListData,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper,
    bool showContributionAmountRoundNote,
    bool isGroupHeadline = false,
    bool showMemberName = false,
    string[] excludingTicketStatus = null)
        : WinLossViewModel(message, betListData, builder, ticketHelper, showContributionAmountRoundNote, isGroupHeadline, showMemberName, excludingTicketStatus)
{
    public IEnumerable<WinLossWeekViewModel> Weeks { get; } = weeks;

    public IEnumerable<Breadcrumb> BreadcrumbLevels { get; }
        = string.IsNullOrWhiteSpace(message.BreadcrumbsJson)
        ? []
        : JsonSerializer.Deserialize<IEnumerable<Breadcrumb>>(message.BreadcrumbsJson);
}
