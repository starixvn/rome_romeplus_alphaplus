namespace AlphaPlus.CashService.Site.Features.Transactions.Models;

using Shared.Requests;

public class RevertTransaction : BaseActionRequest
{
    public long TransId { get; set; }
    public string Remark { get; set; }
}