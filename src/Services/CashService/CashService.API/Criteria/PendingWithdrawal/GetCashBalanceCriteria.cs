using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.PendingWithdrawal;

public class GetCashBalanceCriteria : CriteriaBase
{
    public int CustId { get; set; }

    public override string GetSettingKey() => "Acc_CustomerSelCash_CaptureBalance";

    public override bool IsValid() => CustId > 0;
}
