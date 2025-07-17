namespace AlphaPlus.CashService.API.Features.Promotion.Models;

public class PromotionDetailsSummary(decimal totalBonus, int totalApproved, int totalRejected)
{
    public decimal TotalBonus { get; } = totalBonus;

    public int TotalApproved { get; } = totalApproved;

    public int TotalRejected { get; } = totalRejected;
}
