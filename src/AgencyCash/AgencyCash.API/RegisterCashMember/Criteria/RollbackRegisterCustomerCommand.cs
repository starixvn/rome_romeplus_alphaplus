using Fanex.Data.Repository;

namespace AgencyCash.API.RegisterCashMember.Criteria
{
    public class RollbackRegisterCustomerCommand : INonQueryCommand
    {
        public RollbackRegisterCustomerCommand(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }

        public string GetSettingKey() => "Acc_CustomerIns_Rollback";
        
        public bool IsValid() => true;
    }

}
