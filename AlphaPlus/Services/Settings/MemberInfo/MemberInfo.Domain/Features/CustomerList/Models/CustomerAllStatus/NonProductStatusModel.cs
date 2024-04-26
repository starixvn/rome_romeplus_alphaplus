namespace AlphaPlus.Settings.MemberInfo.Domain.Features.CustomerList.Models
{
    public class NonProductStatusModel
    {
        private readonly byte TransferDailyOption = 7;

        public NonProductStatusModel()
        {
        }

        public NonProductStatusModel(
            bool hasDownline,
            bool disableAutoMPT,
            bool closed,
            bool isDirectClosed,
            bool uplineClosed,
            bool suspended,
            bool uplineSuspend,
            bool isDirectSuspend,
            string transferOption,
            bool doubleCommLosing,
            bool uplineDoubleCommLosing,
            bool disableOutright,
            bool uplineDisableOutright,
            bool isAllDayBalTransfer,
            byte locked,
            bool isAllowAutoTransfer)
        {
            HasDownline = hasDownline;
            DisableAutoMPT = disableAutoMPT;

            Closed = closed;
            IsDirectClosed = isDirectClosed;
            UplineClosed = uplineClosed;

            Suspended = suspended;
            IsDirectSuspend = isDirectSuspend;
            UplineSuspend = uplineSuspend;

            TransferOption = transferOption;

            DoubleCommLosing = doubleCommLosing;
            UplineDoubleCommLosing = uplineDoubleCommLosing;

            DisableOutright = disableOutright;
            UplineDisableOutright = uplineDisableOutright;
            IsAllDayBalTransfer = isAllDayBalTransfer;

            Locked = locked;
            IsAllowAutoTransfer = isAllowAutoTransfer;
        }

        public bool HasDownline { get; set; }

        public bool Closed { get; set; }

        public bool IsDirectClosed { get; set; }

        public bool UplineClosed { get; set; }

        public bool Suspended { get; set; }

        public bool IsDirectSuspend { get; set; }

        public bool UplineSuspend { get; set; }

        public string? TransferOption { get; set; }

        public bool TransferDaily =>
            !string.IsNullOrWhiteSpace(TransferOption) &&
            TransferOption[TransferDailyOption].Equals('0');

        public bool DoubleCommLosing { get; set; }

        public bool UplineDoubleCommLosing { get; set; }

        public bool DisableAutoMPT { get; set; }

        public bool DisableOutright { get; set; }

        public bool UplineDisableOutright { get; set; }

        public bool IsAllDayBalTransfer { get; set; }

        public byte Locked { get; set; }

        public bool IsProtectedMode { get; set; }

        public bool IsAllowAutoTransfer { get; set; }
        public string? ScheduleWeekDays { get; set; }
    }
}