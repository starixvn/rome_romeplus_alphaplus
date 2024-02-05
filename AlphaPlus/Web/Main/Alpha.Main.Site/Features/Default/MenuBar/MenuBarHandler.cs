using MediatR;
using Alpha.Main.Services;

namespace Alpha.Main.Site.Features.Default.MenuBar
{
    public class MenuBarHandler(
        SiteMenuService siteMenuService) :
        IRequestHandler<MenuBarQuery, MenuBarViewModel>
    {
        private readonly SiteMenuService siteMenuService = siteMenuService;

        public Task<MenuBarViewModel> Handle(MenuBarQuery message, CancellationToken cancellationToken)
        {
            var menuItems = siteMenuService.GetMenuByUserLevel(message.UserLevel, message.IsCookieLess);

            var viewModel = new MenuBarViewModel(
                menuItems,
                message.IsSmallDevice,
                isDarkMode: !string.IsNullOrWhiteSpace(message.DarkModeName),
                isHideBanner: message.IsHideBanner);

            return Task.FromResult(viewModel);
        }
    }
}