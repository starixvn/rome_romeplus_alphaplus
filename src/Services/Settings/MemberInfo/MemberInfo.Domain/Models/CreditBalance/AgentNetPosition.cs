namespace AlphaPlus.Settings.MemberInfo.Domain.Models.CreditBalance
{
    public class AgentNetPosition
    {
        public AgentNetPosition()
        {
        }

        public AgentNetPosition(int custId, decimal netPosition)
        {
            CustId = custId;
            NetPosition = netPosition;
        }

        public int CustId { get; set; }

        public decimal NetPosition { get; set; }
    }
}
