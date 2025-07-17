namespace AlphaPlus.CashService.API.Features.Shared.Requests;

public class BaseActionRequest
{
    public int ActorId { get; set; }
    public int SubAccId { get; set; }
    public string IP { get; set; }
    public string Language { get; set; }

    public int UserId { get; set; }
    public short SiteDBGroupId { get; set; }
    public short CustDBGroupId { get; set; }

    public int ReceivedCustId { get; set; }

    public virtual bool IsValid() => ActorId > 0;
}