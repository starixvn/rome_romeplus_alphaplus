namespace AlphaPlus.CashService.Site.Features.Promotion.Models;

public class PromotionDetails
{
    public string Username { get; init; }
    public string Nickname { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string PromotionType { get; init; }
    public string DateRange { get; init; }
    public decimal BonusAmount { get; init; }
    public DateTime LastUpdated { get; init; }
    public byte Status { get; init; }
}
