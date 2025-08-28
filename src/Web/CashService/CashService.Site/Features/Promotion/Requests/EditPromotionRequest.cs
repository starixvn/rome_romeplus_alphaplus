namespace AlphaPlus.CashService.Site.Features.Promotion.Requests;

using AlphaPlus.CashService.Site.Features.Shared.Requests;

public class EditPromotionRequest : BaseActionRequest
{
    public int PromotionId { get; init; }
    public bool Enabled { get; init; }
    public bool? OriginalEnabled { get; init; }

    public List<PromotionLanguageSetting> Contents { get; set; }
    public List<PromotionLanguageSetting> OriginalContents { get; set; }

    public bool IsValid()
    {
        return true;
    }
}