using AlphaPlus.CashService.API.Criteria;
using AlphaPlus.CashService.API.Features.Shared.Criteria;
using Fanex.Data;

namespace AlphaPlus.CashService.API.Criteria.PendingWithdrawal;

public class CreateManualWithdrawCommand() : BaseActionCommand(0, 0, "", "")
{
    public int UplineId { get; set; }

    public decimal MinLimitAmt { get; set; }

    public decimal MaxLimitAmt { get; set; }

    public int WithdrawalLimitCnt { get; set; }

    public string PaymentFrom { get; set; }

    public string PaymentTo { get; set; }

    public decimal Amount { get; set; }

    public byte[] PaySlip { get; set; }

    public string Ip { get; set; }

    public string Remark { get; set; }

    public int ProviderId { get; set; }

    [SpParam(Ignored = true)]
    public int TransId { get; set; }

    [SpParam(Ignored = true)]
    public string CashAccumCreditTbl { get; set; }

    public override string GetSettingKey() => "Age_CashWithdrawal_ManualAdd";

    public override bool IsValid() => CustId > 0;
}