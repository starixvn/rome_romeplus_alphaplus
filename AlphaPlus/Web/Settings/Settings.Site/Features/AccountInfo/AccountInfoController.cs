using System.Net;
using AlphaPlus.Core;
using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.AccountInfo.Models;
using AlphaPlus.Settings.Site.Features.AccountInfo.Requests;
using AlphaPlus.Settings.Site.Features.AccountInfo.ViewModels;
using AlphaPlus.Utilities.IP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace AlphaPlus.Settings.Site.Features.AccountInfo
{
    public class AccountInfoController(IAccountInfoService accountInfoService, IPService ipService, IMemoryCache memoryCache) : BaseController
    {
        private const string AssetsVersionCacheKey = "AccountInfo:AssetsVersion";
        private readonly IAccountInfoService accountInfoService = accountInfoService;
        private readonly IPService ipService = ipService;
        private readonly IMemoryCache memoryCache = memoryCache;

        private string GetAssetsVersion()
        {
            if (!memoryCache.TryGetValue(AssetsVersionCacheKey, out string version))
            {
                version = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(4));
                memoryCache.Set(AssetsVersionCacheKey, version, cacheEntryOptions);
            }

            return version;
        }

        public ActionResult Index(IndexQuery message) => View(new IndexViewModel(message, GetAssetsVersion()));

        public async Task<ActionResult> GetAccount(GetAccountQuery message)
        {
            if (!message.IsValid())
            {
                return ForbiddenResult();
            }

            var customer = await accountInfoService.GetCustomerInfo(message.Username, message.ViewerId, message.ViewerLevelId);

            if (customer != null)
            {
                var balanceInfo = BuildBalanceInfoViewModel(message, customer, await accountInfoService.GetCreditInfo(customer.CustId),
                    await accountInfoService.GetBalanceInfo(customer.CustId, customer.RoleId, message.AGIP),
                    await accountInfoService.IsAllDayBalanceTransfer(message.ViewerId));

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

        public async Task<ActionResult> GivenCredit(GivenCreditQuery message)
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UpdateCustomerStatus([FromBody] CustomerNonProductStatusCommand command)
            => Json(await accountInfoService.UpdateCustomerStatus(command));

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