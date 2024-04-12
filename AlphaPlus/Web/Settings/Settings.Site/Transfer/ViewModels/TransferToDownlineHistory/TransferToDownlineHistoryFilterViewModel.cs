namespace AlphaPlus.Settings.Site.Transfer.ViewModels.TransferToDownlineHistory
{
    using AlphaPlus.Settings.Transfer.API.AppResources;
    using AlphaPlus.Settings.Transfer.API.Extensions;
    using System;
    using ViewModels.Components;

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