using AlphaPlus.Main.Site.Shared.Models;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Main.Site.Features.Default.MenuBar
{
    public interface IMenuItemBuilder
    {
        IMenuItem BuildMenuItem(SiteMapNode node);
    }

    public class MenuItemBuilder : IMenuItemBuilder
    {
        private readonly AlphaPlus.Core.Services.IAuthorizationService authorizationService;
        private readonly IDictionary<string, Func<MenuStatus>> menuItemTypeConfiguration;
        private readonly Func<bool> isProtectedModeFunc;
        private readonly Func<bool> isInternalFunc;
        private readonly Func<string> buildLanguageQueryStringFunc;
        private readonly Func<string> buildDarkModeQueryStringFunc;
        private readonly ITransferService transferService;
        private readonly IUserSession userSession;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IBrowserHelper browserHelper;

        public MenuItemBuilder(
            AlphaPlus.Core.Services.IAuthorizationService authorizationService,
            ITransferService transferService,
            IUserSession userSession,
            IHttpContextAccessor httpContextAccessor,
            IBrowserHelper browserHelper)
        {
            this.authorizationService = authorizationService;
            this.menuItemTypeConfiguration = FunctionRegistry.MenuItemTypeStatusRules;
            this.isProtectedModeFunc = FunctionRegistry.IsProtectedModeFunc;
            this.isInternalFunc = FunctionRegistry.IsInternalFunc;
            this.transferService = transferService;
            this.userSession = userSession;
            this.buildLanguageQueryStringFunc = FunctionRegistry.BuildLanguageQueryString;
            this.buildDarkModeQueryStringFunc = FunctionRegistry.BuildDarkModeQueryString;
            this.httpContextAccessor = httpContextAccessor;
            this.browserHelper = browserHelper;
        }

        private void SetDataForMenuItem(IMenuItem menuItem, SiteMapNode node)
        {
            menuItem.ResourceKey = node.ResourceKey;
            menuItem.Link = node.Url;
            menuItem.IsBeta = node.IsBeta;
            menuItem.IsNew = node.IsNew;
            menuItem.IsOld = node.IsOld;
            menuItem.IsHiddenInMobile = node.IsHiddenInMobile;
            menuItem.ByPassAuthorization = node.ByPassAuthorization;
            menuItem.ByPassProtectedMode = node.ByPassProtectedMode;
            menuItem.ByPassProtectedModeInternal = node.ByPassProtectedModeInternal;
            menuItem.SubMenus = [];
        }

        public IMenuItem BuildMenuItem(SiteMapNode node)
        {
            Func<bool> isHideForSite = IsHideForSite(node);

            var menuItem = new MenuItem(
                menuItemTypeConfiguration[node.MenuItemType ?? string.Empty],
                authorizationService.IsAuthorized,
                isProtectedModeFunc,
                isInternalFunc,
                isHideForSite,
                buildLanguageQueryStringFunc,
                buildDarkModeQueryStringFunc,
                transferService,
                userSession,
                browserHelper);

            SetDataForMenuItem(menuItem, node);

            return menuItem;
        }

        private static Func<bool> IsHideForSite(SiteMapNode node) => () =>
          {
              var hideForSites = node.HideForSites == null ? Enumerable.Empty<string>() :
                  node.HideForSites.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
              return hideForSites
                  .Any(site => string.Equals(site, "*", StringComparison.OrdinalIgnoreCase) || string.Equals(site, UserSession.Current?.Site, StringComparison.OrdinalIgnoreCase));
          };
    }
}