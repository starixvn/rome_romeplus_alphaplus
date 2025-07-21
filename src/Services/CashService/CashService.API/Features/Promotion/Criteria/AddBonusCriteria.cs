namespace AlphaPlus.CashService.API.Features.Promotion.Criteria;

using Fanex.Data;
using Fanex.Data.Repository;

public class AddBonusCriteria : CriteriaBase
{
    public int CustId { get; set; }
    public int UplineId { get; set; }
    public int SubAccId { get; set; }
    public decimal AgentGivenCredit { get; set; }
    public decimal CreditMembers_GivenCredit { get; set; }
    public decimal ExchangeRate { get; set; }
    public decimal Amount { get; set; }
    public string IP { get; set; }
    public string Remark { get; set; }
    public int SiteDbGroupId { get; set; }

    [SpParam(Ignored = true)]
    public int CustDbGroupId { get; set; }

    public string CashAccumCreditTbl { get; set; }
    public int ErrCode { get; set; }
    public string ErrMsg { get; set; }

    public override string GetSettingKey() => $"SiteDb_{SiteDbGroupId}_Age_CashPromotion_ManualAddBonus";

    public override bool IsValid() => true;
}