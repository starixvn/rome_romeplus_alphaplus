namespace AlphaPlus.CashService.Site.Features.Promotion.Models;

using Shared.Requests;

public class DeletePromotionRequest : BaseActionRequest
{
    public int PromotionId { get; set; }
}