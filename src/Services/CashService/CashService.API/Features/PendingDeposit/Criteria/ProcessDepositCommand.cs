namespace AlphaPlus.CashService.API.Features.PendingDeposit.Criteria;

using System.Numerics;
using Fanex.Data;
using Fanex.Data.Repository;

public class ProcessDepositCommand : CriteriaBase
{
    public BigInteger TransId { get; set; }
    public int CustId { get; set; }
    public int AgentId { get; set; }
    public int SubAccId { get; set; }
    public int StatusId { get; set; }

    [SpParam(SqlParamName = "New_Amount")]
    public decimal NewAmount { get; set; }

    public int SiteDbGroupId { get; set; }

    [SpParam(AllowNull = true)]
    public decimal? AgentGivenCredit { get; set; }

    [SpParam(SqlParamName = "CreditMembers_GivenCredit", AllowNull = true)]
    public decimal? CreditMembersGivenCredit { get; set; }

    public string Ip { get; set; }

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }
    public string CashAccumCreditTbl { get; set; }
    public string Remark { get; set; }

    public override string GetSettingKey() => $"SiteDb_{SiteDbGroupId}_Age_CashDeposit_UpdateTransStatus";

    public override bool IsValid() => CustId > 0 && AgentId > 0 && StatusId > 0 && TransId > 0;
}