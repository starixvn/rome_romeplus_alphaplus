using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Shared.AppResources.Betlist.Site;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.Utils;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels;

public class BetListBaseViewModel(
    BetListBaseQuery message,
    BetListData betListData,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper,
    bool showMemberName = false,
    bool supportRiskAmount = false)
{
    public BetListData BetListData { get; } = betListData;

    public IBetListHTMLBuilder Builder { get; } = builder;

    public ITicketHelper TicketHelper { get; } = ticketHelper;

    public bool ShowMemberName { get; } = showMemberName;

    public bool SupportRiskAmount { get; } = supportRiskAmount;

    public int CustId { get; } = message.CustId;

    public string CustName { get; } = message.CustName;

    public byte ViewerLevelId { get; } = message.ViewerLevelId;

    public UserLevel ViewerLevel { get; } = message.ViewerLevel;

    public int ViewerIdOrViewerAdminId { get; } = message.ViewerIdOrViewerAdminId;

    public DateTime FromDate { get; } = message.FromDate;

    public DateTime ToDate { get; } = message.ToDate;

    public string ProductIds { get; } = message.ProductIds;

    public string ResourceIds { get; } = message.ResourceIds;

    public string BetListType { get; } = message.Type;

    public bool DisplayVNIP { get; } = message.DisplayVNIP;

    public string Title => GetTitle();

    protected virtual string GetTitle()
        => HasDateParam()
        ? $"{Resources.memberBetList} {FromDate.ToShortDateWithSlash()} --> {ToDate.ToShortDateWithSlash()} - {CustName}".ToUpperInvariant()
        : Resources.memberBetList.ToUpperInvariant();

    protected bool HasDateParam() => FromDate != DateTime.MinValue && ToDate != DateTime.MinValue;

    public int TotalColumn
    {
        get
        {
            var totalColumn = 0;
            if (ViewerLevel.IsAgentLevel())
            {
                totalColumn = 9;
            }
            else if (ViewerLevel.IsMasterLevel())
            {
                totalColumn = 10;
            }
            else if (ViewerLevel.IsSuperLevel())
            {
                totalColumn = 11;
            }

            return totalColumn + (ShowMemberName ? 1 : 0) + (SupportRiskAmount ? 2 : 0);
        }
    }

    public virtual bool CanViewScoreMap(ITicket ticket) => false;

    public string GetSMARiskAmount(TicketDto ticket)
    {
        var memberWin = ticket.MemberWin;

        if (!memberWin.HasValue)
        {
            return Resources.Platform_NA;
        }

        var amount = 0M;
        if (ViewerLevel.IsSuperLevel())
        {
            amount = memberWin.Value * ticket.SuperPositionTaking;
        }
        else if (ViewerLevel.IsMasterLevel())
        {
            amount = memberWin.Value * ticket.MasterPositionTaking;
        }
        else
        {
            amount = memberWin.Value * ticket.AgentPositionTaking;
        }

        return ConvertByBetType.Winloss(amount);
    }
}