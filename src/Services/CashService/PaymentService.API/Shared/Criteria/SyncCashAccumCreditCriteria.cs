using PaymentService.API.Shared.Helpers;
using Fanex.Data.Repository;

namespace PaymentService.API.Payments.Criteria;

public class SyncCashAccumCreditCriteria(string cashAccumCreditTbl, int custDBGroupId) : NonQueryCommand
{
    public string CashAccumCreditTbl { get; } = cashAccumCreditTbl;

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public override string GetSettingKey()
        => SPNameHelper.CustDBPrefix("", custDBGroupId);

    public override bool IsValid() => !string.IsNullOrWhiteSpace(CashAccumCreditTbl);
}