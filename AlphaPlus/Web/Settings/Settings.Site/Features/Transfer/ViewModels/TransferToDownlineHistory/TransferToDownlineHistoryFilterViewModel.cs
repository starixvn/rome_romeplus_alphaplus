using AlphaPlus.Shared.AppResources.Settings.Transfer;
using AlphaPlus.Settings.Transfer.API.Extensions;
using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.Components;

namespace AlphaPlus.Settings.Site.Features.Transfer.ViewModels.TransferToDownlineHistory
{
    public class TransferToDownlineHistoryFilterViewModel
    {
        public TransferToDownlineHistoryFilterViewModel(
            string userName,
            DateRangePickerViewModel dateRangePickerViewModel,
            DateTime minDate)
        {
            UserName = userName;
            DateRangePickerViewModel = dateRangePickerViewModel;
            ViewDataInstructionMessage = string.Format(
                Resources.CanViewReportDataFrom,
                minDate.ToShortDateWithSlash());
        }

        public string UserName { get; }

        public DateRangePickerViewModel DateRangePickerViewModel { get; }

        public string ViewDataInstructionMessage { get; }
    }
}