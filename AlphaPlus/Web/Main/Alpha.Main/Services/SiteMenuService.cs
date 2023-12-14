namespace Alpha.Main.Services
{
    using AlphaPlus.Core.Models;
    using Models.Menus;
    using System.Collections.Generic;

    public interface SiteMenuService
    {
        IEnumerable<IMenuItem> GetMenuByUserLevel(UserLevel userLevel, bool isCookieLess = false);
    }
}
