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
        Viewer = viewer ?? new User(new UserSessionExt());
        CustId = Viewer.Id;
        Site = Viewer.Site;
        CurrencyId = Viewer.CurrencyId;
        CustName = Viewer.Name;

        ViewerId = Viewer.Id;
        ViewerLevelId = Viewer.LevelId;
        ViewerLevel = _Enumeration.FromValue<_UserLevel>(ViewerLevelId);
        ViewerIdOrViewerAdminId = Viewer.IsInternal ? Viewer.AdminId : Viewer.Id;
        AdminId = Viewer.AdminId;
        SubAccId = Viewer.SubAccId;
        DisplayVNIP = Viewer.IsAdminCanViewVNIP;
        Language = Viewer.Language;
        IsEnabledJackpot = Viewer.IsEnabledJackpot;

        LimitedDateToShowInfo = Viewer.LimitedDateToShowInfo;
        IsInternal = Viewer.IsInternal;
    }

    protected IUser Viewer { get; }

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