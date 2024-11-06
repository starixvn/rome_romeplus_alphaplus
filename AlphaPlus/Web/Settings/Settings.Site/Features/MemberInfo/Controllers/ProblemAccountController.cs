using AlphaPlus.Settings.Site.Features.MemberInfo.Requests.ProblemAccount;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Controllers
{
    public class ProblemAccountController(IMediator mediator) : SiteBaseController(mediator)
    {
        public async Task<ActionResult> Index(IndexRequest message)
            => await View(message, "Index");

        [HttpGet]
        public async Task<ActionResult> Update(UpdateProblemAccountRequest message)
            => await View(message, "Update");


        public async Task<ActionResult> GetProblemAccounts(GetProblemAccountRequest message)
            => await Json(message);

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateCommand(UpdateProblemAccountCommand message)
            => await Json(message);

        public async Task<ActionResult> Excel(ProblemAccountExcelQuery message)
            => await ExcelView(message);
    }
}