using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.Promotion.Criteria;

public class GetPromotionCriteria(int promotionType, int status, int siteDBGroupId, int agentId) : CriteriaBase
{
    public int PromotionType { get; set; } = promotionType;

    public int Status { get; set; } = status;

    public int SiteDBGroupId { get; set; } = siteDBGroupId;

    public int AgentId { get; set; } = agentId;

    public override string GetSettingKey() => "Age_CashService_CashPromotionSel";

    public override bool IsValid() => AgentId > 0;

}
