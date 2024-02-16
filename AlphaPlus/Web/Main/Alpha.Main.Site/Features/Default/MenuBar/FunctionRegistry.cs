using Alpha.Main.Constants;
using AlphaPlus.Core.Models;
using Alpha.Main.Models;
using static AlphaPlus.Core.Models.UserPermission;
using static AlphaPlus.Core.Models.UserSession;
using AlphaPlus.Core.Helpers;
using Fanex.Data.Repository;

namespace Alpha.Main.Site.Features.Default.MenuBar
{
    public static class FunctionRegistry
    {
        private static readonly ITransferService transferService = new TransferService(new DynamicRepository());

        public static readonly Dictionary<string, Func<MenuStatus>> MenuItemTypeStatusRules = new()
        {
            [string.Empty] = null,
            ["CreateAccountMenuItem"] = () => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.GetPermission(PageAccessType.CreateAccount)),
            ["PTTemplateMenuItem"] = () => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.GetPermission(PageAccessType.CreateAccount)),
            ["ReportMenuItem"] = () => new MenuStatus(() => !Current.IsSubAcc || Current.SubPageAccess.ViewReports, null),
            ["BetStatisticsMenuItem"] = () => new MenuStatus(() => !Current.IsSubAcc || Current.SubPageAccess.ViewReports, null),
            ["TotalBetsMenuItem"] = () => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.ViewTotalBets),
            ["MemberInfoMenuItem"] = () => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.ViewMemberInfo),

            ["SingleTransferMenuItem"] = () => new MenuStatus(SingleTransferMenuItemVisible),
            ["TransferMenuItem"] = () => new MenuStatus(!SingleTransferMenuItemVisible && TransferMenuItemVisible),
            ["AutoTransferMonitoringMenuItem"] = () => new MenuStatus(TransferMenuItemVisible && AutoTransferMonitoringMenuItemVisible),
            ["TransferAllAndClearCreditMenuItem"] = () => new MenuStatus(TransferMenuItemVisible && TransferAllAndClearCreditMenuItemVisible),
            ["TransferDownlineHistoryMenuItem"] = () => new MenuStatus(TransferMenuItemVisible && TransferToDownlineHistoryMenuItemVisible),

            ["BetListsMenuItem"] = () => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.GetPermission(PageAccessType.ViewBetLists)),
            ["ViewLogMenuItem"] = () => new MenuStatus(!Current.IsSubAcc || Current.SubPageAccess.ViewLog),
            ["SubAccountMenuItem"] = () => new MenuStatus(!Current.IsSubAcc),
            ["NumberGameMenuItem"] = () => new MenuStatus(IsEnableProduct(ProductId.NumberGame)),
            ["ConsolidatedReportMenuItem"] = () => new MenuStatus(Current.IsShowConsolidatedReport),
            ["TotalBetsForecast"] = () => new MenuStatus(() => !Current.IsSubAcc || Current.SubPageAccess.ViewTotalBets),
            ["BetChoiceLimitMenuItem"] = () => new MenuStatus(() => Current.IsShowBetChoiceLimit),
            ["ProgressiveContributionMenuItem"] = () => new MenuStatus(() => new User().IsEnabledJackpot),
            ["ProgressiveJackpotMenuItem"] = () => new MenuStatus(() => new User().ShowProgressiveJackpotGames),
            ["IndirectLoginAccountMenuItem"] = () => new MenuStatus(
                () => IsAllowedIndirectLogin(Current.SRecommendId) && !Current.IsIndirectLoggedIn && (Current.IsSuper || Current.IsMaster)),
            ["DormantAccountListMenuItem"] = () => new MenuStatus(() => Current.IsSuper || Current.IsMaster),
            ["BonusDetailMenuItem"] = () => new MenuStatus(() => AllowedSitesToViewBonusDetail.Contains(Current.Site.ToUpper()))
        };

        private static bool TransferMenuItemVisible
            => !Current.IsSubAcc || Current.SubPageAccess.Transfer;

        private static bool TransferAllAndClearCreditMenuItemVisible
        {
            get
            {
                if (!Current.IsSuper && !Current.IsMaster)
                {
                    return false;
                }

                var offerClearGivenCreditSites = AlphaPlus.Core.Configurations.ConfigurationManager.GetValue<string[]>("OfferClearGivenCredit:Sites", []);
                return offerClearGivenCreditSites.Any(s
                    => string.Equals(s, Current.Site, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(s, "*", StringComparison.OrdinalIgnoreCase));
            }
        }

        private static bool TransferToDownlineHistoryMenuItemVisible
            => IsAllowedTransferToDownlineHistorySite(Current.Site) && transferService.IsAllDayBalanceTransfer(Current.CustId);

        private static bool AutoTransferMonitoringMenuItemVisible
            => transferService.IsEnableAutoTransfer(Current.Currency, Current.CustId);

        private static bool SingleTransferMenuItemVisible
        {
            get
            {
                if (!TransferMenuItemVisible)
                {
                    return false;
                }

                var menu = new List<bool> {
                    TransferAllAndClearCreditMenuItemVisible,
                    TransferToDownlineHistoryMenuItemVisible,
                    AutoTransferMonitoringMenuItemVisible
                };

                return !menu.Any(m => m);
            }
        }

        public static Func<bool> ShowAccountInfo => () => Current.SubPageAccess.ViewMemberInfo && !Current.IsProtectedMode;

        public static Func<string> GetDefaultUrl => () =>
        {
            if (AlphaContext.Current == null)
            {
                return string.Empty;
            }

            var defaultMainPage = MicroSites.MainUrl("~/Dashboard/Index");

            return defaultMainPage;
        };

        public static Func<bool> IsProtectedModeFunc => () => Current.IsProtectedMode;

        public static Func<bool> IsInternalFunc => () => Current.IsInternal;

        public static Func<string> BuildLanguageQueryString => () => $"lng={Current.Language}";

        public static Func<string> BuildDarkModeQueryString => () => $"isDarkMode={(!string.IsNullOrWhiteSpace(Current.DarkModeName)).ToString().ToLowerInvariant()}";

        private static bool IsAllowedIndirectLogin(int srecommend)
        {
            var allowSuperIds = AlphaPlus.Core.Configurations.ConfigurationManager.GetValue<string[]>("IndirectLogin:AllowSuperIds", []);

            return allowSuperIds.Contains("*") || allowSuperIds.Contains(srecommend.ToString());
        }

        private static string[] AllowedSitesToViewBonusDetail
            => AlphaPlus.Core.Configurations.ConfigurationManager.GetValue<string[]>("AllowedSitesToViewBonusDetail", []);

        private static bool IsAllowedTransferToDownlineHistorySite(string site)
        {
            var allowSites = AlphaPlus.Core.Configurations.ConfigurationManager.GetValue<string[]>("TransferToDownlineHistory:OfferSites", []);
            return allowSites.Contains("*") || allowSites.Contains(site, StringComparer.OrdinalIgnoreCase);
        }
    }
}