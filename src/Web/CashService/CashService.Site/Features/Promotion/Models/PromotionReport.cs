namespace AlphaPlus.CashService.Site.Features.Promotion.Models;

public class PromotionReport
{
    public int PromotionId { get; set; }
    public string PromotionTitle { get; set; }
    public string PromotionType { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public int TotalRequest { get; set; }
    public int TotalApproved { get; set; }
    public int TotalRejected { get; set; }
    public decimal TotalBonus { get; set; }
    public int Status { get; set; }
}
