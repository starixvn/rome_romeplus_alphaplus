using Microsoft.AspNetCore.Mvc;
using System.Net;
using AlphaPlus.Core.Models;
using AlphaPlus.Main.Site.AppResources;
using MediatR;
using AlphaPlus.Core.Filters;
using AlphaPlus.Core;

namespace AlphaPlus.Main.Site.Features.Nickname
{
    public class NicknameController(IMediator mediator, IUserSession userSession) : BaseController
    {
        private readonly IMediator mediator = mediator;
        private readonly IUserSession userSession = userSession;

        [AllowProtectedMode]
        public IActionResult Index()
        {
            var viewModel = new CreateNicknameViewModel(userSession);

            return viewModel.IsValid() ? View(viewModel) : ToDefaultPage();
        }

        [HttpPost, ValidateAntiForgeryToken, AllowProtectedMode]
        public async Task<IActionResult> Index(string nickname)
        {
            var message = new CreateNicknameCommand(userSession)
            {
                Nickname = nickname
            };

            var result = message.IsValid()
                ? await mediator.Send(message, CancellationToken cancellation)
                : Result.CreateErrorResult(Security.NotAuthorized, (int)HttpStatusCode.Forbidden);

            return result.IsSuccessful
                ? ToDefaultPage()
                : View(new CreateNicknameViewModel(userSession, nickname) { Result = result });
        }

        private IActionResult ToDefaultPage()
            => RedirectToAction("Index", "Default");
    }
}