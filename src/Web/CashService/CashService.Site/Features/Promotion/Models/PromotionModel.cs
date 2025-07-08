namespace AlphaPlus.CashService.Site.Features.Promotion.Models;

public class PromotionModel
{
    public int PromotionId { get; set; }
    public int SiteDbGroupId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public string Period => $"{FromDate:MM/dd/yyyy} - {ToDate:MM/dd/yyyy}";
    public string Provider { get; set; }
    public bool Enabled { get; set; }
    public string ModifiedDate { get; set; }
}