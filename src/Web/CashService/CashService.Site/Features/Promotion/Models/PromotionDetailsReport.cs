namespace AlphaPlus.CashService.Site.Features.Promotion.Models;
using System.Collections.Generic;

public class PromotionDetailsReport(
    PromotionDetailsSummary summary,
    IEnumerable<PromotionDetails> promotionDetails)
{
    public PromotionDetailsSummary Summary { get; } = summary;

    public IEnumerable<PromotionDetails> PromotionDetails { get; } = promotionDetails;
}
