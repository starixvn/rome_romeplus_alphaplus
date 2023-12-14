using Alpha.Main.Services;
using Alpha.Main.Site.Requests.Theme;
using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Models;
using MediatR;

namespace Alpha.Main.Site.Handlers
{
    public class ThemeHandler(IUserSession userSession, ThemeService themeService) :
        IRequestHandler<ChangeThemeQuery, bool>,
        IRequestHandler<ResetAssetAppPathQuery, bool>
    {
        private readonly IUserSession userSession = userSession;
        private readonly ThemeService themeService = themeService;

        public async Task<bool> Handle(ChangeThemeQuery request, CancellationToken cancellationToken)
        {
            userSession.DarkModeName = request.IsDarkMode ? "-dark" : string.Empty;

            if (!request.IsInternal)
            {
                await themeService.WriteLog(
                    request.CustId,
                    request.SubAccId,
                    request.IsDarkMode,
                    request.IP,
                    request.Location);
            }

            return true;
        }

        public Task<bool> Handle(ResetAssetAppPathQuery request, CancellationToken cancellationToken)
        {
            userSession.AssetAppPath = AppSettings.AssetAppPath;

            return Task.FromResult(true);
        }
    }
}