namespace AlphaPlus.Settings.Site.Features.MyWatchlist.MyWatchlistAccounts.ViewModels
{
    using AlphaPlus.Settings.Site.Features.MyWatchlist.MyWatchlistAccounts.Requests;
    using Fanex.Common.Extensions;
    using System;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Models;
    using AlphaPlus.Core.Helpers;

    public class MyWatchlistAccountsViewModel
    {
        public MyWatchlistAccountsViewModel(
            MyWatchlistAccountsQuery request,
            GlobalThresholdModel globalThresholdModel,
            bool isMobile,
            bool canUpdateAccount,
            MyWatchlistAccountsPagingViewModel myWatchListAccountPagingViewModel = null)
        {
            SearchAccountId = request.CustId;
            IsMobile = isMobile;
            CanUpdateAccount = canUpdateAccount;
            GlobalThresholdModel = globalThresholdModel;
            MyWatchListAccountPagingViewModel = myWatchListAccountPagingViewModel;
            IsShowLastLoginColumn = request.IsShowLastLoginColumn;
            CustLevel = request.CustLevel.DisplayName;
            IsOpenFromProblemAccount = request.IsOpenFromProblemAccount;
            CustIds = request.CustIds;
        }

        public int SearchAccountId { get; }

        public GlobalThresholdModel GlobalThresholdModel { get; }

        public bool IsMobile { get; }

        public MyWatchlistAccountsPagingViewModel MyWatchListAccountPagingViewModel { get; }

        public bool IsShowLastLoginColumn { get; }

        public string CustLevel { get; }

        public bool CanUpdateAccount { get; }
        public bool IsOpenFromProblemAccount { get; }
        public string CustIds { get; }

        public DateTime MinDate => DateTime.Today.AddMonths(-3).FirstDayOfMonth();

        public string UserGuideLink => AlphaPlus.Core.Configurations.AppSettings.IsSiteLabel789y ?
            MicroSites.MessageUrl("~/StaticMessage/Guide?id=179067") :
            MicroSites.HowToUseUrl("~/Index.aspx?linktomessagesite=179067");
    }
}