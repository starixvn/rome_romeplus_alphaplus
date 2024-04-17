using System.Net;
using AlphaPlus.Core;
using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.Accounts;
using AlphaPlus.Settings.Site.Features.Accounts.Models;
using AlphaPlus.Settings.Site.Features.Accounts.ViewModels;
using AlphaPlus.Utilities.IP;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Settings.Site.Features.Account
{
    public class AccountsController(IUserSession userSession, IAccountService accountService, IPService ipService) : BaseController
    {
        private readonly IUserSession userSession = userSession;
        private readonly IAccountService accountService = accountService;
        private readonly IPService ipService = ipService;

        public ActionResult Index(bool mobileView)
        {
            return View(new IndexViewModel(userSession, mobileView));
        }

        [Route("/accounts/{username}")]
        public async Task<ActionResult> GetAccount(string username)
        {
            if (string.IsNullOrWhiteSpace(username)
            || !userSession.SubPageAccess.ViewMemberInfo
            || username.Trim().Equals(userSession.UserName, StringComparison.OrdinalIgnoreCase))
            {
                return ForbiddenResult();
            }

            var customer = await accountService.GetCustomerInfo(username, userSession.CustId, userSession.RoleId);

            if (customer != null)
            {
                var balanceInfo = BuildBalanceInfoViewModel(customer, await accountService.GetCreditInfo(customer.CustId),
                    await accountService.GetBalanceInfo(customer.CustId, customer.RoleId, userSession.AGIP),
                    await accountService.IsAllDayBalanceTransfer(userSession.CustId));

                var formatedIP = ipService.FormatIP(customer.LastLoginIP, userSession.IsAdminCanViewVNIP);

                customer.LastLoginIP = formatedIP.IP;
                customer.AllowOpenInfoPopup = formatedIP.AllowOpenInfoPopup;

                return Json(new CustomerInfoViewModel(
                    userSession,
                    customer,
                    balanceInfo,
                    await accountService.CheckFirstLoginByQRCode(customer.CustId)));
            }

            return Json(null);
        }

        internal IBalanceInfoViewModel BuildBalanceInfoViewModel(
            CustomerInfoModel customerInfo,
            CreditInfo creditInfo,
            IBalanceInfoModel balanceInfoModel,
            bool isAllDayBalanceInfo)
        {
            return customerInfo.RoleId switch
            {
                3 => new MasterBalanceInfoViewModel(userSession, customerInfo, creditInfo, balanceInfoModel, isAllDayBalanceInfo),
                2 => new AgentBalanceInfoViewModel(userSession, customerInfo, creditInfo, balanceInfoModel, isAllDayBalanceInfo),
                1 => new MemberBalanceInfoViewModel(userSession, customerInfo, creditInfo, balanceInfoModel, isAllDayBalanceInfo),
                _ => throw new NotImplementedException()
            };
        }

        private ActionResult ForbiddenResult()
            => Json(Result.CreateErrorResult("You are not authorized on this function.", (int)HttpStatusCode.Forbidden));
    }
}