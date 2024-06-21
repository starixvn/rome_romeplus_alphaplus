using AlphaPlus.Core.Models;

namespace AlphaPlus.Tickets.Site.Features.Shared.Requests;

public class RequestBase
{
    public RequestBase() : this(null)
    {
    }

    internal RequestBase(IUserSession userSession)
    {
        UserSession = userSession ?? new UserSessionExt();
        ViewerId = UserSession.CustId;
        ViewerLevel = UserSession.RoleId;
        Language = UserSession.Language;
    }

    protected IUserSession UserSession { get; }

    public int ViewerId { get; }

    public byte ViewerLevel { get; }

    public string Language { get; }

    public string SiteName => UserSession.Site;
}
