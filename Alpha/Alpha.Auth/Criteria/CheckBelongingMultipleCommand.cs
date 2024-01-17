namespace Alpha.Auth.Criteria
{
    using Fanex.Data.Repository;

    internal class CheckBelongingMultipleCommand : NonQueryCommand
    {
        public CheckBelongingMultipleCommand(int sessionCustId, string custId, int subAccId)
        {
            SessionCustId = sessionCustId;
            CustId = custId;
            SubAccountID = subAccId;
        }

        public int SessionCustId { get; }

        public string CustId { get; }

        public int SubAccountID { get; }

        public bool IsActive { get; set; }

        public byte RoleId { get; set; }

        public override string GetSettingKey() => "Age_CustomerSelCheckBelonging_MultiCust";

        public override bool IsValid() => SessionCustId > 0;
    }
}
