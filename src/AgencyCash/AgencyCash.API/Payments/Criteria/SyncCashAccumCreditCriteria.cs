using AgencyCash.API.Shared.Helpers;
using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Criteria;

public class SyncCashAccumCreditCriteria(string cashAccumCreditTbl, short custDBGroupId) : NonQueryCommand
{
    public string CashAccumCreditTbl { get; } = cashAccumCreditTbl;

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public override string GetSettingKey()
        => SPNameHelper.CustDBPrefix("Age_CashAccumCredit_SyncData", custDBGroupId);

    public override bool IsValid() => !string.IsNullOrWhiteSpace(CashAccumCreditTbl);
}