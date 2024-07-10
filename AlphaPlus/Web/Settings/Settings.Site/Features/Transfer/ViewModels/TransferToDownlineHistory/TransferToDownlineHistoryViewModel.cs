namespace AlphaPlus.Settings.Site.Features.Transfer.ViewModels.TransferToDownlineHistory
{
    using AlphaPlus.Shared.AppResources.Settings.Transfer;
    using AlphaPlus.Settings.Transfer.API.Params.TransferToDownlineHistory;
    using AlphaPlus.Settings.Transfer.API.Utils;
    using Requests.TransferToDownlineHistory;
    using System;
    using ViewModels.Components;
    using AlphaPlus.Core.Models;

    public class TransferToDownlineHistoryViewModel
    {
        private const int reportMonth = -2;
        private readonly TransferToDownlineHistoryQuery message;
        private readonly TransferToDownlineHistoryParam param;

        public TransferToDownlineHistoryViewModel(
            TransferToDownlineHistoryQuery message,
            TransferToDownlineHistoryParam param)
        {
            this.message = message;
            this.param = param;
            CustLevel = message.CustLevel;
        }

        public UserLevel CustLevel { get; }

        public string UserName => message.UserName;

        public string DirectDownlineText
            => Resources.ResourceManager.GetString(CustLevel.GetDirectDownLineLevel().DisplayName);

        public static DateTime MinDate => SystemTime.Today.AddMonths(reportMonth);

        public static DateTime MaxDate => DateTime.MaxValue;

        public DateTime FromDate => param.FromDate;

        public DateTime ToDate => param.ToDate;

        public int UserSelectedDateRangeOption => message.UserSelectedDateRangeOption;

        public TransferToDownlineHistoryFilterViewModel FilterViewModel => new TransferToDownlineHistoryFilterViewModel(
            UserName,
            new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption),
            MinDate);
    }
}