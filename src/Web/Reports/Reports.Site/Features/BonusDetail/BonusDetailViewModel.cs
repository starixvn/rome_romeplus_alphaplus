using AlphaPlus.Reports.Features.BonusDetail;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using Microsoft.AspNetCore.Mvc.Rendering;
using AlphaPlus.Shared.AppResources.Report;
using AlphaPlus.Core.Models;
using AlphaPlus.Core.Helpers;

namespace AlphaPlus.Reports.Site.Features.BonusDetail;

public class BonusDetailViewModel(BonusDetailQuery message, PagedReportData<GroupBonus> reportData)
{
    public UserLevel ViewerLevel { get; } = message.ViewerLevel;

    public string Member { get; } = message.Member;

    public PageHeaderViewModel PageHeader { get; } = new PageHeaderViewModel(
        Title: Resources.BonusDetail,
        IsFilterVisible: message.IsFilterVisible,
        IconInfoUrl: MicroSites.FeatureUrl("~/staticmessage/guide?id=147430"));

    public DateRangePickerViewModel DateRangePicker { get; }
        = new DateRangePickerViewModel(
            message.MinDate,
            DateTime.MaxValue,
            message.FromDate,
            message.ToDate,
            message.UserSelectedDateRangeOption,
            isRenderSubmitButton: false);

    public PagedReportData<GroupBonus> ReportData { get; } = reportData;

    public PagerViewModel Pager { get; }
        = new PagerViewModel(reportData.PageIndex, reportData.PageSize, reportData.TotalRecords);

    public IEnumerable<SelectListItem> BonusStatus
        => [
            GetSelectListItem(Resources.All, "-1", !message.StatusId.HasValue),
            GetSelectListItem(Resources.Pending, BonusStatusIdConst.Pending.ToString(), message.StatusId == BonusStatusIdConst.Pending),
            GetSelectListItem(Resources.Cancelled, BonusStatusIdConst.Cancelled.ToString(), message.StatusId == BonusStatusIdConst.Cancelled),
            GetSelectListItem(Resources.Completed, BonusStatusIdConst.Completed.ToString(), message.StatusId == BonusStatusIdConst.Completed)
        ];

    public IEnumerable<SelectListItem> MemberFeedback
        => [
            GetSelectListItem(Resources.All, "-1", !message.IsComplained.HasValue),
            GetSelectListItem(Resources.NotReceived, true.ToString(), message.IsComplained == true),
            GetSelectListItem(Resources.Received, false.ToString(), message.IsComplained == false)
        ];

    private static SelectListItem GetSelectListItem(string text, string value, bool selected)
        => new SelectListItem(text, value, selected);

    public bool IsNegative(decimal? cellValue) 
    {
        if (!cellValue.HasValue) 
        {
            return false;
        }
        var rounded = Math.Round(cellValue.Value, 2, MidpointRounding.AwayFromZero);
        return rounded < 0;
    }
}