using AlphaPlus.Core.Models;
using Microsoft.Extensions.Caching.Memory;

namespace AlphaPlus.Main.Site.Features.Default.MenuBar
{
    public interface ISiteMenuService
    {
        IEnumerable<IMenuItem> GetMenuByUserLevel(UserLevel userLevel);
    }

    public class SiteMenuService : ISiteMenuService
    {
        private const string CacheKeyMenuByUserLevel = "Alpha.Main.MenuForLevel.";

        private readonly IMemoryCache cacheService;
        private readonly IMenuItemBuilder menuItemBuilder;
        private readonly ISiteMapReader siteMapReader;


        public SiteMenuService(
            IMemoryCache cacheService,
            IMenuItemBuilder menuItemBuilder,
            ISiteMapReader siteMapReader)
        {

            this.cacheService = cacheService;
            this.menuItemBuilder = menuItemBuilder;
            this.siteMapReader = siteMapReader;
        }

        public IEnumerable<IMenuItem> GetMenuByUserLevel(UserLevel userLevel)
        {
            var menuItems = GetMenuDataByUserLevel(userLevel);

            return menuItems;
        }

        private List<IMenuItem> GetMenuDataByUserLevel(UserLevel userLevel)
        {
            var cacheKey = CacheKeyMenuByUserLevel + userLevel.DisplayName;

            List<IMenuItem> menuItems = cacheService.Get<List<IMenuItem>>(cacheKey);

            if (menuItems == null)
            {
                var rootNode = siteMapReader.GetRootNode();
                menuItems = BuildMenus(rootNode.ChildNodes, userLevel);
                cacheService.Set(cacheKey, menuItems);
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