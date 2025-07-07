using Fanex.Data.Repository;

namespace AgencyCash.API.RegisterCashMember.Criteria
{
    public class RollbackRegisterCustomerCommand(int custId) : INonQueryCommand
    {
        public int CustId { get; } = custId;

        public string GetSettingKey() => "Acc_CustomerIns_Rollback";

        public bool IsValid() => true;
    }

}
