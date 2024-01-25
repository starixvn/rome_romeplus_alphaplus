using AlphaPlus.Settings.Site.SubAccount.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Settings.Site.SubAccount.Controllers
{
    public class SubAccountController : Controller
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

        [HttpPost]
        public async Task<IActionResult> UpdateSubAccountInfo(UpdateSubAccountInfoRequest subAccountRequest)
        {
            return View(await mediator.Send(subAccountRequest));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSubAccountPassword(UpdateSubAccountPasswordRequest subAccountRequest)
        {
            return View(await mediator.Send(subAccountRequest));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSubAccountPageAccess(UpdateSubAccountPageAccessRequest subAccountRequest)
        {
            return View(await mediator.Send(subAccountRequest));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSubAccountSecurityCode(UpdateSubAccountInfoRequest subAccountRequest)
        {
            return View(await mediator.Send(subAccountRequest));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveSubAccountOtp(RemoveSubAccountOtpRequest subAccountRequest)
        {
            return View(await mediator.Send(subAccountRequest));
        }
    }
}