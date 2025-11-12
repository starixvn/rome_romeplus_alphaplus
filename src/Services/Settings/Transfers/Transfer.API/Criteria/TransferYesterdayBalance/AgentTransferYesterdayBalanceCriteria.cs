namespace AlphaPlus.Settings.Transfer.API.Criteria.TransferYesterdayBalance;

using Core.Helpers;

public class AgentTransferYesterdayBalanceCriteria(
    string agentName,
    int memberId,
    decimal amount,
    bool isAllDayBalTransfer,
    string language,
    string ip,
    int creator,
    int subAccId,
    int siteDbGroupId) : TransferYesterdayBalanceCriteria(language, ip, creator, subAccId, siteDbGroupId)
{
    public string ToUsername { get; } = agentName;

    public int FromCustId { get; } = memberId;

    public decimal Amount { get; } = amount;

    public bool IsAllDayBalTransfer { get; } = isAllDayBalTransfer;

    public override string GetSettingKey() => SpNameHelpers.SiteDbPrefix("Age_Transfer_Ins_Ag", SiteDbGroupId);

    public override bool IsValid() => FromCustId > 0 && !string.IsNullOrWhiteSpace(ToUsername);
}