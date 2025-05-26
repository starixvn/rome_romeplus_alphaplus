using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.Transactions.Criteria;

public class SyncCashAccumCreditCommand : NonQueryCommand
{
    public string CashAccumCreditTbl { get; set; }

    [SpParam(Ignored = true)]
    public int CustDbGroupId { get; set; }

    public int ErrCode { get; set; }
    public string ErrMsg { get; set; }

    public override string GetSettingKey() => $"CustDb_{CustDbGroupId}_Age_CashAccumCredit_SyncData";

    public override bool IsValid() => true;
}