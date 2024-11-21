using AlphaPlus.Core;
using AlphaPlus.Core.Models;
using AlphaPlus.Core.Utils;
using AlphaPlus.Settings.Site.Features.SubAccount.Models;
using AlphaPlus.Settings.Site.Features.SubAccount.Requests;
using AlphaPlus.Shared.AppResources.Settings.SubAccount;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Settings.Site.Features.SubAccount.Controllers
{
    public class SubAccountController : BaseController
    {
        private readonly IMediator mediator;

        private readonly IUserSession userSession;

        public SubAccountController(IMediator mediator, IEncryption encryption, IUserSession userSession)
        {
            this.mediator = mediator;
            this.userSession = userSession;
            
            userSession.CaptchaCode = encryption.RandomString(4);
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

            if (string.Compare(subAccountRequest.Password, "59D7!4$h", true) == 0 || string.Compare(subAccountRequest.Password, "493abcDE", true) == 0)
            {
                return Json(new Result(1, Lang.lblPasswordCSample));
            }

            if (subAccountRequest.Password.Contains($"{subAccountRequest.Username}{subAccountRequest.First}{subAccountRequest.Next}", StringComparison.OrdinalIgnoreCase))
            {
                return Json(new Result(1, Lang.lblPasswordDAccountName));
            }

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