namespace AlphaPlus.Settings.Site.Controllers
{
    using System.Threading.Tasks;
    using AlphaPlus.Settings.Site.Requests.ProblemAccount;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    public class ProblemAccountController : SiteBaseController
    {
        private const string ConfirmAddToWatchlistPath = "~/Views/ProblemAccount/ConfirmAddToWatchlist.cshtml";

        public ProblemAccountController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> ConfirmAddToWatchlist(ConfirmAddToWatchlistQuery message)
            => await ViewAsync(message, ConfirmAddToWatchlistPath);

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