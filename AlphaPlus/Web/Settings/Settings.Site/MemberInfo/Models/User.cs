namespace AlphaPlus.Settings.Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AlphaPlus.Core.Extensions;
    using AlphaPlus.Core.Helpers;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using AlphaPlus.Core.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Configurations;

    public class User : IUser
    {
        private const string Jackpot = "Jackpot";
        private const string GameEnableSessionKey = "_GameEnable_";
        private const string CustomerDatelimit = "CustomerDateLimit";

        private readonly IUserSession userSession;

        public User() : this(null){}
        public User(IUserSession userSession)
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

        public DateTime LimitedDateToShowInfo => userSession.GetProperty<DateTime>(CustomerDatelimit);

        public bool IsAdminCanViewVNIP => userSession.IsAdminCanViewVNIP;

        public bool IsInternal => userSession.IsInternal;

        public int Srecommend => userSession.SRecommendId;

        public IEnumerable<int> IndirectLoginSuperIds => AppSettings.IndirectLoginSuperIds;

        public bool IsAllowedIndirectLogin => !userSession.IsIndirectLoggedIn && AppSettings.IsAllowedIndirectLogin(Srecommend);

        public bool IsDisabledESports => userSession.IsDisabledESports;

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
    }
}