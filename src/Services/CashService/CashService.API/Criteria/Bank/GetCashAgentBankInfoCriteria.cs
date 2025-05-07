namespace AlphaPlus.CashService.API.Criteria.Bank;

using Fanex.Data.Repository;

public class GetCashAgentBankInfoCriteria : CriteriaBase
{
    public int AgentId { get; set; }
    public int TransType { get; set; }

    public override string GetSettingKey() => "Age_CashAgentBankInfoSel";

    public override bool IsValid() => AgentId > 0 && TransType > 0;
}