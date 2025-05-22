namespace AlphaPlus.Settings.ViewLogs.API.Models
{
    public class CustomerInfo
    {
        public CustomerInfo(int custId, bool isSubAcc, int roleId)
        {
            CustId = custId;
            IsSubAcc = isSubAcc;
            RoleId = roleId;
        }

        public int CustId { get; }

        public bool IsSubAcc { get; }

        public int RoleId { get; }

        public string SiteName { get; set; }
    }
}
