using AgencyCash.API.Shared.Helpers;
using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Criteria.Deposit;

public class UpdatePaySlipCriteria(
    int agentId,
    int custId,
    long transId,
    byte[] paySlip,
    string ip,
    short siteDBGroupId) : CriteriaBase
{
    public int AgentId { get; } = agentId;

    public int CustId { get; } = custId;

    public long TransId { get; } = transId;

    public byte[] PaySlip { get; } = paySlip;

    public string IP { get; } = ip;

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public string ErrExtraInfo { get; set; }

    public override string GetSettingKey()
        => SPNameHelper.SiteDBPrefix("API_CashDeposit_Payslip_Ins", siteDBGroupId);

    public override bool IsValid() => AgentId > 0 && CustId > 0 && TransId > 0;
}