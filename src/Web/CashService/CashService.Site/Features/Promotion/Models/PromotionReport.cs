using AlphaPlus.Core.Extensions;

namespace AlphaPlus.CashService.Site.Features.Promotion.Models;

public class PromotionReport
{
    public int PromotionId { get; init; }

    public string PromotionTitle { get; init; }

    public byte PromotionType { get; init; }

    public DateTime FromDate { get; init; }

    public DateTime ToDate { get; init; }

    public string Period => $"{FromDate.ToShortDateWithSlash()} - {ToDate.ToShortDateWithSlash()}";

    public bool Enabled { get; init; }

    public int TotalRequest { get; init; }

    public int TotalApproved { get; init; }

    public int TotalRejected { get; init; }

    public decimal TotalAmount { get; init; }
}
