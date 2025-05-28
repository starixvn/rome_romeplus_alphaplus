namespace AlphaPlus.CashService.API.Features.PendingWithdrawal.Criteria;

using Fanex.Data;
using Fanex.Data.Repository;

public class GetCashBalanceCriteria : NonQueryCommand
{
    public int CustId { get; set; }
    public decimal TotalCash { get; set; }
    [SpParam(Ignored = true)] public int SiteDbGroupId { get; set; }

    public override string GetSettingKey() => $"SiteDb_{SiteDbGroupId}_Age_CashService_CashBalance_Sel";

    public override bool IsValid() => CustId > 0;
}