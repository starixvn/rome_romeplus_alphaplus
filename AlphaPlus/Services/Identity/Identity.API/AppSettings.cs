using AlphaPlus.Identity.API.Features.Auth;

namespace AlphaPlus.Identity.API
{
    public class AppSettings
    {
        public byte SettingStatus { get; set; }

        public string AllowedListSuperAccounts { get; set; }

        public int AutoFollowBetMasterId { get; set; }

        public bool IsTest { get; set; }

        public PasswordService PasswordService { get; set; }
    }
}