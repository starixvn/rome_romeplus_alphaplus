namespace AlphaPlus.CashService.Site.Features.Promotion.Requests;

using Shared.Requests;

public class ApprovePromotionTransactionRequest : BaseActionRequest
{
    public int TransId { get; set; }
    public int CustId { get; set; }
    public decimal Amount { get; set; }
    public int StatusId { get; set; }
    public string Remark { get; set; }
}