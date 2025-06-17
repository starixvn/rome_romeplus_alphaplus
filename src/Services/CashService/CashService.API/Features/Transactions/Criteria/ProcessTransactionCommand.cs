namespace AlphaPlus.CashService.API.Features.Transactions.Criteria;

using System.Numerics;
using Fanex.Data.Repository;

public class ProcessTransactionCommand : CriteriaBase
{
    public BigInteger TransId { get; set; }
    public int CustId { get; set; }
    public int AgentId { get; set; }
    public int SubAccId { get; set; }
    public int StatusId { get; set; }
    public int SiteDbGroupId { get; set; }
    public string PaymentFrom { get; set; }
    public int ProviderId { get; set; }
    public int PaymentMethodId { get; set; }
    public string Remark { get; set; }
    public byte[] Payslip { get; set; }
    public int ErrCode { get; set; }
    public string ErrMsg { get; set; }
    public string CashAccumCreditTbl { get; set; }

    public override string GetSettingKey() => $"SiteDb_{SiteDbGroupId}_Age_CashWithdrawal_UpdateTransStatus";

    public override bool IsValid() => CustId > 0 && TransId > 0 && AgentId > 0 && !string.IsNullOrWhiteSpace(PaymentFrom);
}