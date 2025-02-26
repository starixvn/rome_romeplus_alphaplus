using AlphaPlus.Core;
using AlphaPlus.Core.Models;
using AlphaPlus.Main.Site.Features.NovaCasino.Models;
using AlphaPlus.Main.Site.Shared.Models;
using AlphaPlus.Shared.AppResources.Main;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Main.Site.Features.NovaCasino
{
    public class NovaCasinoController : BaseController
    {
        private readonly IUser user;
        private readonly IUserSession userSession;
        private readonly ICustomerService customerService;
        private readonly INovaCasinoService novaCasinoService;

        public NovaCasinoController(
            IUser user,
            IUserSession userSession,
            ICustomerService customerService,
            INovaCasinoService novaCasinoService)
        {
            this.user = user;
            this.userSession = userSession;
            this.customerService = customerService;
            this.novaCasinoService = novaCasinoService;
        }

        [HttpPost]
        public async Task<ActionResult> SSO()
        {
            if (userSession.IsInternal
                || userSession.IsSubAcc
                || !NovaCasinoSettings.IsOfferedNovaCasino(
                    userSession.Site.ToUpperInvariant(),
                    Enumeration.FromValue<UserLevel>(userSession.RoleId).DisplayName,
                    userSession.Currency.ToUpperInvariant()))
            {
                return Json(Result.CreateErrorResult(Security.NotAuthorized));
            }

            var uplines = await customerService.GetUplines(userSession.CustId);
            var request = new NovaCasinoSSORequest
            {
                UserInfo = new UserInfo
                {
                    CustId = userSession.CustId,
                    Username = userSession.UserName,
                    CurrencyId = userSession.CurrencyId,
                    RoleId = userSession.RoleId,
                    Upline = uplines.ToArray()
                }
            };

            var response = await novaCasinoService.SSO(request);
            if (response.IsFailed)
            {
                return Json(response);
            }

            if (response.CustIds != null && response.CustIds.Any())
            {
                await customerService.UpdateSyncedNovaCasino(response.CustIds, userSession.UserLoginIp, userSession.LangKey);
            }

            if (!user.IsSyncedNovaCasino)
            {
                user.IsSyncedNovaCasino = true;
                if (!Enumeration.FromValue<UserLevel>(userSession.RoleId).IsSuperLevel())
                {
                    await customerService.InsertPersonalMessage(request.UserInfo);
                }
            }

            return Json(new NovaCasinoSSOViewModel { Code = 0, LaunchUrl = response.LaunchUrl });
        }
    }
}