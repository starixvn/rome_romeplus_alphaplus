using Alpha.Main.Site.Helpers;
using AlphaPlus.Core.Models;
using Alpha.Main.Configurations;
using MediatR;
using Alpha.Main.Site.ViewModels;
using AlphaPlus.Core.Helpers;


namespace Alpha.Main.Site.Models.Requests
{
    public class IndexQuery : IRequest<IndexViewModel>
    {
        private readonly bool isEnableSecurityCode;
        private readonly IUserSession userSession;
        private readonly bool disableMessages;
        private readonly INicknameChecker nicknameChecker;

        public IndexQuery(
            IUserSession userSession,
            bool isEnableSecurityCode,
            bool? disableMessages,
            INicknameChecker nicknameChecker)
        {
            this.userSession = userSession;
            this.isEnableSecurityCode = isEnableSecurityCode;
            this.disableMessages = disableMessages ?? AppSettings.DisableMessages;
            this.nicknameChecker = nicknameChecker;
            IsProtectedMode = userSession.IsProtectedMode;
            IsOfferProtectedMode = userSession.IsOfferProtectedMode;
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

        public bool ShowNicknameMenu
            => !nicknameChecker.IsOfferForceLoginAndOverDueDate;

        public bool ShowCreateMemberByQRCode =>
            userSession.SubPageAccess.CreateAccount
            && Enumeration.FromValue<UserLevel>(userSession.RoleId).IsAgentLevel()
            && !IsProtectedMode
            && !IsQRCodeHidden;

        public bool IsIndirectLoggedIn => userSession.IsIndirectLoggedIn;

        public string DarkModeName => DarkModeHelper.GetDarkModeName(userSession);

        public bool IsProtectedMode { get; }

        public bool IsOfferProtectedMode { get; }

        public string AssetAppPath => userSession.AssetAppPath;

        private static bool IsQRCodeHidden => AppSettings.HideQRCode;
        public int SuperId => userSession.SRecommendId;

        public string Currency => userSession.Currency;
    }
}