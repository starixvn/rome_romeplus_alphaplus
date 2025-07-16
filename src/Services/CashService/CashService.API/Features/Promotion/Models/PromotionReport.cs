namespace AlphaPlus.CashService.API.Features.Promotion.Models;

public class PromotionReport
{
    public int PromotionId { get; init; }

    public string PromotionTitle { get; init; }

    public byte PromotionType { get; init; }

    public DateTime FromDate { get; init; }

    public DateTime ToDate { get; init; }

    public bool Enabled { get; init; }

    public bool IsSoftDeleted { get; init; }

    public int TotalRequest { get; init; }

    public int TotalApproved { get; init; }

    public int TotalRejected { get; init; }

    public decimal TotalAmount { get; init; }
}