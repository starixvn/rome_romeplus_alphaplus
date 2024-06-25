using AlphaPlus.Shared.AppResources.Betlist.Services;
using AlphaPlus.BetLists.Shared.Helpers;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.ParlayWinLossByMatch;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.WinLoss;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.ParlayWinLossByMatch;

public class ParlayWinLossByMatchViewModel(
    ParlayWinLossByMatchQuery message,
    BetListData betListData,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper)
        : WinLossViewModel(message, betListData, builder, ticketHelper, message.IsEnabledJackpot, showMemberName: true)
{
    public int BetType { get; set; } = message.BetType;

    public byte CustLevelId { get; set; } = message.CustLevelId;

    protected override string GetTitle()
        => HasDateParam()
        ? $"{CacheRefServiceHelper.GetBetTypeName(BetType)} {FromDate.ToShortDateWithSlash()} --> {ToDate.ToShortDateWithSlash()}"
        : Resources.memberBetList;

    public override IDictionary<string, string> GetExcelParams()
        => new Dictionary<string, string>
        {
            [nameof(CustName)] = CustName,
            [nameof(CustId)] = CustId.ToString(),
            [nameof(ProductIds)] = ProductIds,
            [nameof(FromDate)] = FromDate.ToShortDateWithSlash(),
            [nameof(ToDate)] = ToDate.ToShortDateWithSlash(),
            [nameof(BetType)] = BetType.ToString(),
            [nameof(CustLevelId)] = CustLevelId.ToString()
        };
}