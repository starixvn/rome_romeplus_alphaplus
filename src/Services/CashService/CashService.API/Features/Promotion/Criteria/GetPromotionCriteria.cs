using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.Promotion.Criteria;

public class GetPromotionCriteria : CriteriaBase
{
    public GetPromotionCriteria(int promotionType, string status)
    {
        PromotionType = promotionType;
        Status = status;
    }

    public int PromotionType { get; set; }

    public string Status { get; set; }      

    public override string GetSettingKey() => "Age_Promotion_GetPromotion";


    public override bool IsValid() => true;
    
}
