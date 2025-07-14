using AgencyCash.API.Shared.Helpers;
using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Criteria;

public class UpdateTransProviderInfoCriteria(
    int agentId,
    int custId,
    long transId,
    byte transType,
    string providerTransId,
    string ip,
    short siteDBGroupId) : CriteriaBase
{
    public int AgentId { get; } = agentId;

    public int CustId { get; } = custId;

    public long TransId { get; } = transId;

    public byte TransType { get; } = transType;

    public string ProviderTransId { get; } = providerTransId;

    public string IP { get; } = ip;

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public string ErrExtraInfo { get; set; }

    public override string GetSettingKey()
        => SPNameHelper.SiteDBPrefix("API_CashTransaction_ProviderInfo_Upd", siteDBGroupId);

    public override bool IsValid() => AgentId > 0 && CustId > 0 && TransId > 0;
}
