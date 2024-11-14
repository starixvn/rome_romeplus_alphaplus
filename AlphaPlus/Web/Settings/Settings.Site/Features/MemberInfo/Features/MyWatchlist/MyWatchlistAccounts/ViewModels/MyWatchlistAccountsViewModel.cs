using AlphaPlus.Settings.MemberInfo.Domain.Constants;
using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Models;
using AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts.Requests;
using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.Components;
using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts.ViewModels
{
    using AlphaPlus.Core.Configurations;
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
            DateRangePicker = new DateRangePickerJQueryViewModel(
                Resources.AddedDate,
                request.Language,
                request.FromDate,
                request.ToDate);
        }

        public int SearchAccountId { get; }

        public GlobalThresholdModel GlobalThresholdModel { get; }

        public bool IsMobile { get; }

        public MyWatchlistAccountsPagingViewModel MyWatchListAccountPagingViewModel { get; }

        public bool IsShowLastLoginColumn { get; }

        public string CustLevel { get; }

        public bool CanUpdateAccount { get; }

        public DateRangePickerJQueryViewModel DateRangePicker { get; }

        public IEnumerable<SelectListItem> Status
            => ReachedThresholdStatusConstants.Items.Select(s => new SelectListItem
            {
                Text = Resources.ResourceManager.GetString(s.Value),
                Value = s.Key.ToString(),
                Selected = s.Key == ReachedThresholdStatusConstants.All
            });

        public IEnumerable<SelectListItem> ProblemAccounts
        {
            get
            {
                yield return new SelectListItem { Text = Resources.All, Value = "2", Selected = true };
                yield return new SelectListItem { Text = Resources.Yes, Value = "1" };
                yield return new SelectListItem { Text = Resources.No2, Value = "0" };
            }
        }

        public string UserGuideLink
        {
            get
            {
                if (AppSettings.SupportedNewHandbook)
                {
                    return MicroSites.FeatureUrl($"~/HandbookDefault/GetBySlug?slug=my-watchlist");
                }

                return AppSettings.IsSiteLabel789y ?
                    MicroSites.FeatureUrl("~/StaticMessage/Guide?id=179067") :
                    MicroSites.FeatureUrl("~/Index.aspx?linktomessagesite=179067");
            }
        }
    }
}