using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.Shared.Models;

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
        CustId = viewer.CustId;
        CustName = viewer.UserName;
        Site = viewer.Site;
        CurrencyId = viewer.CurrencyId;
        CustLevelId = viewer.RoleId;
        ViewerId = viewer.CustId;
        ViewerLevelId = viewer.RoleId;
        Language = viewer.Language;
        LimitedDateToShowInfo = viewer.GetProperty<DateTime>(CustomerDatelimit);
    }

    protected IUserSession Viewer { get; }

    public int CustId { get; set; }

    public string CustName { get; set; }

    public string Site { get; set; }

    public int CurrencyId { get; set; }

    public byte CustLevelId { get; set; }

    public UserLevel CustLevel => Enumeration.FromValue<UserLevel>(CustLevelId);

    public int ViewerId { get; }

    public byte ViewerLevelId { get; }

    public UserLevel ViewerLevel => Enumeration.FromValue<UserLevel>(ViewerLevelId);

    public IEnumerable<Breadcrumb> BreadcrumbLevel { get; set; } = Enumerable.Empty<Breadcrumb>();

    public string Language { get; }

    public DateTime LimitedDateToShowInfo { get; }

    public bool IsPostBack { get; set; }

    public bool IsHistoricReport { get; set; }

    public bool IsSupportHistoricReport { get; set; }

    public DateTime MinDate { get; set; }

    public virtual bool IsValid()
       => CustId > 0
       && CustLevelId > 0
       && !string.IsNullOrWhiteSpace(CustName)
       && ViewerId > 0
       && ViewerLevel.IsEqualOrUplineOf(CustLevel)
       && !string.IsNullOrWhiteSpace(Language);
}