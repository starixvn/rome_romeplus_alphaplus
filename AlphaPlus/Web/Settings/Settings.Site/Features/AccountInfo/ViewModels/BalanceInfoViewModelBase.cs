using System.Text.Json;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.AccountInfo.Models;
using AlphaPlus.Settings.Site.Features.AccountInfo.Requests;
using AlphaPlus.Shared.AppResources.Settings;

namespace AlphaPlus.Settings.Site.Features.AccountInfo.ViewModels
{
    public abstract class BalanceInfoViewModelBase : IBalanceInfoViewModel
    {
        private readonly bool isAllDayBalanceInfo;
        protected readonly CustomerInfoModel customerInfo;
        protected readonly CreditInfo creditInfo;
        protected readonly GetAccountQuery message;

        protected BalanceInfoViewModelBase(
            GetAccountQuery message,
            CustomerInfoModel customerInfo,
            CreditInfo creditInfo,
            bool isAllDayBalanceInfo)
        {
            this.message = message;
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

        internal virtual bool ShowTransferLink => !isAllDayBalanceInfo && message.AllowTransfer;

        public bool ShowOutstandingLink => message.CanViewReports && Outstanding > 0;

        public Link ReportLink
        {
            get
            {
                var labels = new Dictionary<UserLevel, string>
                {
                    {UserLevel.Master, Resources.MasterOutstanding },
                    {UserLevel.Agent, Resources.AgentOutstanding },
                    {UserLevel.Member, Resources.Outstanding },
                };

                var betListLink = MicroSites.TicketUrl($"~/RunningBetList/Index?CustID={customerInfo.CustId}&amp;CustName={customerInfo.UserName}&amp;GetAll=True");

                var outstandingLink = MicroSites.ReportsUrl($"~/outstanding/index?" +
                        $"CustId={customerInfo.CustId}&CustName={customerInfo.UserName}" +
                        $"&CustLevelId={customerInfo.RoleId}&BreadcrumbLevelJson={BreadcrumbLevelJson}");

                return new Link(labels[SearchAccountLevel], SearchAccountLevel.IsMemberLevel() ? betListLink : outstandingLink);
            }
        }


        public bool ShowTotalBalanceTransferLink => ShowTransferLink && message.AllowedTotalBalanceTransfer && !TotalBalance.Equals(0);

        // public string TotalBalanceTransferLink => BuildTransferLink(TotalBalance);

        public bool ShowYesterdayBalanceTransferLink => ShowTransferLink && !message.AllowedTotalBalanceTransfer && !YesterdayBalance.Equals(0);

        // public string YesterdayBalanceTransferLink => BuildTransferLink(YesterdayBalance);

        public bool CanUpdateAccount => message.CanUpdateAccount;

        protected static decimal RoundDownNumber(decimal value) => Math.Truncate(100 * value) / 100;

        protected static decimal RoundUpNumber(decimal value) => Math.Round(value, 2, MidpointRounding.AwayFromZero);

        private string BreadcrumbLevelJson
        {
            get
            {
                var custInfos = new List<BreadcrumbLevel>
                {
                    new(customerInfo.SuperId, customerInfo.SuperName, 4),
                    new(customerInfo.MasterId, customerInfo.MasterName, 3),
                    new(customerInfo.AgentId, customerInfo.AgentName, 2)
                };

                return JsonSerializer.Serialize(
                    custInfos.Where(custInfo =>
                    custInfo.CustLevelId > customerInfo.RoleId && custInfo.CustLevelId <= message.ViewerLevelId))
                    .Replace("\"", "'");
            }
        }

        protected UserLevel ActorLevel => message.ViewerLevel;

        protected UserLevel SearchAccountLevel => Enumeration.FromValue<UserLevel>(customerInfo.RoleId);
    }
}