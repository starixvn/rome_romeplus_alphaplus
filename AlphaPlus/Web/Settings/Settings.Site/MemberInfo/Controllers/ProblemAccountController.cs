namespace Alpha.MemberInfo.Site.Controllers
{
    using System.Threading.Tasks;
    using Alpha.MemberInfo.Site.Requests.ProblemAccount;
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

        public ActionResult Index(IndexRequest message)
            => View(message, "Index");

        [HttpGet]
        public ActionResult Update(UpdateProblemAccountRequest message)
            => View(message, "Update");
        

        public ActionResult GetProblemAccounts(GetProblemAccountRequest message)
            => Json(message);

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult UpdateCommand(UpdateProblemAccountCommand message)
            => Json(message);

        public ActionResult Excel(ProblemAccountExcelQuery message)
            => ExcelView(message);
    }
}