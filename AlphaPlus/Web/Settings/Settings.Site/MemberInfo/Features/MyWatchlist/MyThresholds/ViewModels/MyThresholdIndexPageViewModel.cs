namespace AlphaPlus.Settings.Site.Features.MyWatchlist.MyThresholds.ViewModels
{
    using System.Collections.Generic;
    using AlphaPlus.Settings.Site.Features.MyWatchlist.MyThresholds.Requests;
    using AlphaPlus.Settings.Site.ViewModels.Components;
    using AlphaPlus.Core.Helpers;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.ReachedThreshold.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Configurations;

    public class MyThresholdIndexPageViewModel
    {
        public MyThresholdIndexPageViewModel(
            MyThresholdIndexPageQuery message,
            bool isMobile,
            bool canUpdateAccount,
            GlobalThresholdModel myThreshold,
            IEnumerable<ReachHistoryModel> myThresholdHistory)
        {
            CustLevel = message.CustLevel;
            DateRangePickerViewModel = new DateRangePickerViewModel(
                message.MinDate,
                message.CurrentDate,
                message.FromDate,
                message.ToDate);
            IsMobile = isMobile;
            CanUpdateAccount = canUpdateAccount;
            MyThreshold = myThreshold;
            MyThresholdHistory = myThresholdHistory;
        }

        public UserLevel CustLevel { get; }

        public DateRangePickerViewModel DateRangePickerViewModel { get; }

        public bool IsMobile { get; }

        public bool CanUpdateAccount { get; }

        public GlobalThresholdModel MyThreshold { get; }

        public IEnumerable<ReachHistoryModel> MyThresholdHistory { get; }

        public string UserGuideLink => AppSettings.IsMBCSite
            ? MicroSites.FeatureUrl("~/Index.aspx?linktomessagesite=179067")
            : MicroSites.FeatureUrl("~/StaticMessage/Guide?id=179067");
    }
}
