using AgencyCash.API.Shared.Helpers;
using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Criteria.Deposit;

public class CheckAllowDepositCriteria(int custId, short custDBGroupId) : NonQueryCommand
{
    public int CustId { get; } = custId;

    public bool IsAllowDeposit { get; set; }

    public override string GetSettingKey()
        => SPNameHelper.CustDBPrefix("Age_CashService_AgentPermissionSel", custDBGroupId);

    public override bool IsValid() => CustId > 0;
}