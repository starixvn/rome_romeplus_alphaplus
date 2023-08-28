namespace AlphaPlus.SmartPunter.Criteria
{
    public class InternalAdminChecker
    {
        public InternalAdminChecker(int customerId)
        {
            CustId = customerId;
        }

        public int CustId { get; }
    }
}