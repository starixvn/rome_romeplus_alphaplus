using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Core.Models;
using MediatR;
using Alpha.Main.AppResources;
using AlphaPlus.SecurityCode.Web;
using Alpha.Main.Configurations;
using AlphaPlus.Core.Filters;
using AlphaPlus.Core;
using Alpha.Main.Site.Features.Language;

namespace Alpha.Main.Site.Features.SecurityCode
{
    [ProtectedModeFilter]
    public class SecurityCodeController(IMediator mediator, IUserSession userSession) : BaseController
    {
        private readonly IMediator mediator = mediator;
        private readonly IUserSession userSession = userSession;

        [HttpGet]
        [AllowProtectedMode]
        public async Task<IActionResult> ChangeSecurityCodeFirstLogin(ForceChangeSecurityCodeQuery query)
        {
            if (!string.IsNullOrWhiteSpace(userSession.SecurityCode))
            {
                return Redirect("~/");
            }

            var viewModel = await mediator.Send(query);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowProtectedMode]
        public async Task<IActionResult> ChangeSecurityCodeFirstLogin(ChangeSecurityCodeFirstLoginCommand query)
        {
            if (!string.IsNullOrWhiteSpace(userSession.SecurityCode))
            {
                return Json(Result.CreateErrorResult(Security.NotAuthorized));
            }

            var viewModel = await mediator.Send(query);

            return Json(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ChangeSecurityCodeByForcing(ForceChangeSecurityCodeQuery query)
        {
            if (!userSession.IsForcedSecurityCode)
            {
                return Redirect("~/");
            }

            var viewModel = await mediator.Send(query);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeSecurityCodeByForcing(ChangeSecurityCodeByForcingCommand query)
        {
            if (!userSession.IsForcedSecurityCode)
            {
                return Json(Result.CreateErrorResult(Security.NotAuthorized));
            }

            var viewModel = await mediator.Send(query);

            return Json(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ChangeUserOwnSecurityCode(SecurityCodeQuery query)
        {
            var viewModel = await mediator.Send(query);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserOwnSecurityCode(ChangeUserOwnSecurityCodeCommand query)
        {
            var viewModel = await mediator.Send(query);

            return Json(viewModel);
        }

        [HttpGet, CheckBelongingFilter, AllowProtectedMode]
        public async Task<IActionResult> ResetSecurityCode(SecurityCodeQuery query)
        {
            var viewModel = await mediator.Send(query);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken, CheckBelongingFilter, AllowProtectedMode]
        public async Task<IActionResult> ResetSecurityCode(ResetSecurityCodeCommand query)
        {
            var viewModel = await mediator.Send(query);

            return Json(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> VerifySecurityCode(SecurityCodeQuery query)
        {
            if (SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp)
            {
                return Redirect("~/Otp/VerifyOtp");
            }

            var viewModel = await mediator.Send(query);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifySecurityCode(VerifySecurityCodeCommand query)
        {
            if (SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp)
            {
                return Json(Result.CreateErrorResult(Security.NotAuthorized));
            }

            var viewModel = await mediator.Send(query);

            return Json(viewModel);
        }

        [AllowProtectedMode]
        public async Task<IActionResult> Languages()
        {
            var viewModel = await mediator.Send(new AllLanguagesQuery(userSession.Language, AppSettings.DisabledVietnamese));
            var domain = Request.Headers.Host.ToString();
            var subDomain = domain.Split('.')[0];
            var mainDomain = domain.Replace(subDomain, string.Empty);

            var cookieOptions = new CookieOptions() { Expires = DateTime.Now.AddDays(365), Path = "/", Domain = mainDomain, HttpOnly = false };
            Response.Cookies.Append("LANGUAGE", viewModel.CurrentLanguage, cookieOptions);
            Response.Cookies.Append("hidLanguage", viewModel.CurrentLanguage, cookieOptions);

            return PartialView(viewModel);
        }

        public IActionResult LearnMore()
            => View($"~/Views/SecurityCode/LearnMore/SecurityCode_{userSession.Language}.cshtml");

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSecurityCodeFunction(UpdateSecurityCodeFunctionCommand query)
            => Json(await mediator.Send(query));
    }
}