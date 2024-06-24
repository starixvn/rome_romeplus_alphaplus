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
        CustName = Viewer.Name;
        Site = Viewer.Site;
        CurrencyId = Viewer.CurrencyId;

        IsInternal = Viewer.IsInternal;
        ViewerId = Viewer.Id;
        ViewerLevelId = Viewer.LevelId;
        ViewerLevel = _Enumeration.FromValue<_UserLevel>(ViewerLevelId);
        ViewerIdOrViewerAdminId = IsInternal ? Viewer.AdminId : Viewer.Id;
        AdminId = Viewer.AdminId;
        SubAccId = Viewer.SubAccId;
        DisplayVNIP = Viewer.IsAdminCanViewVNIP;
        Language = Viewer.Language;
        IsEnabledJackpot = Viewer.IsEnabledJackpot;
    }

    protected IUser Viewer { get; }

    public int CustId { get; set; }

    public string CustName { get; set; }

    public string Site { get; set; }

    public int CurrencyId { get; set; }

    public bool IsInternal { get; }

    public int ViewerId { get; }

    public byte ViewerLevelId { get; }

    public _UserLevel ViewerLevel { get; }

    public int ViewerIdOrViewerAdminId { get; }

    public int AdminId { get; }

    public int SubAccId { get; }

    public bool DisplayVNIP { get; }

    public string Language { get; }

    public bool IsEnabledJackpot { get; }

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public string ProductIds { get; set; } = "*";

    public string ResourceIds { get; set; } = "*";

    public string Type { get; set; }
}