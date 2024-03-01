using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.Shared.Models;

namespace AlphaPlus.Reports.Site.Features.Shared.Requests;

public class BaseQuery
{
    private const string CustomerDatelimit = "CustomerDateLimit";
    private const string Jackpot = "Jackpot";
    private const string GameEnable = "GameEnable_";

    public BaseQuery() : this(new UserSessionExt())
    {
    }

    protected BaseQuery(IUserSession viewer)
    {
        Viewer = viewer;
        ViewerId = viewer.CustId;
        ViewerLevelId = viewer.RoleId;
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

    public UserLevel ViewerLevel => Enumeration.FromValue<UserLevel>(ViewerLevelId);

    public string Site { get; }

    public int CurrencyId { get; set; }

    public string Language { get; }

    public DateTime LimitedDateToShowInfo { get; }

    public int CustId { get; set; }

    public string CustName { get; set; }

    public byte CustLevelId { get; set; }

    public UserLevel CustLevel => Enumeration.FromValue<UserLevel>(CustLevelId);

    public IEnumerable<Breadcrumb> BreadcrumbLevel { get; set; } = Enumerable.Empty<Breadcrumb>();

    public DateTime MinDate { get; protected set; }

    public bool IsEnabledJackpot => EnableJackpotProductIds.Any();

    public IList<int> EnableJackpotProductIds => GetEnableProductsForGame(Jackpot);

    public bool IsPostBack { get; set; }

    public virtual bool IsValid()
       => CustId > 0
       && CustLevelId > 0
       && !string.IsNullOrWhiteSpace(CustName)
       && ViewerId > 0
       && ViewerLevel.IsEqualOrUplineOf(CustLevel)
       && !string.IsNullOrWhiteSpace(Language);

    private IList<int> GetEnableProductsForGame(string gameName)
        => Viewer.GetProperty<IList<int>>(GameEnable + gameName) ?? [];
}