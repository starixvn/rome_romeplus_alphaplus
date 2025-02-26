using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Core.Models;
using AlphaPlus.Shared.AppResources.Main;
using MediatR;
using AlphaPlus.Core.Filters;
using AlphaPlus.Core;

namespace AlphaPlus.Main.Site.Features.Password
{
    public class PasswordController(IMediator mediator, IUserSession userSession) : BaseController
    {
        private readonly IMediator mediator = mediator;
        private readonly IUserSession userSession = userSession;

        // GET: Password
        public async Task<IActionResult> ChangePassword(ChangePasswordQuery query)
        {
            var viewModel = await mediator.Send(query);
            return View(viewModel);
        }

        [AllowProtectedMode]
        public async Task<IActionResult> ForceChangePassword(ForceChangePasswordQuery query)
        {
            if (userSession.IsPasswordExpired || !userSession.IsLogged)
            {
                var viewModel = await mediator.Send(query);
                return View(viewModel);
            }

            return Redirect("~/");
        }

        [CheckBelongingFilter]
        public async Task<IActionResult> ResetMainAccountPassword(ResetMainAccountPasswordQuery query)
        {
            if (!query.IsValid())
            {
                return RedirectToAction("Index", "Error");
            }

            var viewModel = await mediator.Send(query);
            return View(viewModel);
        }

        [CheckBelongingFilter, AllowProtectedMode]
        public async Task<IActionResult> ResetSubAccountPassword(ResetSubAccountPasswordQuery query)
        {
            var viewModel = await mediator.Send(query);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordCommand command)
        {
            var result = await mediator.Send(command);
            return Json(result);
        }

        [HttpPost, ValidateAntiForgeryToken, AllowProtectedMode]
        public async Task<IActionResult> ForceChangePassword(ChangePasswordCommand command)
        {
            if (userSession.IsPasswordExpired || !userSession.IsLogged)
            {
                var result = await mediator.Send(command);
                return Json(result);
            }

            return Json(Result.CreateErrorResult(Security.NotAuthorized));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> SkipForceChangePassword(SkipForceChangePasswordCommand command)
        {
            var result = await mediator.Send(command);
            return Redirect(result.IsSuccessful ?
                Helpers.UrlHelper.MapMainPath("~/") :
                Helpers.UrlHelper.MapMainPath("~/_Authorization/Handlers/SignOut.ashx"));
        }

        [HttpPost, ValidateAntiForgeryToken, CheckBelongingFilter]
        public async Task<IActionResult> ResetMainAccountPassword(ResetMainAccountPasswordCommand command)
        {
            if (!command.IsValid())
            {
                return Json(Result.CreateErrorResult(Security.NotAuthorized));
            }

            var result = await mediator.Send(command);
            return Json(result);
        }

        [HttpPost, ValidateAntiForgeryToken, CheckBelongingFilter, AllowProtectedMode]
        public async Task<IActionResult> ResetSubAccountPassword(ResetSubAccountPasswordCommand command)
        {
            var result = await mediator.Send(command);
            return Json(result);
        }
    }
}