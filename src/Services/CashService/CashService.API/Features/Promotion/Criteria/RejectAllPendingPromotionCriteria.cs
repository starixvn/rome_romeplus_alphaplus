namespace AlphaPlus.CashService.API.Features.Promotion.Criteria;

using Fanex.Data;
using Fanex.Data.Repository;

public class RejectAllPendingPromotionCriteria : CriteriaBase
{
    public int AgentId { get; set; }
    public int SubAccId { get; set; }
    public int PromotionId { get; set; }
    public string IP { get; set; }
    public int ErrCode { get; set; }
    public string ErrMsg { get; set; }
    public string ErrExtraInfo { get; set; }

    [SpParam(Ignored = true)]
    public int SiteDbGroupId { get; set; }

    public override string GetSettingKey() => $"SiteDb_{SiteDbGroupId}_Age_CashPromotion_RejectAll_ByPromotionID";

    public override bool IsValid() => AgentId > 0 && PromotionId > 0;
}