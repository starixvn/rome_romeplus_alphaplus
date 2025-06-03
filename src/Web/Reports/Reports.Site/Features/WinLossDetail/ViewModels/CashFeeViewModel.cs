using AlphaPlus.Core.Utils;
using AlphaPlus.Reports.Features.WinLossDetail.Models;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using AlphaPlus.Reports.Site.Features.WinLossDetail.Requests;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail.ViewModels;

public class CashFeeViewModel(CashFeeQuery cashFeeQuery, CashFeeModel cashFeeModel)
{
    public PageHeaderViewModel PageHeader { get; } = new PageHeaderViewModel(
        Title: "",
        IsFilterVisible: cashFeeQuery.IsFilterVisible);

    public DateRangePickerJQueryViewModel DateRangePicker { get; } = new DateRangePickerJQueryViewModel(
        cashFeeQuery.FromDate,
        cashFeeQuery.ToDate,
        cashFeeQuery.MinDate,
        SystemTime.Today.AddDays(1),
        cashFeeQuery.Language);

    //public IEnumerable<Reports.Features.Shared.Models.Breadcrumb> Breadcrumbs { get; } = breadcrumbs;

    public CashFeeModel CashFee { get; } = cashFeeModel;
}
