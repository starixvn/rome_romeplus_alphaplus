using AlphaPlus.Core.Models;

namespace AlphaPlus.Tickets.Site.Features.Shared.Requests;

public class RequestBase(IUserSession userSession)
{
    protected IUserSession UserSession { get; } = userSession;

    public int ViewerId { get; } = userSession.CustId;

    public byte ViewerLevel { get; } = userSession.RoleId;

    public string Language { get; } = userSession.Language;
}
