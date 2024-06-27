using AlphaPlus.Core;
using AlphaPlus.Core.Models;
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

        public SubAccountController(IMediator mediator)
        {
            this.mediator = mediator;
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
                Username = $"{UserSession.Current.UserName}Sub",
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