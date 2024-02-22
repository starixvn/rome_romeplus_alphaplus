using AlphaPlus.Core.Models;
using MediatR;

namespace Alpha.Main.Site.Features.Theme
{
    using AlphaPlus.Core.Configurations;

    public class ThemeHandler(IUserSession userSession) :
        IRequestHandler<ChangeThemeQuery, bool>,
        IRequestHandler<ResetAssetAppPathQuery, bool>
    {
        private readonly IUserSession userSession = userSession;

        public Task<bool> Handle(ChangeThemeQuery request, CancellationToken cancellationToken)
        {
            userSession.DarkModeName = request.IsDarkMode ? "-dark" : string.Empty;

            return Task.FromResult(true);
        }

        public Task<bool> Handle(ResetAssetAppPathQuery request, CancellationToken cancellationToken)
        {
            userSession.AssetAppPath = AppSettings.AssetAppPath;

            return Task.FromResult(true);
        }
    }
}