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

        public WhiteLabelController(
            IUserSession userSession, 
            IMediator mediator)
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

        [HttpPost]
        public async Task<JsonResult> BannerChat(string username)
        { 
            var name = string.IsNullOrEmpty(username) ? "-" : username;
            var message = string.Format(Resources.WhiteLabelBannerChat, name);
            await Fanex.Logging.Logger.Log.InfoAsync(message);
            return Json(true);
        }

        [HttpPost]
        public async Task<JsonResult> BannerLog(string username)
        { 
            var name = string.IsNullOrEmpty(username) ? "-" : username;
            var message = string.Format(Resources.WhiteLabelBannerClick, name);
            await Fanex.Logging.Logger.Log.InfoAsync(message);
            return Json(true);
        }

        [HttpPost]
        public async Task<JsonResult> SkypeChat(string username)
        { 
            var name = string.IsNullOrEmpty(username) ? "-" : username;
            var message = string.Format(Resources.WhiteLabelSkypeChat, name);
            await Fanex.Logging.Logger.Log.InfoAsync(message);
            return Json(true);
        }
    }
}