namespace AlphaPlus.CashService.Site.Features.Promotion.Models;

public class PromotionReport
{
    public int PromotionId { get; init; }
    public string PromotionTitle { get; init; }
    public string PromotionType { get; init; }
    public string DataRange { get; init; }
    public int TotalRequest { get; init; }
    public int TotalApproved { get; init; }
    public int TotalRejected { get; init; }
    public decimal TotalBonus { get; init; }
    public bool Status { get; init; }
}
