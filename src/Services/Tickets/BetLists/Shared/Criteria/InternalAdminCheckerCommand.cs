using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.Shared.Criteria;

public class InternalAdminCheckerCommand(int custId) : NonQueryCommand
{
    public int CustId { get; } = custId;

    public bool IsInternalAdmin { get; set; }

    public override string GetSettingKey() => "Acc_CustomerIsInternalAdmin";

    public override bool IsValid() => CustId > 0;
}