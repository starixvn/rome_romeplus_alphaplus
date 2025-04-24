using Microsoft.AspNetCore.Mvc;
using MediatR;
using AlphaPlus.Core;

namespace AlphaPlus.Main.Site.Features.TelegramChatBot
{
    using AlphaPlus.Core.Models;
    using AlphaPlus.Main.Site.Shared.Models;
    public class TelegramChatBotController(IMediator mediator, IUserSession userSession) : BaseController
    {
        private readonly IMediator mediator = mediator;
        private readonly IUserSession userSession = userSession;

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

            var result = await mediator.Send(command);

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