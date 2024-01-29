using Alpha.Main.Site.Models.Requests;
using AlphaPlus.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Alpha.Main.Configurations;

namespace Alpha.Main.Site.ViewComponents
{
    public class LanguageListViewComponent(IMediator mediator, IUserSession userSession) : ViewComponent
    {
        private readonly IUserSession userSession = userSession;
        private readonly IMediator mediator = mediator;
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var viewModel = await mediator.Send(new AllLanguagesQuery(userSession.Language, AppSettings.DisabledVietnamese));

            return View(viewModel);
        }
    }
}