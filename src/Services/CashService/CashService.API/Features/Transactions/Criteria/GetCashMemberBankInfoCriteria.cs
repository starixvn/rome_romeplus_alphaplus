namespace AlphaPlus.CashService.API.Features.Transactions.Criteria;

using Fanex.Data;
using Fanex.Data.Repository;

public class GetCashMemberBankInfoCriteria : CriteriaBase
{
    public int CustId { get; set; }

    [SpParam(Ignored = true)]
    public int CustDbGroupId { get; set; }

    public override string GetSettingKey() => $"CustDb_{CustDbGroupId}_Age_CashMemberBankInfoSel";

    public override bool IsValid() => CustId > 0;
}