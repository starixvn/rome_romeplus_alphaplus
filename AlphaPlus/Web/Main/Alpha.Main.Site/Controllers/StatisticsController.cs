using Microsoft.AspNetCore.Mvc;
using AlphaPlus.Core.Models;
using MediatR;
using Alpha.Main.Site.Requests.Dashboard.Statistics;

namespace Alpha.Main.Site.Controllers
{
    public class StatisticsController(IMediator mediator, IUserSession userSession) : Controller
    {
        private readonly IMediator mediator = mediator;
        private readonly IUserSession userSession = userSession;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var viewModel = await mediator.Send(new StatisticsQuery(userSession));

            return PartialView(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> TopWinningMembers()
        {
            var viewModel = await mediator.Send(new TopWinningMembersQuery(userSession));

            return PartialView("TopWinningMembers", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> TopTurnoverMembers()
        {
            var viewModel = await mediator.Send(new TopTurnoverMembersQuery(userSession));

            return PartialView("TopTurnoverMembers", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> NewMembers()
        {
            var viewModel = await mediator.Send(new NewMembersQuery(userSession.CustId));

            return PartialView("NewMembers", viewModel);
        }
    }
}