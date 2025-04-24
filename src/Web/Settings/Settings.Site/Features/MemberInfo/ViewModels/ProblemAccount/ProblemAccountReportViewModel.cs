using AlphaPlus.Settings.Site.Features.MemberInfo.Requests.ProblemAccount;
using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.Components;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
using AlphaPlus.Settings.MemberInfo.Domain.Constants;
using AlphaPlus.Settings.MemberInfo.Domain.Models.ProblemAccount;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.ProblemAccount
{
    public class ProblemAccountReportViewModel
    {
        public ProblemAccountReportViewModel(
            IndexRequest query,
            IEnumerable<ProblemAccountModel> models,
            bool isMobile)
        {
            DateRangePickerViewModel = new DateRangePickerViewModel(new DateTime(2020, 1, 1), DateTime.Now, query.FromDate, query.ToDate);
            var status = ReviewStatusConstant.Items
                .Select(s => new SelectListItem
                {
                    Text = Resources.ResourceManager.GetString(s.Value),
                    Value = s.Key.ToString(),
                    Selected = s.Key == query.UserSelectedReviewedStatusId
                });
            ReviewedStatusSelectionViewModel = new ReviewedStatusSelectionViewModel(status, query.UserSelectedReviewedStatusId);
            IsMobileView = isMobile;
            Models = models;
            Query = query;
        }

        public ReviewedStatusSelectionViewModel ReviewedStatusSelectionViewModel { get; }

        public DateRangePickerViewModel DateRangePickerViewModel { get; }

        public IEnumerable<ProblemAccountModel> Models { get; set; }

        public IndexRequest Query { get; }

        public const int TotalColumnMaster = 10;

        public bool IsMobileView { get; }

        public bool CanUpdateAccount => Query.CanUpdateAccount;

        public int TotalColumnAgent => Query.CustLevel.IsMasterLevel() ? 10 : 9;

        public int TotalColumnMember => Query.CustLevel.IsAgentLevel() && Query.CanUpdateAccount ? 10 : 8;

        public static string UsernameAutoSuggestionPath
            => MicroSites.SettingUrl("~/_GlobalResources/Handlers/QueryUserName.ashx");

        public bool IsShowUplineUsername(byte searchRoleId)
            => Query.CustLevel.GetDirectDownLineLevel().Value != searchRoleId;
    }
}
