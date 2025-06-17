namespace AlphaPlus.CashService.Site.Features.Transactions.Models;

using Shared.Requests;

public class ProcessDepositTransaction : BaseActionRequest
{
    public string TransId { get; set; }
    public int CustId { get; set; }
    public decimal NewAmount { get; set; }
    public string Remark { get; set; }
}