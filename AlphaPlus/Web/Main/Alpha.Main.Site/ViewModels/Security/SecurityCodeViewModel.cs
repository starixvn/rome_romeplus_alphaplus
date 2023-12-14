using AlphaPlus.Core.Models;
using Alpha.Main.Site.Helpers;

namespace Alpha.Main.Site.ViewModels.Security
{
    public class SecurityCodeViewModel
    {
        public string SecurityCodeToken { get; set; }

        public string SecuritySalt { get; set; }

        public string HashKey { get; set; }

        public byte CustRoleId { get; set; }

        public string UserLevel => Enumeration.FromValue<UserLevel>(CustRoleId).DisplayName.ToLower();

        public string MainRootPath => UrlHelper.MapMainPath("~/");

        public string Title { get; set; }

        public string UsernameShower { get; set; }

        public bool IsDefaultAvatar { get; set; }

        public string EmbedImage { get; set; }

        public bool IsEnabledBetLists { get; set; }

        public bool IsEnabledStatement { get; set; }

        public bool IsOfferProtectedMode { get; set; }

        public int CustId { get; set; }

        public int SubAccId { get; set; }
    }
}
