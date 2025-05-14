using AlphaPlus.Core.Models;

namespace AlphaPlus.CashService.Site.Features.Shared.Requests;

public class BaseActionRequest
{
    public void SetUserSession(IUserSession userSession)
    {
        ActorId = 1176086;

        // ActorId = userSession.CustId;
        SubAccId = userSession.SubAccId;
        IP = userSession.UserLoginIp;
        Language = userSession.LangKey;
        UserId = userSession.UserId;
        
        SiteDB = userSession.CustDBGroupId;
    }

    public int ActorId { get; set; }
    public int SubAccId { get; set; }
    public string IP { get; set; }
    public string Language { get; set; }
    public int UserId { get; set; }
    public int SiteDB { get; set; }
} 