using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Core.Models;
using MediatR;
using AlphaPlus.Core;

namespace Alpha.Main.Site.Features.TelegramChatBot
{
    using Alpha.Main.Site.Shared.Models;

    public class TelegramChatBotController(IMediator mediator, IUserSession userSession) : BaseController
    {
        private readonly IMediator mediator = mediator;
        private readonly IUserSession userSession = userSession;

        // TODO: PartialUM
        // protected override void OnActionExecuting(ActionExecutingContext filterContext)
        // {
        //     if (AppSettings.EnabledUMTelegramChatbot)
        //     {
        //         filterContext.Result = new RedirectResult($"~/PartialUM/Index?umfrom={AppSettings.TelegramChatbotUMFrom}&umto={AppSettings.TelegramChatbotUMTo}");
        //         return;
        //     }

        //     base.OnActionExecuting(filterContext);
        // }

        public async Task<IActionResult> Setup(SetUpChatBotQuery query)
        {
            var model = await mediator.Send(query);

            return model.IsLinked
                ? View("ChatBotSetting", new ChatBotSettingViewModel(model, query))
                : View("RegisterAccount", new RegisterAccountViewModel(model, query));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Unsubscribe(UnsubscribeCommand command)
        {
            if (userSession.IsInternal)
            {
                return Json(UnauthorizedResult.CreateUnauthorizedResult());
            }

            return Json(await mediator.Send(command));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateQRCode(GenerateQRCodeCommand command)
        {
            if (userSession.IsInternal)
            {
                return Json(UnauthorizedResult.CreateUnauthorizedResult());
            }

            var viewModel = await mediator.Send(command);

            return PartialView("VerifyAccount", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyOTP(VerifyOTPCommand command)
        {
            if (userSession.IsInternal)
            {
                return Json(UnauthorizedResult.CreateUnauthorizedResult());
            }

            // TODO: captcha mvc
            // var isCaptchaValid = this.IsCaptchaValid(null);
            // if (command.IsShowCaptcha && !isCaptchaValid)
            // {
            //     return Json(Result.CreateErrorResult(ChatBot.InvalidCaptcha));
            // }

            var result = await mediator.Send(command);
            // if (result.IsNotOk && result.Code == -3 && !isCaptchaValid)
            // {
            //     return PartialView("Captcha");
            // }

            return Json(result);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ResendOTP(ResendOTPCommand command)
        {
            if (userSession.IsInternal)
            {
                return Json(UnauthorizedResult.CreateUnauthorizedResult());
            }

            return Json(await mediator.Send(command));
        }

        [HttpGet]
        public IActionResult Captcha() => PartialView();

        [HttpPost]
        public async Task<IActionResult> UpdateFunction(UpdateFunctionCommand command)
        {
            if (userSession.IsInternal)
            {
                return Json(UnauthorizedResult.CreateUnauthorizedResult());
            }

            return Json(await mediator.Send(command));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateLanguage(UpdateLanguageCommand command)
        {
            if (userSession.IsInternal || string.IsNullOrWhiteSpace(command.Language))
            {
                return Json(UnauthorizedResult.CreateUnauthorizedResult());
            }

            return Json(await mediator.Send(command));
        }
    }
}