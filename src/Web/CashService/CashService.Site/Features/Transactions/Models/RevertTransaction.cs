namespace AlphaPlus.CashService.Site.Features.Transactions.Models;

using Shared.Requests;

public class RevertTransaction : BaseActionRequest
{
    public int CustId { get; set; }
    public long TransId { get; set; }
    public string Remark { get; set; }
}