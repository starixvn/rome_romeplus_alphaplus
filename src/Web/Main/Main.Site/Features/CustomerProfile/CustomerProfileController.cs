using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Core.Models;
using MediatR;
using AlphaPlus.Core.Filters;
using AlphaPlus.Core;

namespace AlphaPlus.Main.Site.Features.CustomerProfile
{
    public class CustomerProfileController(IUserSession userSession, IMediator mediator) : BaseController
    {
        private readonly IUserSession userSession = userSession;
        private readonly IMediator mediator = mediator;

        [AllowProtectedMode]
        public async Task<IActionResult> BriefProfile()
        {
            var query = new BriefProfileQuery(userSession);

            var viewModel = await mediator.Send(query);

            return View(viewModel);
        }
    }
}