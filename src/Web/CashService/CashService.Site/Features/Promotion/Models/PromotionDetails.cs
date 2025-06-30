namespace AlphaPlus.CashService.Site.Features.Promotion.Models;

public class PromotionDetails
{
    public string Username { get; set; }
    public string Nickname { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PromotionType { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public decimal BonusAmount { get; set; }
    public DateTime LastUpdated { get; set; }
    public int Status { get; set; }
}
