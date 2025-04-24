using Microsoft.AspNetCore.Mvc.Rendering;
using AlphaPlus.Shared.AppResources.Settings.Transfer;
using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.Components;
using AlphaPlus.Core.Models;
using AlphaPlus.Core.Utils;

namespace AlphaPlus.Settings.Site.Features.Transfer.ViewModels.AutoTransfer;

public class HistoryViewModel : AutoTransferViewModel
{
    private const int reportMonth = -3;

    public HistoryViewModel(IUserSession userSession) : base(userSession)
    {
        MinDate = SystemTime.Yesterday.AddMonths(reportMonth);
        MaxDate = SystemTime.Yesterday;
        FromDate = SystemTime.Yesterday;
        ToDate = SystemTime.Yesterday;

        DateRangePickerViewModel = new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, 1);
        DateRangePickerViewModel.RangeOptions = DateRangePickerViewModel.RangeOptions.Where(x => x.Value != "0");
        Columns = GetColumns();
    }

    public DateTime FromDate { get; }

    public DateTime ToDate { get; }

    public static DateTime MinDate { get; private set; }

    public static DateTime MaxDate { get; private set; }

    public DateRangePickerViewModel DateRangePickerViewModel { get; }

    private IEnumerable<SelectListItem> GetColumns()
    {
        if (ShowLevelType)
        {
            yield return new SelectListItem { Text = Resources.Level, Value = "8" };
        }

        yield return new SelectListItem { Text = Resources.First_LastName, Value = "3" };
        yield return new SelectListItem { Text = Resources.TransferAmount, Value = "4" };
        yield return new SelectListItem { Text = Resources.Status, Value = "5" };
        yield return new SelectListItem { Text = Resources.Reason, Value = "6" };
        yield return new SelectListItem { Text = Resources.CompletedTime, Value = "7" };
    }
}