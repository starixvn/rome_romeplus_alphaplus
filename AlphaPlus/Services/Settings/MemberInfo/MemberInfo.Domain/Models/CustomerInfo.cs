namespace AlphaPlus.Settings.MemberInfo.Domain.Models
{
    public class CustomerInfo
    {
        public CustomerInfo()
        {
        }

        public CustomerInfo(string username, int custId)
        {
            Username = username;
            CustId = custId;
        }

        public string Username { get; set; } = string.Empty;

        public int CustId { get; set; }
    }
}
