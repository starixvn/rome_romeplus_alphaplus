using AlphaPlus.Shared.AppResources.Settings.MemberInfo;

namespace AlphaPlus.Settings.MemberInfo.Domain.Models.CreditBalance
{
    public class CreditBalanceItem
    {
        public CreditBalanceItem()
        {
        }

        public CreditBalanceItem(
            int rowNumber,
            bool enabled,
            bool closed,
            bool suspended,
            int custId,
            string userName,
            string firstName,
            string lastName,
            decimal credit,
            decimal betCredit,
            decimal balance,
            decimal yesterdayBalance,
            decimal outstanding,
            decimal pendingTransfer3rd,
            DateTime createdDate,
            DateTime lastLogged,
            string ipLogin,
            bool isDirectMember)
        {
            RowNumber = rowNumber;
            Enabled = enabled;
            Closed = closed;
            Suspended = suspended;
            CustId = custId;
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            Credit = credit;
            BetCredit = betCredit;
            Balance = balance;
            Yesterday_Balance = yesterdayBalance;
            Outstanding = outstanding;
            PendingTransfer3rd = pendingTransfer3rd;

            LastLogged = lastLogged;
            CreatedDate = createdDate;
            IPLogin = ipLogin;
            IsDirectMember = isDirectMember;
        }

        public int RowNumber { get; set; }

        public bool Enabled { get; set; }

        public bool Closed { get; set; }

        public byte ClosedReason { get; set; }

        public bool Suspended { get; set; }

        public byte Locked { get; set; }

        public int CustId { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public decimal Credit { get; set; }

        public decimal BetCredit { get; set; }

        public decimal Balance { get; set; }

        public decimal Yesterday_Balance { get; set; }

        public decimal Outstanding { get; set; }

        public decimal PendingTransfer3rd { get; set; }

        public DateTime? LastLogged { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string IPLogin { get; set; }

        public bool IsDirectMember { get; set; }

        public string Status
        {
            get
            {
                if (!Enabled)
                {
                    return Resources.Disabled;
                }

                if (Closed)
                {
                    return Resources.Closed;
                }

                if (Suspended)
                {
                    return Resources.Suspended;
                }

                return Resources.Open;
            }
        }
    }
}
