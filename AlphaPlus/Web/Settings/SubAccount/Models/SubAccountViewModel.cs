using AlphaPlus.Core.Configurations;

namespace AlphaPlus.Settings.Site.SubAccount.Models
{
    public class SubAccountViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public string PassWord { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsClosed { get; set; }
        public PageAccess PageAccess { get; set; }
    }
}