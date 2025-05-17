using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.PendingDeposit;

public class CreateManualDepositCommand : NonQueryCommand
{
    public int CustId { get; set; }

    public int UplineId { get; set; }

    public int SubAccId { get; set; }

    public decimal AgentGivenCredit { get; set; }

    public decimal CreditMembers_GivenCredit { get; set; }

    public decimal MinLimitAmt { get; set; }

    public decimal MaxLimitAmt { get; set; }

    public string PaymentFrom { get; set; }

    public string PaymentTo { get; set; }

    public decimal Amount { get; set; }

    public byte[] Payslip { get; set; }

    public string Ip { get; set; }

    public string Remark { get; set; }

    public string CashAccumCreditTbl { get; set; }

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    [SpParam(Ignored = true)]
    public int SiteDbGroupId { get; set; }

    public override string GetSettingKey() => $"SiteDb_{SiteDbGroupId}_Age_CashDeposit_ManualAdd";

    public override bool IsValid() => CustId > 0;
}