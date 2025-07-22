namespace AlphaPlus.CashService.Site.Features.Promotion.Requests;

using Shared.Requests;

public class AddBonusRequest : BaseActionRequest
{
    public int CustId { get; set; }
    public decimal Amount { get; set; }
    public string Remark { get; set; }
}