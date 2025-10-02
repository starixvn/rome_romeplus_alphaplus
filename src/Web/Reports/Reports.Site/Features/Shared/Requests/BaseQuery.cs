using AlphaPlus.Core.Models;

namespace AlphaPlus.Reports.Site.Features.Shared.Requests;

public class BaseQuery
{
    private const string CustomerDatelimit = "CustomerDateLimit";
    private const string GameEnableJackpot = "GameEnable_Jackpot";

    public BaseQuery() : this(new UserSessionExt())
    {
    }

    internal BaseQuery(IUserSession viewer)
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

        SubAccId = viewer.SubAccId;
        UserId = viewer.UserId;
    }

    protected IUserSession Viewer { get; }

    public int ViewerId { get; }

    public byte ViewerLevelId { get; }

    public UserLevel ViewerLevel { get; }

    public string Site { get; }

    public int CurrencyId { get; }

    public string Language { get; }

    public DateTime LimitedDateToShowInfo { get; }

    public IList<int> EnableJackpotProductIds => Viewer.GetProperty<IList<int>>(GameEnableJackpot) ?? [];

    public int CustId { get; set; }

    public string CustName { get; set; }

    public byte CustLevelId { get; set; }

    public UserLevel CustLevel => Enumeration.FromValue<UserLevel>(CustLevelId);

    public string BreadcrumbsJson { get; set; }

    public DateTime MinDate { get; protected set; } = DateTime.MinValue;

    public bool IsFilterVisible { get; set; } = true;
    public int SubAccId { get; }
    public int UserId { get; }

    public virtual bool IsValid()
       => CustId > 0
       && ViewerId > 0
       && ViewerLevel.IsEqualOrUplineOf(CustLevel);
}