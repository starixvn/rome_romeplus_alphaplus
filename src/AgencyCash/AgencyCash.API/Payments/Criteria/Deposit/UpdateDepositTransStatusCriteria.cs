using AgencyCash.API.Shared.Helpers;
using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Criteria.Deposit;

public class UpdateDepositTransStatusCriteria(
    int agentId,
    int custId,
    long transId,
    byte statusId,
    short siteDBGroupId,
    decimal agentGivenCredit,
    decimal creditMembersGivenCredit,
    string providerTransId,
    string ip) : CriteriaBase
{
    public int AgentId { get; } = agentId;

    public int CustId { get; } = custId;

    public long TransId { get; } = transId;

    public byte StatusId { get; } = statusId;

    public short SiteDBGroupId { get; } = siteDBGroupId;

    public decimal AgentGivenCredit { get; } = agentGivenCredit;

    public decimal CreditMembers_GivenCredit { get; } = creditMembersGivenCredit;

    public string ProviderTransId { get; } = providerTransId;

    public string IP { get; } = ip;

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public string ErrExtraInfo { get; set; }

    public string CashAccumCreditTbl { get; set; }

    public override string GetSettingKey()
        => SPNameHelper.SiteDBPrefix("API_CashDeposit_UpdateTransStatus", SiteDBGroupId);

    public override bool IsValid() => AgentId > 0 && CustId > 0 && TransId > 0;
}