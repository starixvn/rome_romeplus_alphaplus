using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.Notifications.Criteria;

public class GetTransactionPendingCountCriteria(int custId, byte typeId) : CriteriaBase
{
    public int CustId { get; } = custId;

    public byte TypeId { get; } = typeId;

    public override string GetSettingKey() => "Age_CashTransaction_PendingCount";

    public override bool IsValid() => CustId > 0 && TypeId > 0;
}
