using Alpha.Main.Site.Helpers;
using AlphaPlus.Core.Models;

namespace Alpha.Main.Site.Features.Password
{
    public class PasswordViewModel
    {
        public int CustId { get; set; }

        public int SubAccountId { get; set; }

        public byte CustRoleId { get; set; }

        public string UserLevel
        {
            get
            {
                return Enumeration.FromValue<UserLevel>(CustRoleId).DisplayName.ToLower();
            }
        }

        public string HashKey { get; set; }

        public bool EnableSecurityCode { get; set; }

        public bool IsOfferProtectedMode { get; set; }

        public string MainRootPath => UrlHelper.MapMainPath("~/");

        public bool IsChangeMemberPassword
        {
            get
            {
                return Enumeration.FromValue<UserLevel>(CustRoleId).IsMemberLevel();
            }
        }

        public string Title { get; set; }
    }
}
