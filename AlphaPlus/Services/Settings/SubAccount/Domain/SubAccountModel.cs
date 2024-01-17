using AlphaPlus.Core.Configurations;

namespace AlphaPlus.Settings.Domain
{
    public class SubAccountModel
    {
        public int SubAccId { get; set; }
        public string Username { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Closed { get; set; }
        public string PageAccess { get; set; }
    }

    public static class PageAccessHelper
    {
        public static PageAccess ParsePageAccess(this string permissions)
        {
            return new PageAccess { Permission = permissions };
        }
    }
}