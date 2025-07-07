using AlphaPlus.Core.Models;

namespace AlphaPlus.CashService.Site.Features.Shared.Requests;

public class BaseActionRequest
{
    public BaseActionRequest SetUserSession(IUserSession userSession)
    {
        ActorId = userSession.CustId;

        SubAccId = userSession.SubAccId;
        IP = userSession.UserLoginIp;
        Language = userSession.LangKey;
        UserId = userSession.UserId;

        SiteDBGroupId = userSession.SiteDBGroupId;
        CustDBGroupId = userSession.CustDBGroupId;

        return this;
    }

    public BaseActionRequest SetLimitedDateToShowInfo(IUserSession userSession)
    {
        const string CustomerDatelimit = "CustomerDateLimit";
        LimitedDateToShowInfo = userSession.GetProperty<DateTime>(CustomerDatelimit);

        return this;
    }

    public int ActorId { get; set; }
    public int SubAccId { get; set; }
    public string IP { get; set; }
    public string Language { get; set; }
    public int UserId { get; set; }

    public int SiteDBGroupId { get; set; }
    public int CustDBGroupId { get; set; }

    public DateTime LimitedDateToShowInfo { get; set; }
}