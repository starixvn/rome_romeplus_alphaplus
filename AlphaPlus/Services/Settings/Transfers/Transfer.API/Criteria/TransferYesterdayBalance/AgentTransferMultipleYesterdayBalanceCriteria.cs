namespace AlphaPlus.Settings.Transfer.API.Criteria.TransferYesterdayBalance;

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

    public override string GetSettingKey() => "SiteDB_Age_Transfer_InsClearBalance_Ag";

    public override bool IsValid() => FromCustId > 0 && !string.IsNullOrWhiteSpace(ToCustIds);
}