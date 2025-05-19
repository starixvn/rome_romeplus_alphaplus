namespace AlphaPlus.CashService.API.Criteria.PendingWithdrawal;

using Fanex.Data;
using Fanex.Data.Repository;

public class CreateManualWithdrawCommand : CriteriaBase
{
    public int UplineId { get; set; }

    public decimal MinLimitAmt { get; set; }

    [SpParam(AllowNull = true)]
    public decimal? MaxLimitAmt { get; set; }

    public string PaymentFrom { get; set; }

    public string PaymentTo { get; set; }

    public decimal Amount { get; set; }

    public byte[] Payslip { get; set; }

    public string Ip { get; set; }

    public string Remark { get; set; }

    public int ProviderId { get; set; }

    public long TransId { get; set; }

    public string CashAccumCreditTbl { get; set; }

    [SpParam(Ignored = true)]
    public int SiteDbGroupId { get; set; }

    public int CustId { get; set; }
    public int SubAccId { get; set; }
    public int ErrCode { get; set; }
    public string ErrMsg { get; set; }

    public override string GetSettingKey() => $"SiteDb_{SiteDbGroupId}_Age_CashWithdrawal_ManualAdd";

    public override bool IsValid() => CustId > 0;
}