using AlphaPlus.Core;
using AlphaPlus.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Main.Site.Features.Tour
{
    public class TourController : BaseController
    {
        private readonly IUserSession userSession;

        public TourController(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        public ActionResult ProfilePicture()
        {
            var viewModel = new TourViewModel(userSession.Language, userSession.RoleId);

            return View(viewModel);
        }

        public ActionResult RebateAndRetention()
        {
            var viewModel = new TourViewModel(userSession.Language, userSession.RoleId);

            return View(viewModel);
        }
    }
}