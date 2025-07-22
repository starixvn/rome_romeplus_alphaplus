namespace AlphaPlus.CashService.API.Features.Promotion.Requests;

using Shared.Requests;

public class UpdatePromoteTransactionRequest : BaseActionRequest
{
    public int TransId { get; set; }
    public int CustId { get; set; }
    public decimal Amount { get; set; }
    public int StatusId { get; set; }
    public string Remark { get; set; }
}