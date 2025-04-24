using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Transfer.API.Criteria.TransferCondition;

internal class GetSuperAutomateTransferCriteria(int custId) : CriteriaBase
{
    public int CustId { get; } = custId;

    public override string GetSettingKey() => "Acc_CustomerSelSuperAutoTransfer";

    public override bool IsValid() => CustId > 0;
}
