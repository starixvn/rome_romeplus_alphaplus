namespace AlphaPlus.CashService.Site.Features.Promotion.Models;
using System.Collections.Generic;

public class PromotionDetailsReport
{
    public PromotionDetailsSummary Summary { get; set; }
    public IEnumerable<PromotionDetails> Details { get; set; }

    public PromotionDetailsReport(PromotionDetailsSummary summary, IEnumerable<PromotionDetails> details)
    {
        Summary = summary;
        Details = details;
    }
}
