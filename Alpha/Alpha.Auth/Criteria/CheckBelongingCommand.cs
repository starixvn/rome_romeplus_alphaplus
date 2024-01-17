namespace Alpha.Auth.Criteria
{
    using Fanex.Data.Repository;

    internal class CheckBelongingCommand : NonQueryCommand
    {
        public CheckBelongingCommand(int sessionCustId, int? custId, int subAccId, long transId)
        {
            SessionCustId = sessionCustId;
            CustId = custId;
            SubAccountID = subAccId;
            TransId = transId;
        }

        public int SessionCustId { get; }

        public int? CustId { get; }

        public int SubAccountID { get; }

        public long TransId { get; }

        public bool IsActive { get; set; }

        public byte RoleId { get; set; }

        public string Username { get; set; }

        public override string GetSettingKey() => "Age_CustomerSelCheckBelonging";

        public override bool IsValid() => SessionCustId > 0;
    }
}
