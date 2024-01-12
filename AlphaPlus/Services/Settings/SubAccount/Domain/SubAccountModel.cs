using AlphaPlus.Core.Configurations;

namespace AlphaPlus.Settings.Domain
{
    public class SubAccountModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsClosed { get; set; }
        public PageAccess PageAccess { get; set; }
    }
}