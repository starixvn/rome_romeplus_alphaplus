using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.Promotion.Criteria;

public class GetPromotionByIdCriteria(int promotionId, int siteDBGroupId, int agentId) : CriteriaBase
{
    public int PromotionId { get; set; } = promotionId;

    public int SiteDBGroupId { get; set; } = siteDBGroupId;

    public int AgentId { get; set; } = agentId;

    public override string GetSettingKey() => "Age_CashService_CashPromotionSelByID";

    public override bool IsValid() => AgentId > 0 && PromotionId > 0;

}
