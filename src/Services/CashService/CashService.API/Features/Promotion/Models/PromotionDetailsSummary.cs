namespace AlphaPlus.CashService.API.Features.Promotion.Models;

public class PromotionDetailsSummary
{
    public decimal TotalBonus { get; init; }

    public int TotalApproved { get; init; }

    public int TotalRejected { get; init; }
}
