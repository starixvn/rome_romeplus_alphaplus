namespace AlphaPlus.CashService.API.Features.PendingWithdrawal.Criteria;

using Fanex.Data;
using Fanex.Data.Repository;

public class ProcessWithdrawalTransactionCommand : CriteriaBase
{
    public long TransId { get; set; }
    public int CustId { get; set; }
    public int AgentId { get; set; }
    public int SubAccId { get; set; }
    public int StatusId { get; set; }
    public int SiteDbGroupId { get; set; }

    [SpParam(AllowNull = true)]
    public string PaymentFrom { get; set; }

    public int ProviderId { get; set; }
    public int PaymentMethodId { get; set; }
    public string Remark { get; set; }
    public byte[] Payslip { get; set; }
    public int ErrCode { get; set; }
    public string ErrMsg { get; set; }
    public string CashAccumCreditTbl { get; set; }
    public string Ip { get; set; }

    public override string GetSettingKey() => $"SiteDb_{SiteDbGroupId}_Age_CashWithdrawal_UpdateTransStatus";

    public override bool IsValid() => CustId > 0 && TransId > 0 && AgentId > 0;
}