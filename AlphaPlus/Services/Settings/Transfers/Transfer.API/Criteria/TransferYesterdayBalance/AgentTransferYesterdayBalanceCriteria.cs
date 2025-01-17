namespace AlphaPlus.Settings.Transfer.API.Criteria.TransferYesterdayBalance;

public class AgentTransferYesterdayBalanceCriteria(
    string agentName,
    int memberId,
    decimal amount,
    bool isAllDayBalTransfer,
    string language,
    string ip,
    int creator,
    int subAccId) : TransferYesterdayBalanceCriteria(language, ip, creator, subAccId)
{
    public string ToUsername { get; } = agentName;

    public int FromCustId { get; } = memberId;

    public decimal Amount { get; } = amount;

    public bool IsAllDayBalTransfer { get; } = isAllDayBalTransfer;

    public override string GetSettingKey() => "SiteDB_Age_Transfer_Ins_Ag";

    public override bool IsValid() => FromCustId > 0 && !string.IsNullOrWhiteSpace(ToUsername);
}