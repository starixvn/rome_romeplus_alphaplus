namespace AlphaPlus.CashService.API.Features.Promotion.Models;

public class PromotionReport
{
    public int PromotionId { get; init; }

    public string PromotionTitle { get; init; }

    public string PromotionType { get; init; }

    public DateTime FromDate { get; init; }

    public DateTime ToDate { get; init; }

    public string DateRange
        => $"{FromDate.ToString("MM/dd/yyyy")} - {ToDate.ToString("MM/dd/yyyy")}";

    public int TotalRequest { get; init; }

    public int TotalApproved { get; init; }

    public int TotalRejected { get; init; }

    public decimal TotalBonus { get; init; }

    public byte Status { get; init; }
}