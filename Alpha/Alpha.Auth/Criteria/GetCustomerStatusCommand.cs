namespace Alpha.Auth.Criteria
{
    using Fanex.Data.Repository;

    internal class GetCustomerStatusCommand : NonQueryCommand
    {
        public GetCustomerStatusCommand(int custId, int subAccId)
        {
            CustId = custId;
            SubAccId = subAccId;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public bool IsActive { get; set; }

        public byte Locked { get; set; }

        public override string GetSettingKey() => "Age_CustomerStatusSel";

        public override bool IsValid() => CustId > 0 && SubAccId >= 0;
    }
}
