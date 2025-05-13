using AlphaPlus.CashService.API.Criteria;

namespace AlphaPlus.CashService.API.Criteria.Transactions;

public class SyncCashAccumCreditCommand() : BaseActionCommand(0, 0, "", "")
{
    public string CashAccumCreditTbl { get; set; }

    public override string GetSettingKey() => "Age_CashAccumCredit_SyncData";

    public override bool IsValid() => true;
}