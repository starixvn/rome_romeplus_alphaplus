using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Transfer.API.Criteria;

public class GetTransferOptionCriteria(int custId) : NonQueryCommand
{
    public int CustId { get; } = custId;

    public string TransferOption { get; set; } = string.Empty;

    public override string GetSettingKey() => "Acc_CustomerSelTransferOption";

    public override bool IsValid() => CustId > 0;
}
