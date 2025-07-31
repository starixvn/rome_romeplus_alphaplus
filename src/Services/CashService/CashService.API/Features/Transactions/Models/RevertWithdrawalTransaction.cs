namespace AlphaPlus.CashService.API.Features.Transactions.Models;

using Shared.Requests;

public class RevertWithdrawalTransaction : BaseActionRequest
{
    public long TransId { get; set; }
    public string Remark { get; set; }
    public int CustId { get; set; }
}