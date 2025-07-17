namespace AlphaPlus.CashService.API.Features.Promotion.Models;

using Shared.Requests;

public class EditPromotionRequest : BaseActionRequest
{
    public int PromotionId { get; set; }
    public List<PromotionLanguageSetting> Contents { get; set; }
    public bool Enabled { get; set; }
}