using MediatR;

namespace AlphaPlus.Main.Site.Features.Default.MenuBar
{
    public class MenuBarHandler(
        ISiteMenuService siteMenuService) :
        IRequestHandler<MenuBarQuery, MenuBarViewModel>
    {
        private readonly ISiteMenuService siteMenuService = siteMenuService;

        public Task<MenuBarViewModel> Handle(MenuBarQuery message, CancellationToken cancellationToken)
        {
            var menuItems = siteMenuService.GetMenuByUserLevel(message.UserLevel);

            var viewModel = new MenuBarViewModel(
                menuItems,
                message.IsSmallDevice,
                isDarkMode: !string.IsNullOrWhiteSpace(message.DarkModeName));

            return Task.FromResult(viewModel);
        }
    }
}