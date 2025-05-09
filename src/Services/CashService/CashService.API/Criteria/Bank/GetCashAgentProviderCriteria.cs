namespace AlphaPlus.CashService.API.Criteria.Bank;

using Fanex.Data.Repository;

public class GetCashAgentProviderCriteria : CriteriaBase
{
    public int AgentId { get; set; }

    public int TransType { get; set; }

    public override string GetSettingKey() => "Age_CashAgentProviderSel";

    public override bool IsValid() => AgentId > 0 && TransType > 0;
}