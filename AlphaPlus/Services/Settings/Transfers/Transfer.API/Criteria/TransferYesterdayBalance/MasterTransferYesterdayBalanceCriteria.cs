namespace AlphaPlus.Settings.Transfer.API.Criteria.TransferYesterdayBalance;

public class MasterTransferYesterdayBalanceCriteria(
    int custId,
    string superIdsTransferTotal,
    string toCustIds,
    bool fullTransfer,
    decimal amtTrans,
    string custStatus,
    byte autoTransfer,
    string language,
    string ip,
    int creator,
    int subAccId) : TransferYesterdayBalanceCriteria(superIdsTransferTotal, language, ip, creator, subAccId)
{
    public int MasterId { get; } = custId;

    public string ToCustIds { get; } = toCustIds;

    public bool FullTransfer { get; } = fullTransfer;

    public decimal AmtTrans { get; } = amtTrans;

    public string CustStatus { get; } = custStatus;

    public byte AutoTransfer { get; } = autoTransfer;

    public override string GetSettingKey() => "Age_Transfer_Ins_MAg";

    public override bool IsValid() => MasterId > 0 && !string.IsNullOrWhiteSpace(ToCustIds);
}