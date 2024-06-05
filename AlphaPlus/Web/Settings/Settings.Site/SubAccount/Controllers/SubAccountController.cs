using AlphaPlus.Core;
using AlphaPlus.Settings.Site.SubAccount.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Settings.Site.SubAccount.Controllers
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
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddSubAccountRequest subAccountRequest)
        {
            return Json(await mediator.Send(subAccountRequest));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSubAccountInfo(UpdateSubAccountInfoRequest subAccountRequest)
        {
            return Json(await mediator.Send(subAccountRequest));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSubAccountPageAccess(UpdateSubAccountPageAccessRequest subAccountRequest)
        {
            return Json(await mediator.Send(subAccountRequest));
        }
    }
}