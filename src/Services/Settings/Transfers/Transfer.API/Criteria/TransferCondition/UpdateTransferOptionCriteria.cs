using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Transfer.API.Criteria.TransferCondition;

public class UpdateTransferOptionCriteria(
    int custId,
    string transferOption,
    bool turnOffAutoTransfer,
    int subAccId,
    int userId,
    string ip,
    string language,
    bool returnErrCode) : CriteriaBase
{
    public int CustId { get; } = custId;

    public string TransferOption { get; } = transferOption;

    public bool TurnOffAutoTransfer { get; } = turnOffAutoTransfer;

    public int SubAccId { get; } = subAccId;

    public int UserId { get; } = userId;

    public string IP { get; } = ip;

    public string Language { get; } = language;

    public bool ReturnErrCode { get; } = returnErrCode;

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public override string GetSettingKey() => "Acc_CustomerUpdTransferOption";

    public override bool IsValid()
        => CustId > 0
        && UserId > 0
        && !string.IsNullOrWhiteSpace(TransferOption)
        && TransferOption.Length == 8;
}