using Alpha.Main.Services;
using Alpha.Main.AppResources;
using Alpha.Main.Configurations;
using AlphaPlus.Core.Helpers;
using System.Text.RegularExpressions;

namespace Alpha.Main.Models.Menus
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
        private readonly TransferService transferService;
        private readonly IUser user;
        private IBrowserHelper browserHelper;

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
            TransferService transferService,
            IUser user,
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
            this.user = user;
            this.browserHelper = browserHelper;
        }


        public string ResourceKey { get; set; }

        public string AppPath { get; set; }

        public bool ByPassAuthorization { get; set; }

        public bool ByPassProtectedMode { get; set; }

        public bool ByPassProtectedModeInternal { get; set; }

        public bool IsBeta { get; set; }

        public string DisplayName
        {
            get
            {
                return Alpha_sitemap.ResourceManager.GetString(ResourceKey);
            }
        }

        public virtual string Link
        {
            get
            {
                var context = AlphaContext.Current;
                var appVirtualPath = !string.IsNullOrWhiteSpace(AppPath) ? AppSettings.GetAppPath(AppPath) : string.Empty;

                var menuLink = string.IsNullOrWhiteSpace(appVirtualPath) ? link : Regex.Replace(link, context.Request.PathBase.Value ?? string.Empty, appVirtualPath);

                if (!string.IsNullOrWhiteSpace(menuLink) && string.Equals("transfer", ResourceKey, StringComparison.OrdinalIgnoreCase))
                {
                    menuLink = string.Format(menuLink, transferService.IsAllDayBalanceTransfer(user.CustId));
                }

                if (!string.IsNullOrWhiteSpace(menuLink) && menuLink.IndexOf("PTTemplate/Index", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    var languageParam = buildLanguageQueryStringFunc();
                    var darkModeParam = buildDarkModeQueryStringFunc();
                    return $"{menuLink}?{languageParam}&{darkModeParam}";
                }

                return menuLink;
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
                    && isVisibleForMobileOrDesktop()
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

        public IEnumerable<IMenuItem> VisibileSubMenus
        {
            get
            {
                return SubMenus.Where(menuItem => menuItem.Status.Visible);
            }
        }

        public bool IsNew { get; set; }

        public bool IsOld { get; set; }

        public bool IsIncludeLanguage { get; set; }

        public bool IsMobileView => browserHelper.IsMobile();

        public bool IsHiddenInMobile { get; set; }

        private bool isVisibleForMobileOrDesktop() =>
            IsMobileView && !IsHiddenInMobile ||
            !IsMobileView;
    }
}