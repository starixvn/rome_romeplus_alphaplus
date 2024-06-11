using AlphaPlus.BetLists.AppResources;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.ParlayWinLossByMatch;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.WinLoss;
using AlphaPlus.Tickets.Site.Features.Shared;
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
        ? $"{CacheRefServiceHelper.GetBetTypeName(BetType)} {message.FromDate.ToShortDateWithSlash()} --> {message.ToDate.ToShortDateWithSlash()}"
        : Resources.memberBetList;

    public IDictionary<string, string> GetExcelParams()
        => new Dictionary<string, string>
        {
            [nameof(CustName)] = CustName,
            [nameof(CustId)] = CustId.ToString(),
            [nameof(ProductsId)] = ProductsId,
            [nameof(FromDate)] = FromDate.ToShortDateWithSlash(),
            [nameof(ToDate)] = ToDate.ToShortDateWithSlash(),
            [nameof(BetType)] = BetType.ToString(),
            [nameof(CustLevelId)] = CustLevelId.ToString()
        };
}