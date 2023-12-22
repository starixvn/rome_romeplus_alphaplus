using Alpha.Main.Helpers;

namespace Alpha.Main.Models.Menus
{
    public interface MenuItemBuilder
    {
        IMenuItem BuildMenuItem(SiteMapNode node);
    }
}
