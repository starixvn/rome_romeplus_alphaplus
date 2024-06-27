namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.ParlayMaxPayout
{
    using Controllers;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Requests;

    public class ParlayMaxPayoutController : SiteBaseController
    {
        private const string editSinglePath = "~/Features/MemberInfo/Features/ParlayMaxPayout/Views/EditSingle.cshtml";
        private const string editMultiplePath = "~/Features/MemberInfo/Features/ParlayMaxPayout/Views/EditMultiple.cshtml";

        public ParlayMaxPayoutController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> EditSingle(ParlayMaxPayoutSingleSelectQuery message)
            => await View(message, editSinglePath);

        [HttpPost]
        public async Task<ActionResult> EditMultiple(ParlayMaxPayoutMultipleSelectQuery message)
            => await View(message, editMultiplePath);

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateSingle([FromBody]ParlayMaxPayoutUpdateSingleQuery message)
            => await Json(message);

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateMultiple([FromBody]ParlayMaxPayoutUpdateMultipleQuery message)
            => await Json(message);

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> CheckDownline([FromBody]ParlayMaxPayoutCheckDownlineQuery message)
            => await Json(message);
    }
}