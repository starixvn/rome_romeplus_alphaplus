using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Promotion;

public class GetPromotionCriteria : CriteriaBase
{
    public int PromotionType { get; set; }

    public string Status { get; set; }      

    public override string GetSettingKey() => string.Empty;


    public override bool IsValid() => true;
    
}
