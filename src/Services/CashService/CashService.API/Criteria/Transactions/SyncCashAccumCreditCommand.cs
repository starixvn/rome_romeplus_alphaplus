using AlphaPlus.CashService.API.Models;

namespace AlphaPlus.CashService.API.Criteria.Transactions;

public class SyncCashAccumCreditCommand : BaseNonQueryCommand
{
    public string CashAccumCreditTbl { get; set; }

    public override string GetSettingKey() => "Age_CashAccumCredit_SyncData";

    public override bool IsValid() => true;
}