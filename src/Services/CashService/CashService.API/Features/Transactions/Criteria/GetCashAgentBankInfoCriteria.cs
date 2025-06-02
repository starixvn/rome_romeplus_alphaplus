namespace AlphaPlus.CashService.API.Features.Transactions.Criteria;

using Fanex.Data;
using Fanex.Data.Repository;

public class GetCashAgentBankInfoCriteria : CriteriaBase
{
    public int AgentId { get; set; }
    public int TransType { get; set; }

    [SpParam(Ignored = true)]
    public int CustDbGroupId { get; set; }

    public override string GetSettingKey() => $"CustDb_{CustDbGroupId}_Age_CashAgentBankInfoSel";

    public override bool IsValid() => AgentId > 0 && TransType > 0;
}