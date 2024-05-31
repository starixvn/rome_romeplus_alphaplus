using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Tickets.Site.App_GlobalResources;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using _UserLevel = AlphaPlus.Core.Models.UserLevel;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels;

public class BetListBaseViewModel
{
    private static readonly IDictionary<_UserLevel, int> TotalColumnByViewLevelId = new Dictionary<_UserLevel, int>
    {
        [_UserLevel.Agent] = 9,
        [_UserLevel.Master] = 10,
        [_UserLevel.Super] = 11
    };

    protected readonly BetListBaseQuery baseQuery;

    protected BetListBaseViewModel(
        BetListBaseQuery query,
        BetListData betListData,
        IBetListHTMLBuilder builder,
        ITicketHelper ticketHelper,
        bool showMemberName = false)
    {
        baseQuery = query;
        CustName = query.CustName;
        FromDate = query.FromDate;
        ToDate = query.ToDate;
        BetListData = betListData;
        Builder = builder;
        TicketHelper = ticketHelper;
        ProductsId = baseQuery.ProductIds;
        ResourceIds = baseQuery.ResourceIds;
        BetListType = baseQuery.Type;
        CustId = baseQuery.CustId;
        IsHistoricReport = baseQuery.IsHistoricReport;
        ViewerIdOrViewerAdminId = baseQuery.ViewerIdOrViewerAdminId;
        ViewerLevelId = baseQuery.ViewerLevelId;
        ViewerLevel = baseQuery.ViewerLevel;
        DisplayVNIP = baseQuery.DisplayVNIP;
        Title = GetTitle(query).ToUpperInvariant();
        TotalColumn = TotalColumnByViewLevelId[ViewerLevel] + (showMemberName ? 1 : 0);
        ShowMemberName = showMemberName;
    }

    public string CustName { get; }

    public int CustId { get; }

    public DateTime FromDate { get; }

    public DateTime ToDate { get; }

    public BetListData BetListData { get; }

    public IBetListHTMLBuilder Builder { get; }

    public ITicketHelper TicketHelper { get; }

    public bool DisplayVNIP { get; }

    public string ProductsId { get; }

    public string ResourceIds { get; }

    public string BetListType { get; }

    public bool IsHistoricReport { get; }

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

    public int ViewerIdOrViewerAdminId { get; }

    public byte ViewerLevelId { get; }

    public _UserLevel ViewerLevel { get; }

    public string Title { get; protected set; }

    public int TotalColumn { get; }

    public bool ShowMemberName { get; }

    public string GetTitle(BetListBaseQuery query)
        => HasDateParam(query)
        ? $"{Resources.memberBetList} {query.FromDate.ToShortDateString()} --> {query.ToDate.ToShortDateString()} - {query.CustName}"
        : Resources.memberBetList;

    public bool HasDateParam(BetListBaseQuery query)
        => query.FromDate != DateTime.MinValue && query.ToDate != DateTime.MinValue;

    // TODO: will implement here
    public virtual bool CanViewScoreMap(ITicket ticket) => false;
}