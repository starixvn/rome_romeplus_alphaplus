using AlphaPlus.Core.Utils;
using AlphaPlus.Reports.Features.CashBillingService.Models;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.WinLoss;

public class CashFeeViewModel(
CashFeeGetDataQuery message,
CashFeeModel cashFeeModel,
IEnumerable<Reports.Features.Shared.Models.Breadcrumb> breadcrumbs)
{
    public PageHeaderViewModel PageHeader { get; } = new PageHeaderViewModel(
        Title: GetCashFeeTitle(message),
        IsFilterVisible: message.IsFilterVisible);

    public DateRangePickerJQueryViewModel DateRangePicker { get; } = new DateRangePickerJQueryViewModel(
        message.FromDate,
        message.ToDate,
        message.MinDate,
        SystemTime.Today.AddDays(1),
        message.Language);

    public IEnumerable<Reports.Features.Shared.Models.Breadcrumb> Breadcrumbs { get; } = breadcrumbs;

    public CashFeeModel CashFee { get; } = cashFeeModel;

    public int CustId { get; } = message.CustId;

    public string CustName { get; } = message.CustName;

    public byte CustLevelId { get; } = message.CustLevelId;

    public byte LevelType { get; } = message.LevelType;

    public string Username { get; } = message.Username;

    public string UserSelectedProductIds { get; } = message.UserSelectedProductIds;

    public string UserSelectedResourceIds { get; } = message.UserSelectedResourceIds;

    private static string GetCashFeeTitle(CashFeeGetDataQuery message)
    {
        return $"{message.CustName} - {Resources.CashServiceBilling.ToUpper()}";
    }
}