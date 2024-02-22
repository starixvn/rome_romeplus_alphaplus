using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Models;
using AlphaPlus.Tickets.Site.Features.BetLists.Models;
using _Enumeration = AlphaPlus.Core.Models.Enumeration;
using _UserLevel = AlphaPlus.Core.Models.UserLevel;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests;

public class BetListBaseQuery
{
    public BetListBaseQuery() : this(null)
    {
    }

    protected BetListBaseQuery(IUser viewer)
    {
        viewer = viewer ?? new User(new UserSessionExt());
        CustId = viewer.Id;
        Site = viewer.Site;
        CurrencyId = viewer.CurrencyId;
        CustName = viewer.Name;

        ViewerId = viewer.Id;
        ViewerLevelId = viewer.LevelId;
        ViewerLevel = _Enumeration.FromValue<_UserLevel>(ViewerLevelId);
        ViewerIdOrViewerAdminId = viewer.IsInternal ? viewer.AdminId : viewer.Id;
        AdminId = viewer.AdminId;
        SubAccId = viewer.SubAccId;
        DisplayVNIP = viewer.IsAdminCanViewVNIP;
        Language = viewer.Language;
        IsEnabledJackpot = viewer.IsEnabledJackpot;

        LimitedDateToShowInfo = viewer.LimitedDateToShowInfo;
        IsInternal = viewer.IsInternal;
    }

    protected IUserSession Viewer { get; }

    public int CustId { get; set; }

    public string Site { get; set; }

    public int CurrencyId { get; set; }

    public string CustName { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public string ProductIds { get; set; }

    public string ResourceIds { get; set; }

    public string Language { get; set; }

    public int ViewerId { get; }

    public byte ViewerLevelId { get; }

    public int ViewerIdOrViewerAdminId { get; }

    public int AdminId { get; }

    public int SubAccId { get; }

    public bool DisplayVNIP { get; }

    public _UserLevel ViewerLevel { get; }

    public string Type { get; set; }

    public bool IsHistoricReport { get; set; }

    public bool IsEnabledJackpot { get; set; }

    public DateTime LimitedDateToShowInfo { get; set; }

    public bool IsInternal { get; }

    public bool Is789Y => AppSettings.SiteLabel.Equals(AppSettings.SiteLabel789y);
}