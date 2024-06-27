using AlphaPlus.Settings.Site.Features.CustomerList.Requests;
using AlphaPlus.Shared.AppResources.Settings.CustomerList;
using AlphaPlus.Settings.Site.Features.CustomerList.Models;
using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.MemberInfo.Helpers;

namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels
{
    public class CustomerListViewModel
    {
        private readonly Dictionary<UserLevel, string> PageTitleDictionary = new()
        {
            { UserLevel.Super, CustomerListResources.MasterAgentList },
            { UserLevel.Master, CustomerListResources.AgentList },
            { UserLevel.Agent, CustomerListResources.MemberList }
        };

        public CustomerListViewModel(
            CustomerListRequest request,
            IEnumerable<CustInfo> breadcrumbLevel,
            bool isMobile,
            bool limitedDateToShowInfo,
            bool isEnabledAutoTransfer,
            CustomerListPagingViewModel customerListPagingViewModel = null)
        {
            UplineRoleId = request.ViewerLevelId;

            SearchAccountId = request.CustId;
            SearchAccountRoleId = request.CustLevelId;
            SearchAccountName = request.CustName;

            HideEditMultiple = request.ViewerLevel.IsSuperLevel() && request.CustLevel.IsMasterLevel() || !request.CanUpdateAccount;

            PageTitle = PageTitleDictionary[Enumeration.FromValue<UserLevel>(request.CustLevelId)];

            BreadcrumbLevel = breadcrumbLevel;
            BreadcrumbLevelJson = JsonHelper.ToJsonFunc(breadcrumbLevel);

            PageIndex = request.PageIndex;
            PageSize = request.PageSize;
            Status = request.Status;
            DoubleComm = request.DoubleComm;
            UnconfirmSetting = request.UnconfirmSetting;
            IsMobileView = isMobile;
            LimitedDateToShowInfo = limitedDateToShowInfo;
            IsShowMyanmarOdds = request.IsShowMyanmarOdds;
            CanViewReport = request.CanViewReports;
            CanUpdateAccount = request.CanUpdateAccount;

            DownlineLevel = request.CustLevel.GetDirectDownLineLevel();
            DownlineLevelName = DownlineLevel.DisplayName;
            Language = request.Language;

            SearchAccountLevel = request.CustLevel;
            CommissionColSpan = SearchAccountLevel.IsAgentLevel() ?
                      (IsShowMyanmarOdds ? 8 : 6) :
                     (IsShowMyanmarOdds ? 4 : 6);

            CustomerListPagingViewModel = customerListPagingViewModel;
            AllowAutoTransfer = request.AllowAutoTransfer;
            EnableAutoTransfer = isEnabledAutoTransfer;
            EnableAllDayBalanceTransfer = request.EnableAllDayBalanceTransfer.HasValue && request.EnableAllDayBalanceTransfer.Value;
        }

        public bool HideEditMultiple { get; }

        public byte UplineRoleId { get; }

        public int SearchAccountId { get; }

        public string SearchAccountName { get; }

        public byte SearchAccountRoleId { get; }

        public byte Status { get; }

        public byte DoubleComm { get; }

        public byte UnconfirmSetting { get; }

        public int PageSize { get; }

        public int PageIndex { get; }

        public string DownlineLevelName { get; }

        public UserLevel DownlineLevel { get; }

        public UserLevel SearchAccountLevel { get; }

        public string PageTitle { get; }

        public bool IsMobileView { get; }

        public bool CanViewReport { get; }

        public bool CanUpdateAccount { get; }

        public bool LimitedDateToShowInfo { get; }

        public bool IsShowMyanmarOdds { get; }

        public int CommissionColSpan { get; }

        public IEnumerable<CustInfo> BreadcrumbLevel { get; }

        public string BreadcrumbLevelJson { get; }

        public string Resources { get; }

        public string Language { get; }

        public int AllowAutoTransfer { get; }

        public bool EnableAutoTransfer { get; }

        public bool EnableAllDayBalanceTransfer { get; }

        public IEnumerable<CustomerListOption> StatusOptions
             =>
                [
                    new CustomerListOption(0, CustomerListResources.All),
                    new CustomerListOption(1, CustomerListResources.Open),
                    new CustomerListOption(2, CustomerListResources.Suspended),
                    new CustomerListOption(3, CustomerListResources.Closed),
                    new CustomerListOption(4, CustomerListResources.Disabled),
                    new CustomerListOption(5, CustomerListResources.Locked)
                ];

        public IEnumerable<CustomerListOption> DoubleCommOptions
             =>
                [
                    new CustomerListOption(2 , CustomerListResources.All),
                    new CustomerListOption(1 , CustomerListResources.Allowed),
                    new CustomerListOption(0 , CustomerListResources.Disallowed)
                ];

        public IEnumerable<CustomerListOption> AllowAutoTransferOptions
             =>
                [
                    new CustomerListOption(2 , CustomerListResources.All),
                    new CustomerListOption(1 , CustomerListResources.Enabled),
                    new CustomerListOption(0 , CustomerListResources.Disabled)
                ];

        public CustomerListPagingViewModel CustomerListPagingViewModel { get; }
    }

    public class CustomerListOption(int value, string text)
    {
        public int Value { get; } = value;

        public string Text { get; } = text;
    }
}