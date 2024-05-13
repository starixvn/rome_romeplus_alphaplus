using System.Net;
using AlphaPlus.Core;
using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.AccountInfo.Models;
using AlphaPlus.Settings.Site.Features.AccountInfo.Requests;
using AlphaPlus.Settings.Site.Features.AccountInfo.ViewModels;
using AlphaPlus.Utilities.IP;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Settings.Site.Features.AccountInfo
{
    public class AccountInfoController(IAccountInfoService accountInfoService, IPService ipService) : BaseController
    {
        private readonly IAccountInfoService accountInfoService = accountInfoService;
        private readonly IPService ipService = ipService;

        public ActionResult Index(IndexQuery message) => View(new IndexViewModel(message));

        public async Task<ActionResult> GetAccount(GetAccountQuery message)
        {
            if (!message.IsValid())
            {
                return ForbiddenResult();
            }

            var customer = await accountInfoService.GetCustomerInfo(message.Username, message.ActorId, message.ActorRoleId);

            if (customer != null)
            {
                var balanceInfo = BuildBalanceInfoViewModel(message, customer, await accountInfoService.GetCreditInfo(customer.CustId),
                    await accountInfoService.GetBalanceInfo(customer.CustId, customer.RoleId, message.AGIP),
                    await accountInfoService.IsAllDayBalanceTransfer(message.ActorId));

                var formatedIP = ipService.FormatIP(customer.LastLoginIP, message.IsAdminCanViewVNIP);

                customer.LastLoginIP = formatedIP.IP;
                customer.AllowOpenInfoPopup = formatedIP.AllowOpenInfoPopup;

                return Json(new CustomerInfoViewModel(
                    message,
                    customer,
                    balanceInfo,
                    await accountInfoService.CheckFirstLoginByQRCode(customer.CustId)));
            }

            return Json(null);
        }

        public async Task<ActionResult> GetDownlineList(DownlineListQuery message)
        {
            if (!message.IsValid())
            {
                return ForbiddenResult();
            }

            var downlineList = await accountInfoService.GetDownlineList(
                message.CustId,
                message.PageIndex,
                message.RoleId,
                message.StatusId,
                message.TotalPages);

            var totalDownlinesSummarys = message.TotalPages.Equals(0)
                ? await accountInfoService.GetTotalDownlinesSummary(message.CustId, message.RoleId)
                : [];

            return Json(new DownlineListViewModel(downlineList, totalDownlinesSummarys));
        }

        public async Task<ActionResult> GetLastTxn(GetLastTxnQuery message)
        {
            var lastTxn = await accountInfoService.GetLastTxn(message.Username, message.UplineId, message.IsInternal);

            return Json(DateTime.MinValue == lastTxn ? string.Empty : lastTxn.ToString());
        }

        public async Task<ActionResult> GetOnlineList(OnlineListQuery message)
        => Json(await accountInfoService.GetOnlineList(message.SearchAccountId, message.IsAdminCanViewVNIP));

        private static IBalanceInfoViewModel BuildBalanceInfoViewModel(
            GetAccountQuery message,
            CustomerInfoModel customerInfo,
            CreditInfo creditInfo,
            IBalanceInfoModel balanceInfoModel,
            bool isAllDayBalanceInfo)
        {
            return customerInfo.RoleId switch
            {
                3 => new MasterBalanceInfoViewModel(message, customerInfo, creditInfo, balanceInfoModel, isAllDayBalanceInfo),
                2 => new AgentBalanceInfoViewModel(message, customerInfo, creditInfo, balanceInfoModel, isAllDayBalanceInfo),
                1 => new MemberBalanceInfoViewModel(message, customerInfo, creditInfo, balanceInfoModel, isAllDayBalanceInfo),
                _ => throw new NotImplementedException()
            };
        }

        private JsonResult ForbiddenResult()
            => Json(Result.CreateErrorResult("You are not authorized on this function.", (int)HttpStatusCode.Forbidden));
    }
}