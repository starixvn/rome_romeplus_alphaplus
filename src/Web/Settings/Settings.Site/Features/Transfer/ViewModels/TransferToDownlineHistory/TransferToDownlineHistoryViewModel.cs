using AlphaPlus.Core.Models;
using AlphaPlus.Core.Utils;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Shared.AppResources.Settings.Transfer;
using AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferToDownlineHistory;
using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.Components;

namespace AlphaPlus.Settings.Site.Features.Transfer.ViewModels.TransferToDownlineHistory;

public class TransferToDownlineHistoryViewModel
{
    private const int reportMonth = -2;

    public TransferToDownlineHistoryViewModel(TransferToDownlineHistoryQuery message)
    {
        CustLevel = message.CustLevel;
        UserName = message.UserName;
        DateRangePickerViewModel = new DateRangePickerViewModel(
            SystemTime.Today.AddMonths(reportMonth),
            DateTime.MaxValue,
            message.FromDate,
            message.ToDate,
            message.UserSelectedDateRangeOption);
        LevelType = message.LevelType;
        ViewDataInstructionMessage = string.Format(Resources.CanViewReportDataFrom, DateRangePickerViewModel.MinDate.ToShortDateWithSlash());
        ShowLevelType = CustLevel.IsMasterOrSuperLevel();
    }

    public UserLevel CustLevel { get; }

    public string UserName { get; }

    public DateRangePickerViewModel DateRangePickerViewModel { get; }

    public byte LevelType { get; }

    public string ViewDataInstructionMessage { get; }

    public bool ShowLevelType { get; }
}
