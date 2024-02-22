using AlphaPlus.Core.Models;
using Fanex.Caching;

namespace AlphaPlus.Main.Site.Features.Default.MenuBar
{
    public interface ISiteMenuService
    {
        IEnumerable<IMenuItem> GetMenuByUserLevel(UserLevel userLevel, bool isCookieLess = false);
    }

    public class SiteMenuService : ISiteMenuService
    {
        private const string CacheKeyMenuByUserLevel = "Alpha.Main.MenuForLevel.";

        private readonly ICacheService cacheService;
        private readonly IMenuItemBuilder menuItemBuilder;
        private readonly ISiteMapReader siteMapReader;


        public SiteMenuService(
            ICacheService cacheService,
            IMenuItemBuilder menuItemBuilder,
            ISiteMapReader siteMapReader)
        {

            this.cacheService = cacheService;
            this.menuItemBuilder = menuItemBuilder;
            this.siteMapReader = siteMapReader;
        }

        public IEnumerable<IMenuItem> GetMenuByUserLevel(UserLevel userLevel, bool isCookieLess = false)
        {
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