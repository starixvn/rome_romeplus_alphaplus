namespace AlphaPlus.Settings.Transfer.API.Criteria.TransferYesterdayBalance;

public class AgentTransferYesterdayBalanceCriteria(
    int custId,
    string superIdsTransferTotal,
    string toUsername,
    decimal amount,
    bool isAllDayBalTransfer,
    byte autoTransfer,
    string language,
    string ip,
    int creator,
    int subAccId) : TransferYesterdayBalanceCriteria(superIdsTransferTotal, autoTransfer, language, ip, creator, subAccId)
{
    public int FromCustId { get; } = custId;

    public string ToUsername { get; } = toUsername;

    public decimal Amount { get; } = amount;

    public bool IsAllDayBalTransfer { get; } = isAllDayBalTransfer;

    public override string GetSettingKey() => "Age_Transfer_Ins_Ag";

    public override bool IsValid() => FromCustId > 0 && !string.IsNullOrWhiteSpace(ToUsername);
}