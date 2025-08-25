namespace AlphaPlus.CashService.API.Features.Promotion.Criteria;

using Fanex.Data;
using Fanex.Data.Repository;

public class AddPromotionCriteria : NonQueryCommand
{
    [SpParam(SqlParamName = "Type")]
    public int PromotionType { get; set; }

    public bool Enabled { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public string PromotionDetails { get; set; }
    public string PromotionContents { get; set; }

    [SpParam(SqlParamName = "AgentID")]
    public int CreatedBy { get; set; }

    public string IP { get; set; }
    public int SubAccId { get; set; }
    public string Language { get; set; }
    public int SiteDbGroupId { get; set; }
    public int ErrCode { get; set; }
    public string ErrMsg { get; set; }

    public override string GetSettingKey() => "Age_CashService_CashPromotionIns";

    public override bool IsValid() => true;
}