using AlphaPlus.Core.Models;

namespace AlphaPlus.CashService.Site.Features.Shared.Requests;

public class BaseActionRequest
{
    public virtual void SetUserSession(IUserSession userSession)
    {
        ActorId = userSession.CustId;

        SubAccId = userSession.SubAccId;
        IP = userSession.UserLoginIp;
        Language = userSession.LangKey;
        UserId = userSession.UserId;
        ReceivedCustId = userSession.CustId;
        
        SiteDBGroupId = (short)userSession.SiteDBGroupId;
        CustDBGroupId = (short)userSession.CustDBGroupId;
    }

    public void SetLimitedDateToShowInfo(IUserSession userSession)
    {
        const string CustomerDatelimit = "CustomerDateLimit";
        LimitedDateToShowInfo = userSession.GetProperty<DateTime>(CustomerDatelimit);
    }

    public int ActorId { get; set; }
    public int SubAccId { get; set; }
    public string IP { get; set; }
    public string Language { get; set; }
    public int UserId { get; set; }

    public int ReceivedCustId { get; set; }

    public short SiteDBGroupId { get; set; }
    public short CustDBGroupId { get; set; }

    public DateTime LimitedDateToShowInfo { get; set; }
}