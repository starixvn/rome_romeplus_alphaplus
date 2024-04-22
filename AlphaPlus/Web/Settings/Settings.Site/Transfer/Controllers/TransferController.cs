using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Models;
using MediatR;
using AlphaPlus.Core;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AlphaPlus.Settings.Site.Transfer.Controllers
{
    public class TransferController : SiteBaseController
    {
        public TransferController(IMediator mediator) : base(mediator) {}

        public async Task<IActionResult> SuperList() => View();

        public async Task<IActionResult> MasterList() => View();

        public async Task<IActionResult> AgentList() => View();
    }
}
