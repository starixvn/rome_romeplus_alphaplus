using AlphaPlus.Core;
using AlphaPlus.Core.Models;
using AlphaPlus.Shared.AppResources.Main;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Main.Site.Features.WhiteLabel
{
    public class WhiteLabelController : BaseController
    {
        private readonly IUserSession userSession;
        private readonly IMediator mediator;

        public WhiteLabelController(IUserSession userSession, IMediator mediator)
        {
            this.userSession = userSession;
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> SSO()
        {
            if (userSession.IsSubAcc
                || userSession.IsInternal
                || userSession.IsIndirectLoggedIn
                || !Enumeration.FromValue<UserLevel>(userSession.RoleId).IsAgentLevel())
            {
                return Json(Result.CreateErrorResult(Security.NotAuthorized));
            }

            var response = await mediator.Send(new WhiteLabelSSORequest(
                userSession.CustId.ToString(),
                Request.Headers["User-Agent"].ToString(),
                userSession.UserLoginIp));
            return Json(response);
        }
    }
}