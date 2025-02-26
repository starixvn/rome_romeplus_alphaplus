namespace AlphaPlus.Settings.Site.Features.CopyAccount.Models
{
    public class Downline
    {
        public int CustId { get; set; }

        public byte RoleId { get; set; }

        public bool IsActive { get; set; }

        public bool IsSubAcc { get; set; }
    }
}