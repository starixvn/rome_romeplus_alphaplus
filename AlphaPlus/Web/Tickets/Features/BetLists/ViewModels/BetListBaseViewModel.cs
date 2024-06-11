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
    private static readonly Dictionary<_UserLevel, int> TotalColumnByViewLevelId = new Dictionary<_UserLevel, int>
    {
        [_UserLevel.Agent] = 9,
        [_UserLevel.Master] = 10,
        [_UserLevel.Super] = 11
    };

    protected readonly BetListBaseQuery BaseQuery = message;

    public BetListData BetListData { get; } = betListData;

    public IBetListHTMLBuilder Builder { get; } = builder;

    public ITicketHelper TicketHelper { get; } = ticketHelper;

    public bool ShowMemberName { get; } = showMemberName;

    public int CustId { get; } = message.CustId;

    public string CustName { get; } = message.CustName;

    public byte ViewerLevelId { get; } = message.ViewerLevelId;

    public _UserLevel ViewerLevel { get; } = message.ViewerLevel;

    public int ViewerIdOrViewerAdminId { get; } = message.ViewerIdOrViewerAdminId;

    public bool DisplayVNIP { get; } = message.DisplayVNIP;

    public string Title => GetTitle();

    public int TotalColumn => TotalColumnByViewLevelId[ViewerLevel] + (ShowMemberName ? 1 : 0);

    public DateTime FromDate { get; } = message.FromDate;

    public DateTime ToDate { get; } = message.ToDate;

    public string ProductsId { get; } = message.ProductIds;

    public string ResourceIds { get; } = message.ResourceIds;

    public string BetListType { get; } = message.Type;

    public bool IsHistoricReport { get; } = message.IsHistoricReport;

    // public string ResultsPath
    // {
    //     get
    //     {
    //         if (HttpContext.Current == null)
    //         {
    //             return AppPathHelper.GetAppPath(AppSettings.ResultAppPath, "~");
    //         }
    //         return HttpContext.Current.Session.IsCookieless ?
    //                 $"/site-results/(S({HttpContext.Current.Session.SessionID}))/"
    //                 : AppPathHelper.GetAppPath(AppSettings.ResultAppPath, "~");
    //     }
    // }

    // public string OldMainPath
    // {
    //     get
    //     {
    //         if (HttpContext.Current == null)
    //         {
    //             return MicroSites.SettingUrl("~");
    //         }
    //         return HttpContext.Current.Session.IsCookieless ?
    //             $"/ex-main/(S({HttpContext.Current.Session.SessionID}))/" : MicroSites.SettingUrl("~");
    //     }
    // }

    // public string TotalBetsForecastPath
    // {
    //     get
    //     {
    //         if (HttpContext.Current == null)
    //         {
    //             return AppPathHelper.GetAppPath(AppSettings.TotalBetsForecastAppPath, "~");
    //         }
    //         return HttpContext.Current.Session.IsCookieless ?
    //         $"/ex-totalbetsforecast/(S({HttpContext.Current.Session.SessionID}))/" : AppPathHelper.GetAppPath(AppSettings.TotalBetsForecastAppPath, "~");
    //     }
    // }

    // public string SiteMainPath => AppPathHelper.GetAppPath(AppSettings.MainAppPath, "~");

    protected virtual string GetTitle()
        => HasDateParam()
        ? $"{Resources.memberBetList} {BaseQuery.FromDate.ToShortDateWithSlash()} --> {BaseQuery.ToDate.ToShortDateWithSlash()} - {BaseQuery.CustName}"
        : Resources.memberBetList;

    protected bool HasDateParam()
        => BaseQuery.FromDate != DateTime.MinValue && BaseQuery.ToDate != DateTime.MinValue;

    // TODO: will implement here
    public virtual bool CanViewScoreMap(ITicket ticket) => false;
}