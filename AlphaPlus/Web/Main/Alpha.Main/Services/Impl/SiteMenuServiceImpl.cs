using AlphaPlus.Core.Models;
using Alpha.Main.Helpers;
using Alpha.Main.Models.Menus;
using Fanex.Caching;

namespace Alpha.Main.Services.Impl
{
    public class SiteMenuServiceImpl : SiteMenuService
    {
        private const string CacheKeyMenuByUserLevel = "Alpha.Main.MenuForLevel.";

        // private readonly CachingManager cacheManager;
        private readonly ICacheService cacheService;
        private readonly MenuItemBuilder menuItemBuilder;
        private readonly SiteMapReader siteMapReader;


        public SiteMenuServiceImpl(
            // CachingManager cacheManager,
            ICacheService cacheService,
            MenuItemBuilder menuItemBuilder,
            SiteMapReader siteMapReader)
        {

            // this.cacheManager = cacheManager;
            this.cacheService = cacheService;
            this.menuItemBuilder = menuItemBuilder;
            this.siteMapReader = siteMapReader;
        }

        public IEnumerable<IMenuItem> GetMenuByUserLevel(UserLevel userLevel, bool isCookieLess = false)
        {
            //var menuItems = Enumerable.Empty<IMenuItem>();

            var menuItems = GetMenuDataByUserLevel(userLevel, isCookieLess);

            return menuItems;
        }

        private List<IMenuItem> GetMenuDataByUserLevel(UserLevel userLevel, bool isCookieLess)
        {
            var cacheKey = CacheKeyMenuByUserLevel + userLevel.DisplayName;

            List<IMenuItem> menuItems = cacheService.Get<List<IMenuItem>>(cacheKey);

            if (menuItems == null || isCookieLess)
            {
                var rootNode = siteMapReader.GetRootNode();

                menuItems = BuildMenus(rootNode.ChildNodes, userLevel);

                if (!isCookieLess)
                {
                    cacheService.Set(cacheKey, menuItems);
                }
            }

            return menuItems;
        }

        private List<IMenuItem> BuildMenus(SiteMapNode[] siteMapNodes, UserLevel userLevel)
        {
            var menuItems = new List<IMenuItem>();

            if (siteMapNodes != null && siteMapNodes.Length > 0)
            {
                siteMapNodes.ToList().ForEach(node =>
                {
                    var roles = node.Roles?.Split(",");
                    if (roles == null
                        || roles.Length == 0
                        || roles.Contains(userLevel.DisplayName)
                        || roles.Contains(userLevel.DisplayName.ToLowerInvariant())
                        || roles.Contains(userLevel.Value.ToString()))
                    {
                        var menuItem = menuItemBuilder.BuildMenuItem(node);

                        if (node.HasChildNodes)
                        {
                            menuItem.SubMenus = BuildMenus(node.ChildNodes, userLevel);
                        }

                        menuItems.Add(menuItem);
                    }
                });
            }

            return menuItems;
        }
    }
}