namespace AlphaPlus.CashService.API.Features.PendingDeposit.Criteria;

using Fanex.Data;
using Fanex.Data.Repository;

public class CreateManualDepositCommand : CriteriaBase
{
    public int CustId { get; set; }

    public int UplineId { get; set; }

    public int SubAccId { get; set; }

    public decimal AgentGivenCredit { get; set; }

    public decimal CreditMembers_GivenCredit { get; set; }

    public string PaymentFrom { get; set; }

    public string PaymentTo { get; set; }

    public decimal Amount { get; set; }

    public byte[] PaySlip { get; set; }

    public string Ip { get; set; }

    public string Remark { get; set; }

    public string CashAccumCreditTbl { get; set; }

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public int SiteDbGroupId { get; set; }

    [SpParam(AllowNull = true)]
    public decimal? AgentAvailableCredit { get; set; }

    public override string GetSettingKey() => $"SiteDb_{SiteDbGroupId}_Age_CashDeposit_ManualAdd";

    public override bool IsValid() => CustId > 0;
}