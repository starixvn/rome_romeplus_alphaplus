namespace AlphaPlus.Settings.MemberInfo.Domain.Models.MemberRequests
{
    public class MemberRequestModel
    {
        public MemberRequestModel()
        {
        }

        public MemberRequestModel(
            int custId,
            string username,
            bool sync,
            decimal balance,
            decimal outstanding,
            decimal betCredit)
        {
            CustId = custId;
            Username = username;
            Sync = sync;
            Balance = balance;
            Outstanding = outstanding;
            BetCredit = betCredit;
        }
        
        public int CustId { get; set; }

        public string Username { get; set; }

        public bool Sync { get; set; }

        public decimal Balance { get; set; }

        public decimal Outstanding { get; set; }

        public decimal BetCredit { get; set; }
    }
}