using System.Text.Json;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.Models;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.ViewModels;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.Transfer.Requests;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;
using AlphaPlus.Shared.AppResources.Settings;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.Balance
{
    public abstract class BalanceInfoViewModelBase : IBalanceInfoViewModel
    {
        private readonly bool isAllDayBalanceInfo;
        protected readonly CustomerInfoModel account;
        protected readonly CreditInfo creditInfo;
        protected readonly CustomerInfoQuery message;

        protected BalanceInfoViewModelBase(
            CustomerInfoQuery message,
            CustomerInfoModel account,
            CreditInfo creditInfo,
            bool isAllDayBalanceInfo)
        {
            this.message = message;
            this.account = account;
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

                var betListLink = MicroSites.TicketUrl($"~/RunningBetList/Index?CustID={account.CustId}&CustName={account.UserName}&GetAll=True");

                var outstandingLink = MicroSites.ReportsUrl($"~/outstanding/index?" +
                        $"CustId={account.CustId}&CustName={account.UserName}" +
                        $"&CustLevelId={account.RoleId}&BreadcrumbLevelJson={BreadcrumbLevelJson}");

                return new Link(labels[SearchAccountLevel], SearchAccountLevel.IsMemberLevel() ? betListLink : outstandingLink);
            }
        }


        public bool ShowTotalBalanceTransferLink => ShowTransferLink && message.AllowedTotalBalanceTransfer && !TotalBalance.Equals(0);

        public string TotalBalanceTransferLink => BuildTransferLink();

        public bool ShowYesterdayBalanceTransferLink => ShowTransferLink && !message.AllowedTotalBalanceTransfer && !YesterdayBalance.Equals(0);

        public string YesterdayBalanceTransferLink => BuildTransferLink();

        public bool CanUpdateAccount => message.CanUpdateAccount;

        protected static decimal RoundDownNumber(decimal value) => Math.Truncate(100 * value) / 100;

        protected static decimal RoundUpNumber(decimal value) => Math.Round(value, 2, MidpointRounding.AwayFromZero);

        private string BreadcrumbLevelJson
        {
            get
            {
                var custInfos = new List<BreadcrumbLevel>
                {
                    new(account.SuperId, account.SuperName, 4),
                    new(account.MasterId, account.MasterName, 3),
                    new(account.AgentId, account.AgentName, 2)
                };

                return JsonSerializer.Serialize(
                    custInfos.Where(custInfo =>
                    custInfo.CustLevelId > account.RoleId && custInfo.CustLevelId <= message.ViewerLevelId))
                    .Replace("\"", "'");
            }
        }

        protected UserLevel ActorLevel => message.ViewerLevel;

        protected UserLevel SearchAccountLevel => Enumeration.FromValue<UserLevel>(account.RoleId);

        internal string BuildTransferLink()
            => MicroSites.SettingUrl(
                string.Format("~/TransferYesterdayBalance/{0}?custId={1}&custName={2}",
                    SearchAccountLevel.IsMemberLevel() ? "Member" : "Single",
                    account.CustId,
                    account.UserName));
    }
}