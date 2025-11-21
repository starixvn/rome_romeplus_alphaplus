namespace AlphaPlus.Settings.Transfer.API.Criteria.TransferYesterdayBalance;

using Core.Helpers;

public class AgentTransferMultipleYesterdayBalanceCriteria(
    int agentId,
    string toCustIds,
    string status,
    byte autoTransfer,
    bool isAllDayBalTransfer,
    string language,
    string ip,
    int subAccId) : TransferYesterdayBalanceCriteria(language, ip, agentId, subAccId)
{
    public int FromCustId { get; } = agentId;

    public string ToCustIds { get; } = toCustIds;

    public string CustStatus { get; } = status;

    public byte AutoTransfer { get; } = autoTransfer;

    public bool IsAllDayBalTransfer { get; } = isAllDayBalTransfer;

    public override string GetSettingKey() => SpNameHelpers.SiteDbPrefix("Age_Transfer_InsClearBalance_Ag", SiteDbGroupId);

    public override bool IsValid() => FromCustId > 0 && !string.IsNullOrWhiteSpace(ToCustIds);
}