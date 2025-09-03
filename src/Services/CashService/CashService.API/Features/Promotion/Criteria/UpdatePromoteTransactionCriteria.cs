namespace AlphaPlus.CashService.API.Features.Promotion.Criteria;

using Fanex.Data;
using Fanex.Data.Repository;

public class UpdatePromoteTransactionCriteria : CriteriaBase
{
    public int CustId { get; set; }
    public int AgentId { get; set; }
    public int SubAccId { get; set; }
    public long TransId { get; set; }
    public int StatusId { get; set; }

    [SpParam(SqlParamName = "New_Amount")]
    public decimal NewAmount { get; set; }

    public decimal AgentGivenCredit { get; set; }

    [SpParam(SqlParamName = "CreditMembers_GivenCredit")]
    public decimal CreditMembersGivenCredit { get; set; }

    public string IP { get; set; }
    public string Remark { get; set; }
    public string CashAccumCreditTbl { get; set; }

    [SpParam(Ignored = true)]
    public int SiteDbGroupId { get; set; }

    public int ErrCode { get; set; }
    public string ErrMsg { get; set; }
    public decimal? AgentAvailableCredit { get; set; }

    public override string GetSettingKey() => $"SiteDb_{SiteDbGroupId}_Age_CashPromotion_UpdateTransStatus";

    public override bool IsValid() => true;
}