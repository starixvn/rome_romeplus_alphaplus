using System.Web;
using Alpha.Main.AppResources;
using Alpha.Main.ProfilePicture.Models;
using AlphaPlus.Core.Models;
using Alpha.Main.Configurations;
using Alpha.Main.Site.Helpers;
using Alpha.Main.Site.Models.Requests;
using AlphaPlus.SecurityCode.Web;
using AlphaPlus.Core.Helpers;

namespace Alpha.Main.Site.ViewModels
{
    public class IndexViewModel
    {
        private readonly IndexQuery message;
        private readonly Func<string> getDefaultUrl;
        private readonly IBrowserHelper browserHelper;
        private readonly ActorProfilePictureModel profilePicture;

        public IndexViewModel(
            IndexQuery message,
            string passwordHash,
            Func<string> getDefaultUrl,
            IBrowserHelper browserHelper,
            ActorProfilePictureModel profilePicture,
            bool isEnableAutoTransfer)
        {
            this.message = message;
            PasswordHash = passwordHash;
            this.getDefaultUrl = getDefaultUrl ?? FunctionRegistry.GetDefaultUrl;
            this.browserHelper = browserHelper;
            RequiredNetworkSpeedCalculation = message.CustId > 0 && AppSettings.TrackingConnectionCustIds.Contains(message.CustId);
            DisableMessages = message.DisableMessages;
            this.profilePicture = profilePicture;
            IsProtectedMode = message.IsProtectedMode;
            AssetAppPath = message.AssetAppPath;
            IsEnableAutoTransfer = isEnableAutoTransfer;
        }

        public bool EnableSecurityCode => message.EnableSecurityCode;

        public int CustId => message.CustId;

        public int SubAccId => message.SubAccId;

        public int OnUserId => message.OnUserId;

        public string UserLevel => Enumeration.FromValue<UserLevel>(message.RoleId).DisplayName.ToLower();

        public byte RoleId => message.RoleId;

        public string Language => message.Language;

        public string LanguageKey => message.LanguageKey;

        public string HowToUseRootPath => UrlHelper.MapHowToUsePath("~/");

        public string MemberInfoRootPath => UrlHelper.MapMemberInfoPath("~/");

        public string AccountInfoRootPath => UrlHelper.MapAccountInfoPath("~/");

        public string ExTotalBetsForecastRootPath => UrlHelper.MapExTotalBetsForecastPath("~/");

        public string MessageSiteRootPath => UrlHelper.MapMessagesPath("~/");

        public string MainRootPath => UrlHelper.MapMainPath("~/");

        public string ResultRootPath => UrlHelper.MapResultPath();

        public string TransferRootPath => UrlHelper.MapTransferPath("~/");

        public string PasswordHash { get; }

        public string SubPageAccess => message.SubPageAccess;

        public bool IsSubAccount => message.IsSubAccount;

        public bool IsInternalWebVPN => message.IsInternalWebVpn;

        public bool IsInternal => message.IsInternal || message.IsIndirectLoggedIn;

        public bool IsShowIntroduction => true;

        public bool ShowOldCustomerList { get; }

        public string DefaultPath => getDefaultUrl?.Invoke() ?? MainRootPath;

        public HeaderNavViewModel HeaderNavViewModel =>
            new(
                CustId,
                message.UserName,
                BuildNickName(message),
                EnableSecurityCode,
                message.ShowSearchAccount,
                IsEnabledOtp,
                string.IsNullOrEmpty(profilePicture.ImageBase64String),
                profilePicture.ImageBase64String,
                message.ShowNicknameMenu,
                message.ShowCreateMemberByQRCode,
                !string.IsNullOrWhiteSpace(message.DarkModeName),
                IsProtectedMode,
                message.IsOfferProtectedMode,
                IsSubAccount,
                message.SuperId);

        public bool IsEnabledOtp => SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp;

        public double InitialScaleViewPort
        {
            get
            {
                var browserName = browserHelper.GetDevice().Info.BrowserName.ToLower();
                var isTablet = browserHelper.IsMobile(isOnlyTablet: true);

                if (browserName.Contains("chrome") && browserHelper.IsAndroid() && !browserHelper.IsAndroidNativeBrowser())
                {
                    if (!isTablet)
                    {
                        return 0.5;
                    }
                }

                return 0.7;
            }
        }

        public bool RequiredNetworkSpeedCalculation { get; }

        public bool DisableMessages { get; }

        public bool IsProtectedMode { get; }

        internal static string BuildNickName(IndexQuery message)
        {
            if (message.IsSubAcc)
            {
                return $"<p>{Resources.subAcc} {HttpUtility.HtmlEncode(message.SubUserName)}</p><p>{HttpUtility.HtmlEncode(message.NickName)}</p>";
            }

            if (message.IsInternal)
            {
                return $"{Resources.admin} {HttpUtility.HtmlEncode(message.AdminName)}";
            }

            return HttpUtility.HtmlEncode(message.NickName);
        }

        public bool IsOfferLiveChat { get; } = AppSettings.IsOfferLiveChat;

        public int UMAnnouncementPopupClientIntervalInMinutes { get; } = AppSettings.UMAnnouncementPopupClientIntervalInMinutes;

        public string DarkModeName => message.DarkModeName;

        public bool IsDarkMode => !string.IsNullOrWhiteSpace(message.DarkModeName);

        public string AssetAppPath { get; }

        public bool IsEnabledCDNReCheckin
            => !string.IsNullOrWhiteSpace(AssetAppPath)
            && !AlphaPlus.Core.Configurations.AppSettings.AssetAppPath.Equals(AssetAppPath, StringComparison.OrdinalIgnoreCase);

        public int CDNReCheckinIntervalInSeconds { get; } = AppSettings.CDNReCheckinIntervalInSeconds;

        public int RecheckinIntervalMiliseconds => AppSettings.RecheckinIntervalMiliseconds;

        public string SessionID => AlphaContext.Current.Session.Id;

        public string Username => message.IsSubAcc ? message.SubUserName : message.UserName;

        public bool IsShowSuggestEnableTelegramOtp { get; set; }

        public bool HightLightFeaturePermission => false;

        public string HightLightFeatureUrl => MicroSites.MessageUrl($"~/BackupDomains/Index");

        public bool IsEnableAutoTransfer { get; set; }

        public bool IsEnableReCheckIn => message.IsEnableReCheckIn;
    }
}