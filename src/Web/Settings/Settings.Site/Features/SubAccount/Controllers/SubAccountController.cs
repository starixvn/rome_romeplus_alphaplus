using AlphaPlus.Core;
using AlphaPlus.Core.Models;
using AlphaPlus.Core.Utils;
using AlphaPlus.Settings.Site.Features.SubAccount.Models;
using AlphaPlus.Settings.Site.Features.SubAccount.Requests;
using AlphaPlus.Shared.AppResources.Settings.SubAccount;
using Fanex.Cryptography.Aes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Settings.Site.Features.SubAccount.Controllers
{
    public class SubAccountController : BaseController
    {
        private readonly IMediator mediator;

        private readonly IUserSession userSession;

        private readonly IAesService aesService;

        public SubAccountController(IMediator mediator, IEncryption encryption, IUserSession userSession, IAesService aesService)
        {
            this.mediator = mediator;
            this.userSession = userSession;
            this.aesService = aesService;
            
            userSession.CaptchaCode = string.IsNullOrWhiteSpace(userSession.CaptchaCode) 
                ? encryption.RandomString(4)
                : userSession.CaptchaCode;
        }

        public async Task<IActionResult> Index(SubAccountListRequest subAccountListRequest)
        {
            return View(await mediator.Send(subAccountListRequest));
        }

        public async Task<IActionResult> Get(SubAccountRequest subAccountRequest)
        {
            return View(await mediator.Send(subAccountRequest));
        }

        public IActionResult Add()
        {
            ViewBag.Title = Lang.addsubaccount;
            var model = new SubAccountViewModel
            {
                Username = $"{userSession.UserName}Sub",
                PageAccess = string.Empty
            };

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddSubAccountRequest subAccountRequest)
        {
            if (!subAccountRequest.IsValid())
            {
                return Json(new Result(1, Lang.lblNotAuthorized));
            }

            var rawPassword = aesService.Decrypt(subAccountRequest.Password, userSession.CaptchaCode);

            if (string.Compare(rawPassword, "59D7!4$h", true) == 0 || string.Compare(rawPassword, "493abcDE", true) == 0)
            {
                return Json(new Result(1, Lang.lblPasswordCSample));
            }

            if (rawPassword.Contains($"{subAccountRequest.Username}{subAccountRequest.First}{subAccountRequest.Next}", StringComparison.OrdinalIgnoreCase))
            {
                return Json(new Result(1, Lang.lblPasswordDAccountName));
            }

            subAccountRequest.Password = rawPassword;

            return Json(await mediator.Send(subAccountRequest));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateInfo(UpdateSubAccountInfoRequest subAccountRequest)
        {
            if (!subAccountRequest.IsValid())
            {
                return Json(new Result(1, Lang.lblNotAuthorized));
            }

            return Json(await mediator.Send(subAccountRequest));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePageAccess(UpdateSubAccountPageAccessRequest subAccountRequest)
        {
            if (!subAccountRequest.IsValid())
            {
                return Json(new Result(1, Lang.lblNotAuthorized));
            }

            return Json(await mediator.Send(subAccountRequest));
        }
    }
}