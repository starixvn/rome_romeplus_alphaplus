using AlphaPlus.CashService.API.Features.Shared.Criteria;
using Fanex.Data;

namespace AlphaPlus.CashService.API.Criteria.Transactions;

public class SyncCashAccumCreditCommand() : BaseActionCommand(0, 0, "", "")
{
    public string CashAccumCreditTbl { get; set; }

    [SpParam(Ignored = true)]
    public int CustDbGroupId { get; set; }

    public override string GetSettingKey() => $"Cust_{CustDbGroupId}_Age_CashAccumCredit_SyncData";

    public override bool IsValid() => true;
}