namespace AlphaPlus.CashService.API.Features.Promotion.Models;

public class PromotionDetailsReport(
    PromotionDetailsSummary summary,
    IEnumerable<PromotionDetails> promotionDetails)
{
    public PromotionDetailsSummary Summary { get; } = summary;

    public IEnumerable<PromotionDetails> PromotionDetails { get; } = promotionDetails;
}