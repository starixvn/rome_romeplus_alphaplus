namespace AlphaPlus.CashService.API.Features.Promotion.Models;

using Shared.Requests;

public class PromotionRequest : BaseActionRequest
{
    public int PromotionTypeId { get; set; }
    public int StatusId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public List<PromotionProductSetting> ProductSettings { get; set; }
    public string Title { get; set; }
    public byte[] Banner { get; set; }
    public string Content { get; set; }
}