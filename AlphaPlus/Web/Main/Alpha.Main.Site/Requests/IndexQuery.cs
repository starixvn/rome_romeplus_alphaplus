using Alpha.Main.Site.Helpers;
using AlphaPlus.Core.Models;
using MediatR;
using Alpha.Main.Site.ViewModels;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Configurations;

namespace Alpha.Main.Site.Models.Requests
{
    public class IndexQuery : IRequest<IndexViewModel>
    {
        private readonly bool isEnableSecurityCode;
        private readonly IUserSession userSession;
        private readonly bool disableMessages;
        private readonly AuthorizationOptions options;

        public IndexQuery(
            IUserSession userSession,
            bool isEnableSecurityCode,
            bool? disableMessages,
            AuthorizationOptions options)
        {
            this.userSession = userSession;
            this.isEnableSecurityCode = isEnableSecurityCode;
            this.disableMessages = disableMessages ?? Configurations.AppSettings.DisableMessages;
            IsProtectedMode = userSession.IsProtectedMode;
            IsOfferProtectedMode = userSession.IsOfferProtectedMode;
            this.options = options;
        }

        public int CustId => userSession.CustId;

        public string UserName => userSession.UserName;

        public string NickName => userSession.NickName;

        public int OnUserId => userSession.OnUserId;

        public bool IsSubAcc => userSession.IsSubAcc;

        public int SubAccId => userSession.SubAccId;

        public string SubUserName => userSession.SubUserName;

        public byte RoleId => userSession.RoleId;

        public string Language => userSession.Language;

        public string LanguageKey => userSession.LangKey;

        public string SubPageAccess => userSession.SubPageAccess;

        public bool IsSubAccount => userSession.IsSubAcc;

        public bool IsInternalWebVpn => userSession.IsInternalWebVPN;

        public bool IsInternal => userSession.IsInternal;

        public string AdminName => userSession.AdminName;

        public bool EnableSecurityCode => userSession.IsLogged &&
                    !userSession.IsPasswordExpired &&
                    this.isEnableSecurityCode;

        public bool ShowSearchAccount
            => userSession.SubPageAccess.ViewMemberInfo && !IsProtectedMode;

        public bool DisableMessages => disableMessages;

        public bool ShowCreateMemberByQRCode =>
            userSession.SubPageAccess.CreateAccount
            && Enumeration.FromValue<UserLevel>(userSession.RoleId).IsAgentLevel()
            && !IsProtectedMode
            && !IsQRCodeHidden;

        public bool IsIndirectLoggedIn => userSession.IsIndirectLoggedIn;

        public string DarkModeName => DarkModeHelper.GetDarkModeName(userSession);

        public IndexQuery(bool isProtectedMode, bool isOfferProtectedMode)
        {
            this.IsProtectedMode = isProtectedMode;
            this.IsOfferProtectedMode = isOfferProtectedMode;

        }
        public bool IsProtectedMode { get; }

        public bool IsOfferProtectedMode { get; }

        public string AssetAppPath => userSession.AssetAppPath;

        private static bool IsQRCodeHidden => Configurations.AppSettings.HideQRCode;
        public int SuperId => userSession.SRecommendId;

        public string Currency => userSession.Currency;

        public bool IsEnableReCheckIn => !userSession.IsInternal || !options.DisabledCheckDuplicationLogin;
    }
}