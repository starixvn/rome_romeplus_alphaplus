namespace AlphaPlus.CashService.API.Features.Promotion.Criteria;

using Fanex.Data;
using Fanex.Data.Repository;

public class DeletePromotionCriteria : NonQueryCommand
{
    public int PromotionId { get; set; }
    public int SiteDbGroupId { get; set; }
    public int AgentId { get; set; }
    public int SubAccId { get; set; }
    public string IP { get; set; }
    public string Language { get; set; }
    public int ErrCode { get; set; }
    public string ErrMsg { get; set; } = string.Empty;

    [SpParam(Ignored = true)]
    public int CustDbGroupId { get; set; }

    public override string GetSettingKey() => $"CustDb_{CustDbGroupId}_Age_CashService_Promotion_Delete";

    public override bool IsValid() => PromotionId > 0 && AgentId > 0;
}