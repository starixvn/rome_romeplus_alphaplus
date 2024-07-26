namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.DormantAccountList.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using AlphaPlus.Settings.Site.Features.MemberInfo.Features.DormantAccountList.Requests;
    using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
    using AlphaPlus.Settings.MemberInfo.Domain.Constants;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.DormantAccountList.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using AlphaPlus.Core.Models;

    public class DormantAccountViewModel
    {
        public DormantAccountViewModel(
            DormantAccountReport report,
            DormantAccountQuery message,
            bool isShowDateTimeColumn)
        {
            Report = report;
            Message = message;
            IsShowDateTimeColumn = isShowDateTimeColumn;
        }

        public DormantAccountReport Report { get; }

        public DormantAccountQuery Message { get; }

        public bool IsShowDateTimeColumn { get; }

        public IEnumerable<SelectListItem> Levels
        {
            get
            {
                yield return BuildSelectListItem(Resources.All, 0, 0 == Message.LevelId);

                var currentLevelId = Message.ViewerLevelId;
                while (currentLevelId > 1)
                {
                    currentLevelId--;
                    yield return BuildSelectListItem(
                        Resources.ResourceManager.GetString(Enumeration.FromValue<UserLevel>(currentLevelId).DisplayName),
                        currentLevelId,
                        currentLevelId == Message.LevelId);
                }
            }
        }

        public IEnumerable<SelectListItem> Status
        {
            get
            {
                yield return BuildSelectListItem(Resources.All, StatusIdentity.All, StatusIdentity.All == Message.StatusId);
                yield return BuildSelectListItem(Resources.Closed, StatusIdentity.Closed, StatusIdentity.Closed == Message.StatusId);
                yield return BuildSelectListItem(Resources.Disabled, StatusIdentity.Disabled, StatusIdentity.Disabled == Message.StatusId);
                yield return BuildSelectListItem(Resources.Deleted, StatusIdentity.Deleted, StatusIdentity.Deleted == Message.StatusId);
            }
        }

        private static SelectListItem BuildSelectListItem(string text, byte value, bool selected)
            => new SelectListItem { Text = text, Value = value.ToString(), Selected = selected };

        public string GetArchiveProcessViewLang()
            => new[] { "th-TH", "vi-VN", "zh-CN", "zh-TW" }.FirstOrDefault(l => l.Equals(Message.Language)) ?? "en-US";
    }
}