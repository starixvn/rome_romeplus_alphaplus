namespace AlphaPlus.CashService.API.Features.Promotion.Models;

using Shared.Requests;

public class PromotionRequest : BaseActionRequest
{
    public int PromotionType { get; set; }
    public int StatusId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public List<PromotionProductSetting> ProductSettings { get; set; }

    public List<PromotionLanguageSetting> Contents { get; set; }
    public bool Enabled { get;  set; }    
}