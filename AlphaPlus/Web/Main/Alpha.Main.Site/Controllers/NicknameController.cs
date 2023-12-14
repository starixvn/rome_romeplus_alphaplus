using Microsoft.AspNetCore.Mvc;
using System.Net;
using AlphaPlus.Core.Models;
using Alpha.Main.AppResources;
using Alpha.Main.Models;
using MediatR;
using Alpha.Main.Site.Requests.Nickname;
using Alpha.Main.Site.ViewModels.Nickname;
using AlphaPlus.Core.Filters;
using AlphaPlus.Core;

namespace Alpha.Main.Site.Controllers
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
                ? await mediator.Send(message)
                : Result.CreateErrorResult(Security.NotAuthorized, (int)HttpStatusCode.Forbidden);

            return result.IsSuccessful
                ? ToDefaultPage()
                : View(new CreateNicknameViewModel(userSession, nickname) { Result = result });
        }

        private IActionResult ToDefaultPage()
            => RedirectToAction("Index", "Default");
    }
}