using Alpha.Main.Models.Menus;
using MediatR;
using Alpha.Main.Site.Models.Requests;
using Alpha.Main.Services;
using Alpha.Main.Site.ViewModels;

namespace Alpha.Main.Site.Handlers
{
    public class MenuBarHandler(
        SiteMenuService siteMenuService) :
        IRequestHandler<MenuBarQuery, MenuBarViewModel>
    {
        private readonly SiteMenuService siteMenuService = siteMenuService;

        public Task<MenuBarViewModel> Handle(MenuBarQuery message, CancellationToken cancellationToken)
        {
            var menuItems = Enumerable.Empty<IMenuItem>();
            // TODO: menu
            // var menuItems = siteMenuService.GetMenuByUserLevel(message.UserLevel, message.IsCookieLess);

            var viewModel = new MenuBarViewModel(
                menuItems,
                message.IsSmallDevice,
                isDarkMode: !string.IsNullOrWhiteSpace(message.DarkModeName),
                isHideBanner: message.IsHideBanner);

            return Task.FromResult(viewModel);
        }
    }
}