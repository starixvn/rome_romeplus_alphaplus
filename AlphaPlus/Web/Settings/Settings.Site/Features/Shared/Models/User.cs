using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.Features.Shared.Models
{
    public class User : IUser
    {
        private const string Jackpot = "Jackpot";
        private const string GameEnableSessionKey = "_GameEnable_";
        private const string CustomerDateLimit = "CustomerDateLimit";

        private readonly IUserSession userSession;

        public User() : this(null) { }

        internal User(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public int LoggedInUserId => userSession.UserId;

        public int Id => userSession.CustId;

        public string Name => userSession.UserName;

        public string Site => userSession.Site;

        public string Language => userSession.Language;

        public string LanguageKey => userSession.LangKey;

        public byte LevelId => userSession.RoleId;

        public int SubAccId => userSession.SubAccId;

        public string SubAccName => userSession.SubUserName;

        public string IP => userSession.UserLoginIp;

        public bool CanUpdateAccount => userSession.SubPageAccess.UpdateAccount;

        public bool IsEnabledJackpot => EnableJackpotProductIds.Any();

        public IEnumerable<int> EnableJackpotProductIds => GetEnableProductsForGame(Jackpot);

        public bool GetHideProduct(int productId) => userSession.GetHideProduct(productId);

        public bool GetSyncedProduct(int productId) => userSession.GetSyncedProduct(productId);

        internal virtual IEnumerable<int> GetEnableProductsForGame(string gameName)
            => AlphaContext.Current.Session.Get<IList<int>>(AlphaContext.Current.Session.Id + GameEnableSessionKey + gameName) ?? Enumerable.Empty<int>();

        public bool GetDisabledProduct(int productId) => userSession.GetDisabledProduct(productId);

        public bool CanViewBetLists => userSession.SubPageAccess.GetPermission(PageAccessType.ViewBetLists);

        public bool IsAllowedTotalBalanceTransfer => userSession.IsAllowedTotalBalanceTransfer;

        public bool CanTransfer => userSession.SubPageAccess.Transfer;

        public DateTime LimitedDateToShowInfo => userSession.GetProperty<DateTime>(CustomerDateLimit);

        public bool IsAdminCanViewVNIP => userSession.IsAdminCanViewVNIP;

        public bool IsInternal => userSession.IsInternal;

        public int Srecommend => userSession.SRecommendId;

        // public IEnumerable<int> IndirectLoginSuperIds => AppSettings.IndirectLoginSuperIds;

        // public bool IsAllowedIndirectLogin => !userSession.IsIndirectLoggedIn && AppSettings.IsAllowedIndirectLogin(Srecommend);

        public bool AllowedTotalBalanceTransfer => userSession.IsAllowedTotalBalanceTransfer;

        public bool IsShowMyanmarOdds => userSession.IsMyanmarOdds;

        public bool CanViewReports => userSession.SubPageAccess.ViewReports;

        public bool IsOfferProtectedMode => userSession.IsOfferProtectedMode;

        public string DarkModeName => userSession.DarkModeName;

        public string AssetAppPath => userSession.AssetAppPath;

        public string SiteDBGroupIp => userSession.AGIP;

        public string Currency => userSession.Currency;

        public bool IsSubAcc => userSession.IsSubAcc;

        public int CurrencyId => userSession.CurrencyId;

        public PageAccess SubPageAccess => userSession.SubPageAccess;

        public string AGIP => userSession.AGIP;

        public string LangKey => userSession.LangKey;

        public bool IsEnabledCustomerDateLimit => userSession.GetProperty<DateTime>("CustomerDateLimit") != default;

        public int[] OfferedProductGroupIds => userSession.OfferedProductGroupIds;
    }
}