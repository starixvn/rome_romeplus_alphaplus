using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;
using AlphaPlus.Shared.AppResources.Main;

namespace AlphaPlus.Main.Site.Features.Default.MenuBar
{
    public class MenuItem : IMenuItem
    {
        private string link = string.Empty;
        private readonly Func<MenuStatus> getMenuStatusFunc;
        private readonly Predicate<string> authorizeUrlFunc;
        private readonly Func<bool> isProtectedModeFunc;
        private readonly Func<bool> isInternalFunc;
        private readonly Func<bool> isHideForSite;
        private readonly Func<string> buildLanguageQueryStringFunc;
        private readonly Func<string> buildDarkModeQueryStringFunc;
        private readonly ITransferService transferService;
        private readonly IBrowserHelper browserHelper;
        private readonly IUserSession userSession;

        public MenuItem(Func<MenuStatus> getMenuStatusFunc = null)
            : this(getMenuStatusFunc, null, null, null, null, null, null, null, null, null)
        {
        }

        public MenuItem(Func<MenuStatus> getMenuStatusFunc, Predicate<string> authorizeUrlFunc)
            : this(getMenuStatusFunc, authorizeUrlFunc, null, null, null, null, null, null, null, null)
        {
        }

        public MenuItem(
            Func<MenuStatus> getMenuStatusFunc,
            Predicate<string> authorizeUrlFunc,
            Func<bool> isProtectedModeFunc,
            Func<bool> isInternalFunc,
            Func<bool> isHideForSite,
            Func<string> buildLanguageQueryStringFunc,
            Func<string> buildDarkModeQueryStringFunc,
            ITransferService transferService,
            IUserSession userSession,
            IBrowserHelper browserHelper)
        {
            this.getMenuStatusFunc = getMenuStatusFunc ?? (() => new MenuStatus(true, false));
            this.authorizeUrlFunc = authorizeUrlFunc;
            this.isProtectedModeFunc = isProtectedModeFunc;
            this.isInternalFunc = isInternalFunc;
            this.isHideForSite = isHideForSite;
            this.buildLanguageQueryStringFunc = buildLanguageQueryStringFunc;
            this.buildDarkModeQueryStringFunc = buildDarkModeQueryStringFunc;
            this.transferService = transferService;
            this.userSession = userSession;
            this.browserHelper = browserHelper;
        }


        public string ResourceKey { get; set; }

        public bool ByPassAuthorization { get; set; }

        public bool ByPassProtectedMode { get; set; }

        public bool ByPassProtectedModeInternal { get; set; }

        public bool IsBeta { get; set; }

        public string DisplayName => Alpha_sitemap.ResourceManager.GetString(ResourceKey);

        public virtual string Link
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(link))
                {
                    if (link.IndexOf("TransferList/Index", StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        return string.Format(link, transferService.IsAllDayBalanceTransfer(userSession.CustId));
                    }

                    if ((link.IndexOf("PTTemplate/Index", StringComparison.OrdinalIgnoreCase) > -1)
                    || (link.IndexOf("Customer/CreateDirectMember", StringComparison.OrdinalIgnoreCase) > -1)
                    || (link.IndexOf("Customer/CreateCashMember", StringComparison.OrdinalIgnoreCase) > -1))
                    {
                        var languageParam = buildLanguageQueryStringFunc();
                        var darkModeParam = buildDarkModeQueryStringFunc();
                        return $"{link}?{languageParam}&{darkModeParam}";
                    }
                }

                return link;
            }

            set
            {
                link = value;
            }
        }

        public virtual MenuStatus Status
        {
            get
            {
                var isAuthorized = (ByPassAuthorization
                    || authorizeUrlFunc(Link))
                    && IsVisibleForMobileOrDesktop()
                    && !isHideForSite();

                if (isProtectedModeFunc())
                {
                    isAuthorized &= ByPassProtectedMode;

                    if (isInternalFunc())
                    {
                        isAuthorized |= ByPassProtectedModeInternal;
                    }
                }

                return isAuthorized
                    ? getMenuStatusFunc()
                    : new MenuStatus(false, false);
            }
        }

        public bool ShowSubMenus { get; set; }

        public List<IMenuItem> SubMenus { get; set; }

        public IEnumerable<IMenuItem> VisibleSubMenus => SubMenus.Where(menuItem => menuItem.Status.Visible);

        public bool IsNew { get; set; }

        public bool IsOld { get; set; }

        public bool IsMobileView => browserHelper.IsMobile();

        public bool IsHiddenInMobile { get; set; }

        private bool IsVisibleForMobileOrDesktop() => IsMobileView && !IsHiddenInMobile || !IsMobileView;

        public bool HasBadge { get; set; }
    }
}