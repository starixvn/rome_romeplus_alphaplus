using Alpha.Main.Site.Helpers;
using Alpha.Main.Site.Models.Requests;
using AlphaPlus.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Alpha.Main.Configurations;

namespace Alpha.Main.Site.ViewModels;

public class MenuBarViewComponent(IMediator mediator, IUserSession userSession) : ViewComponent
{
    private readonly IMediator mediator = mediator;
    private readonly IUserSession userSession = userSession;

    // [AllowProtectedMode]
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var viewModel = await mediator.Send(new MenuBarQuery(
            userSession.RoleId,
            Request.Headers.UserAgent.ToString(),
            darkModeName: DarkModeHelper.GetDarkModeName(userSession),
            isHideBanner: AppSettings.BannerIsHide || userSession.IsProtectedMode));

        return View(viewModel);
    }
}