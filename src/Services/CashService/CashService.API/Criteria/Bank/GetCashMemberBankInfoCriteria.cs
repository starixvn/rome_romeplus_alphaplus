namespace AlphaPlus.CashService.API.Criteria.Bank;

using Fanex.Data.Repository;

public class GetCashMemberBankInfoCriteria : CriteriaBase
{
    public int CustId { get; set; }

    public override string GetSettingKey() => "Age_CashMemberBankInfoSel";

    public override bool IsValid() => CustId > 0;
}