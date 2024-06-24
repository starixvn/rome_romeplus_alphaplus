using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Tickets.Site.App_GlobalResources;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using _UserLevel = AlphaPlus.Core.Models.UserLevel;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels;

public class BetListBaseViewModel(
    BetListBaseQuery message,
    BetListData betListData,
    IBetListHTMLBuilder builder,
    ITicketHelper ticketHelper,
    bool showMemberName = false)
{
    public BetListData BetListData { get; } = betListData;

    public IBetListHTMLBuilder Builder { get; } = builder;

    public ITicketHelper TicketHelper { get; } = ticketHelper;

    public bool ShowMemberName { get; } = showMemberName;

    public int CustId { get; } = message.CustId;

    public string CustName { get; } = message.CustName;

    public byte ViewerLevelId { get; } = message.ViewerLevelId;

    public _UserLevel ViewerLevel { get; } = message.ViewerLevel;

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
        ? $"{Resources.memberBetList} {FromDate.ToShortDateWithSlash()} --> {ToDate.ToShortDateWithSlash()} - {CustName}"
        : Resources.memberBetList;

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

            return totalColumn + (ShowMemberName ? 1 : 0);
        }
    }

    // TODO: will implement here
    public virtual bool CanViewScoreMap(ITicket ticket) => false;
}