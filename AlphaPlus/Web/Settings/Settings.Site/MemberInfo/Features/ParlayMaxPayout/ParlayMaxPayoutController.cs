namespace AlphaPlus.Settings.Site.Features.ParlayMaxPayout
{
    using Controllers;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Requests;

    public class ParlayMaxPayoutController : SiteBaseController
    {
        private const string editSinglePath = "~/Features/ParlayMaxPayout/Views/EditSingle.cshtml";
        private const string editMultiplePath = "~/Features/ParlayMaxPayout/Views/EditMultiple.cshtml";

        public ParlayMaxPayoutController(IMediator mediator) : base(mediator)
        {
        }

        public ActionResult EditSingle(ParlayMaxPayoutSingleSelectQuery message)
            => View(message, editSinglePath);

        [HttpPost]
        public ActionResult EditMultiple(ParlayMaxPayoutMultipleSelectQuery message)
            => View(message, editMultiplePath);

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult UpdateSingle(ParlayMaxPayoutUpdateSingleQuery message)
            => Json(message);

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult UpdateMultiple(ParlayMaxPayoutUpdateMultipleQuery message)
            => Json(message);

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult CheckDownline(ParlayMaxPayoutCheckDownlineQuery message)
            => Json(message);
    }
}