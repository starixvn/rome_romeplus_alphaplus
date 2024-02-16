namespace Alpha.Main.Site.Features.Default.MenuBar
{
    public interface IMenuItem
    {
        string ResourceKey { get; set; }

        string DisplayName { get; }

        string Link { get; set; }

        bool ByPassAuthorization { get; set; }

        bool ByPassProtectedMode { get; set; }

        bool ByPassProtectedModeInternal { get; set; }

        bool IsBeta { get; set; }

        bool IsNew { get; set; }

        bool IsOld { get; set; }

        bool IsMobileView { get; }

        bool IsHiddenInMobile { get; set; }

        MenuStatus Status { get; }

        bool ShowSubMenus { get; set; }

        List<IMenuItem> SubMenus { get; set; }

        IEnumerable<IMenuItem> VisibleSubMenus { get; }
    }
}
