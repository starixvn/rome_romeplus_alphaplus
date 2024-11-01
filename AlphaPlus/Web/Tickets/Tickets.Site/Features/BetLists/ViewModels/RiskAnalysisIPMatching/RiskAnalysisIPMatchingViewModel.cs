using AlphaPlus.Shared.AppResources.Betlist.Services;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.RiskAnalysisIPMatching;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.WinLoss;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.RiskAnalysisIPMatching;

public class RiskAnalysisIPMatchingViewModel(
    RiskAnalysisIPMatchingQuery message,
    BetListData betListData,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper) : WinLossViewModel(message, betListData, builder, ticketHelper, false, false, true, excludingTicketStatus, supportRiskAmount: true)
{
    public string IP { get; } = message.IP;

    public string Date { get; } = message.Date.ToShortDateWithSlash();

    private static readonly string[] excludingTicketStatus = [BetStatus.Running, BetStatus.Waiting];

    protected override string GetTitle() => string.Format(Resources.IPMatchingTitle, Date, IP);
}
