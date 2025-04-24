namespace AlphaPlus.Settings.MemberInfo.Domain.Models.CreditBalance
{
    public class CustomerClosedReason
    {
        public CustomerClosedReason()
        {
        }

        public CustomerClosedReason(int custId, byte closedReason, byte locked)
        {
            CustId = custId;
            ClosedReason = closedReason;
            Locked = locked;
        }

        public int CustId { get; set; }

        public byte ClosedReason { get; set; }

        public byte Locked { get; set; }
    }
}
