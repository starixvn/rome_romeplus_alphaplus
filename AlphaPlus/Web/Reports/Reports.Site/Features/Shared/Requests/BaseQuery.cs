using AlphaPlus.Core.Models;

namespace AlphaPlus.Reports.Site.Features.Shared.Requests;

public class BaseQuery
{
    private const string CustomerDatelimit = "CustomerDateLimit";

    public BaseQuery() : this(new UserSessionExt())
    {
    }

    protected BaseQuery(IUserSession viewer)
    {
        Viewer = viewer;
        ViewerId = viewer.CustId;
        ViewerLevelId = viewer.RoleId;
        ViewerLevel = Enumeration.FromValue<UserLevel>(ViewerLevelId);

        Site = viewer.Site;
        CurrencyId = viewer.CurrencyId;
        Language = viewer.Language;
        LimitedDateToShowInfo = viewer.GetProperty<DateTime>(CustomerDatelimit);

        CustId = ViewerId;
        CustName = viewer.UserName;
        CustLevelId = ViewerLevelId;
    }

    protected IUserSession Viewer { get; }

    public int ViewerId { get; }

    public byte ViewerLevelId { get; }

    public UserLevel ViewerLevel { get; }

    public string Site { get; }

    public int CurrencyId { get; }

    public string Language { get; }

    public DateTime LimitedDateToShowInfo { get; }

    public int CustId { get; set; }

    public string CustName { get; set; }

    public byte CustLevelId { get; set; }

    public UserLevel CustLevel => Enumeration.FromValue<UserLevel>(CustLevelId);

    public string BreadcrumbsJson { get; set; }

    public DateTime MinDate { get; protected set; } = DateTime.MinValue;

    public bool IsPostBack { get; set; }

    public bool IsFilterVisible { get; set; } = true;

    public virtual bool IsValid()
       => CustId > 0
       && CustLevel.IsNotMemberLevel()
       && ViewerId > 0
       && ViewerLevel.IsEqualOrUplineOf(CustLevel);
}