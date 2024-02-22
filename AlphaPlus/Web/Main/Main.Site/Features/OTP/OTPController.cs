using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Core.Models;
using AlphaPlus.Main.Site.AppResources;
using MediatR;
using AlphaPlus.SecurityCode.Web;
using AlphaPlus.Core.Filters;
using AlphaPlus.Core;
using AlphaPlus.Main.Site.Features.TelegramChatBot;

namespace AlphaPlus.Main.Site.Features.OTP
{
    using AlphaPlus.Main.Site.Shared.Models;

    public class OtpController(IMediator mediator, IUserSession userSession) : BaseController
    {
        private readonly IMediator mediator = mediator;

        public async Task<IActionResult> SetUpOtp(SetUpOtpQuery query)
        {
            var customerOtp = await mediator.Send(query);

            return View(customerOtp);
        }

        [HttpGet]
        public async Task<IActionResult> VerifyOtp(VerifyOtpQuery query)
        {
            if (!SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp)
            {
                return Redirect("~/SecurityCode/VerifySecurityCode");
            }

            var verifyOtp = await mediator.Send(query);

            return View(verifyOtp);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> VerifyOtp(VerifyOtpCommand command)
        {
            if (userSession.IsInternal)
            {
                return UnauthorizedResponse();
            }

            if (!SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp)
            {
                return Json(Result.CreateErrorResult(Security.NotAuthorized));
            }

            var result = await mediator.Send(command);

            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> VerifyOtpDifferenceDevice()
        {
            var viewModel = await mediator.Send(new VerifyOtpDifferentDeviceQuery(userSession));

            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> SetUpOtp(SetUpOtpCommand command)
        {
            if (userSession.IsInternal)
            {
                return UnauthorizedResponse();
            }

            var result = await mediator.Send(command);

            return Json(result);
        }

        [HttpGet, AllowProtectedMode]
        public async Task<IActionResult> ResetOtp(ResetOtpQuery query)
        {
            var viewModel = await mediator.Send(query);

            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken, AllowProtectedMode]
        public async Task<IActionResult> ResetOtp(ResetOtpCommand command)
        {
            if (userSession.IsInternal)
            {
                return UnauthorizedResponse();
            }

            var result = await mediator.Send(command);

            return View(result);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> RemoveOtp()
        {
            if (userSession.IsInternal)
            {
                return UnauthorizedResponse();
            }

            var result = await mediator.Send(new RemoveOtpCommand(userSession));

            return Json(result);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> ChangeOtpStatus(ChangeOtpStatusCommand command)
        {
            if (userSession.IsInternal)
            {
                return UnauthorizedResponse();
            }

            var result = await mediator.Send(command);

            return Json(result);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateOTP()
        {
            if (userSession.IsInternal)
            {
                return UnauthorizedResponse();
            }

            return Json(await mediator.Send(new GenerateOTPCommand(userSession)));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOtpFunction(UpdateOtpTelegramCommand command)
        {
            if (userSession.IsInternal)
            {
                return UnauthorizedResponse();
            }

            return Json(await mediator.Send(command));
        }

        private JsonResult UnauthorizedResponse() => Json(UnauthorizedResult.CreateUnauthorizedResult());
    }
}