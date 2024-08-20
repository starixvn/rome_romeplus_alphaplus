using AlphaPlus.Shared.AppResources.Betlist.Services;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.DailyStatement;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.Blocks;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.DailyStatement;

public class DailyStatementViewModel(
    DailyStatementQuery message,
    BetListData betListData,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper,
    HashSet<int> supportScoreMapBetTypeIds = null)
        : BetListBaseViewModel(message, betListData, builder, ticketHelper, false)
{
    private static readonly string[] ExcludedTicketStatus =
    [
        BetStatus.Running,
        BetStatus.Waiting,
        BetStatus.Void,
        BetStatus.Reject,
        BetStatus.Pending
    ];

    public Footer Footer { get; } = new Footer(betListData, message.ViewerLevel, false, ExcludedTicketStatus);

    public bool ShowContributionAmountRoundNote { get; } = message.IsEnabledJackpot;

    protected override string GetTitle() => $"{Resources.DailyStatement} {message.WinLossDate.ToShortDateWithSlash()} - {CustName}";

    public override bool CanViewScoreMap(ITicket ticket)
        => ticket.Status.Equals(BetStatus.Running, StringComparison.OrdinalIgnoreCase)
        && supportScoreMapBetTypeIds?.Contains(ticket.BetTypeId) == true;

    public IDictionary<string, string> GetExcelParams()
        => new Dictionary<string, string>
        {
            [nameof(CustId)] = CustId.ToString(),
            [nameof(CustName)] = CustName,
            [nameof(message.WinLossDate)] = message.WinLossDate.ToShortDateWithSlash(),
            [nameof(message.TransType)] = message.TransType.ToString()
        };
}