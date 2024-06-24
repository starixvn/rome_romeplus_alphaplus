using AlphaPlus.BetLists.AppResources;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Core.Models;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.JackpotDetails;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.WinLoss;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.JackpotDetails;

public class JackpotDetailsViewModel(
    JackpotDetailsQuery message,
    BetListData betListData,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper) : WinLossViewModel(message, betListData, builder, ticketHelper, false)
{
    public int ProductId { get; } = message.ProductId;

    protected override string GetTitle()
        => ProductIdConstants.Pragmatic == ProductId
        ? Resources.PPJackpotWonDetails
        : Resources.ProgressiveJackpotWonDetails;

    public override IDictionary<string, string> GetExcelParams()
        => new Dictionary<string, string>
        {
            [nameof(CustId)] = CustId.ToString(),
            [nameof(message.TransId)] = message.TransId.ToString(),
            [nameof(message.BetType)] = message.BetType.ToString(),
            [nameof(ProductId)] = ProductId.ToString()
        };
}