using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.Accounts.Models;

namespace AlphaPlus.Settings.Site.Features.Accounts.ViewModels
{
    public abstract class BalanceInfoViewModelBase : IBalanceInfoViewModel
    {
        private readonly bool isAllDayBalanceInfo;
        protected readonly CustomerInfoModel customerInfo;
        protected readonly CreditInfo creditInfo;
        protected readonly IUserSession userSession;

        protected BalanceInfoViewModelBase(
            IUserSession userSession,
            CustomerInfoModel customerInfo,
            CreditInfo creditInfo,
            bool isAllDayBalanceInfo)
        {
            this.userSession = userSession;
            this.customerInfo = customerInfo;
            this.creditInfo = creditInfo;
            this.isAllDayBalanceInfo = isAllDayBalanceInfo;
        }

        public abstract decimal TotalBalance { get; }

        public abstract decimal YesterdayBalance { get; }

        public abstract decimal Outstanding { get; }

        public abstract decimal GivenCredit { get; }

        public abstract decimal GivenCreditMin { get; }

        public abstract decimal GivenCreditMax { get; }

        public abstract decimal PendingTransfer3rd { get; }

        internal virtual bool ShowTransferLink => !isAllDayBalanceInfo && userSession.SubPageAccess.Transfer;

        public bool ShowOutstandingLink => userSession.SubPageAccess.ViewReports && Outstanding > 0;


        public bool ShowTotalBalanceTransferLink => ShowTransferLink && userSession.IsAllowedTotalBalanceTransfer && !TotalBalance.Equals(0);

        // public string TotalBalanceTransferLink => BuildTransferLink(TotalBalance);

        public bool ShowYesterdayBalanceTransferLink => ShowTransferLink && !userSession.IsAllowedTotalBalanceTransfer && !YesterdayBalance.Equals(0);

        // public string YesterdayBalanceTransferLink => BuildTransferLink(YesterdayBalance);

        public bool CanUpdateAccount => !userSession.IsSubAcc || (userSession.IsSubAcc && userSession.SubPageAccess.UpdateAccount);

        protected static decimal RoundDownNumber(decimal value) => Math.Truncate(100 * value) / 100;

        protected static decimal RoundUpNumber(decimal value) => Math.Round(value, 2, MidpointRounding.AwayFromZero);

        // private string BreadcrumbLevelJson
        // {
        //     get
        //     {
        //         var custInfos = new List<BreadcrumbLevel>
        //         {
        //             new BreadcrumbLevel(customerInfo.SuperId, customerInfo.SuperName, 4),
        //             new BreadcrumbLevel(customerInfo.MasterId, customerInfo.MasterName, 3),
        //             new BreadcrumbLevel(customerInfo.AgentId, customerInfo.AgentName, 2)
        //         };

        //         return JsonConvert.SerializeObject(
        //             custInfos.Where(custInfo =>
        //             custInfo.CustLevelId > customerInfo.RoleId && custInfo.CustLevelId <= message.ActorRoleId))
        //             .Replace("\"", "'");
        //     }
        // }

        protected UserLevel ActorLevel => Enumeration.FromValue<UserLevel>(userSession.RoleId);

        protected UserLevel SearchAccountLevel => Enumeration.FromValue<UserLevel>(customerInfo.RoleId);
    }
}