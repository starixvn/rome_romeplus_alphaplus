using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.Transactions.Criteria;

public class RevertWithdrawalTransactionToPendingCommand : INonQueryCommand
{
    public long TransId { get; set; }
    public string Remark { get; set; }
    public int UserId { get; set; }
    public int CustId { get; set; }
    public int AgentId { get; set; }
    public int SubAccId { get; set; }
    public string Ip { get; set; }
    public short SiteDbGroupId { get; set; }
    public decimal AgentGivenCredit { get; set; }

    [SpParam(SqlParamName = "CreditMembers_GivenCredit")]
    public decimal CreditMembersGivenCredit { get; set; }

    public string CashAccumCreditTbl { get; set; }

    [SpParam(Ignored = true)]
    public short CustDbGroupId { get; set; }

    public int ErrCode { get; set; }
    public string ErrMsg { get; set; }

    public string GetSettingKey() => $"SiteDb_{SiteDbGroupId}_Age_CashTransaction_RevertWithdrawal_Upd";

    public bool IsValid() => TransId > 0;
}