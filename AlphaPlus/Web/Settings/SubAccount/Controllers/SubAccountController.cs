using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaPlus.Settings.Site.SubAccount.Models;
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

        public async Task<IActionResult> Index(SubAccountRequest subAccountRequest)
        {
            return View(await mediator.Send(subAccountRequest));
        }
    }
}