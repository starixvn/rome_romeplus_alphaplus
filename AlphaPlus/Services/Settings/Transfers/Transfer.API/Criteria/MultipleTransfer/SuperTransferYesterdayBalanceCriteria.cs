namespace AlphaPlus.Settings.Transfer.API.Criteria.MultipleTransfer;

public class SuperTransferYesterdayBalanceCriteria(
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
    int subAccId) : TransferYesterdayBalanceCriteria(superIdsTransferTotal, autoTransfer, language, ip, creator, subAccId)
{
    public int SAgId { get; } = custId;

    public string ToCustIds { get; } = toCustIds;

    public bool FullTransfer { get; } = fullTransfer;

    public decimal AmtTrans { get; } = amtTrans;

    public string CustStatus { get; } = custStatus;

    public override string GetSettingKey() => "Age_Transfer_Ins_SAg";

    public override bool IsValid() => SAgId > 0 && !string.IsNullOrWhiteSpace(ToCustIds);
}