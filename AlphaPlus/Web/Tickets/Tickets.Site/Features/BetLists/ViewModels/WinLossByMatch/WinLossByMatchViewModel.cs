using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.WinLossByMatch;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.WinLoss;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.WinLossByMatch;

public class WinLossByMatchViewModel(
    WinLossByMatchQuery message,
    BetListData betListData,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper)
        : WinLossViewModel(message, betListData, builder, ticketHelper, message.IsEnabledJackpot, showMemberName: true)
{
    public int BetType { get; } = message.BetType;

    public int LeagueId { get; } = message.LeagueId;

    public int MatchId { get; } = message.MatchId;

    public string Race { get; } = message.Race;

    public int LiveIndicator { get; } = message.LiveIndicator;

    public byte CustLevelId { get; } = message.CustLevelId;

    public override IDictionary<string, string> GetExcelParams()
        => new Dictionary<string, string>
        {
            [nameof(CustName)] = CustName,
            [nameof(CustId)] = CustId.ToString(),
            [nameof(ProductIds)] = ProductIds,
            [nameof(FromDate)] = FromDate.ToShortDateWithSlash(),
            [nameof(ToDate)] = ToDate.ToShortDateWithSlash(),
            [nameof(BetType)] = BetType.ToString(),
            [nameof(LeagueId)] = LeagueId.ToString(),
            [nameof(MatchId)] = MatchId.ToString(),
            [nameof(Race)] = Race,
            [nameof(LiveIndicator)] = LiveIndicator.ToString(),
            [nameof(CustLevelId)] = CustLevelId.ToString()
        };
}