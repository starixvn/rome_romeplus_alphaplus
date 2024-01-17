namespace Alpha.Auth.Criteria
{
    using Fanex.Data.Repository;

    internal class CheckBelongingOldCommand : NonQueryCommand
    {
        public CheckBelongingOldCommand(int sessionCustId, int? custId, long transId)
        {
            SessionCustId = sessionCustId;
            CustId = custId;
            TransId = transId;
        }

        public int SessionCustId { get; }

        public int? CustId { get; }

        public long TransId { get; }

        public byte RoleId { get; set; }

        public override string GetSettingKey() => "Age_CustomerSelCheckBelonging_OLD";

        public override bool IsValid() => SessionCustId > 0;
    }
}
