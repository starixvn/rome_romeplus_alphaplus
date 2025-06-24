namespace AlphaPlus.Settings.Site.Features.CustomerList
{
    public static class DataKey
    {
        public static class SearchAccount
        {
            public const string IsSuspended = "SearchAccount.IsSuspended";
            public const string IsDirectSuspend = "SearchAccount.Upline.IsDirectSuspend";
            public const string IsAllowedMasterAutoPT = "SearchAccount.IsAllowedMasterAutoPT";
            public const string IsAllowedOutright = "SearchAccount.IsAllowedOutright";
            public const string IsAllowedDoubleComm = "SearchAccount.IsAllowedDoubleComm";
            public const string IsClosed = "SearchAccount.IsClosed";
            public const string IsDirectClosed = "SearchAccount.Upline.IsDirectClosed";
            public const string IsLockedAccess = "SearchAccount.Locked";
            public const string IsOfferProtectedMode = "SearchAccount.IsOfferProtectedMode";
            public const string IsProtectedMode = "SearchAccount.IsProtectedMode";
            public const string IsEnableSecurityCode = "SearchAccount.IsEnableSecurityCode";
            public const string IsAllDayBalTransfer = "SearchAccount.IsAllDayBalTransfer";
            public const string AllowedAllDayBalTransfer = "SearchAccount.AllowedAllDayBalTransfer";
            public const string IsAllowAutoTransfer = "SearchAccount.IsAllowAutoTransfer";
            public const string ScheduleWeekDays = "SearchAccount.ScheduleWeekDays";
            public const string IsCashMember = "SearchAccount.IsCashMember";

            public static class Upline
            {
#pragma warning disable RECS0146 // Member hides static member from outer class
#pragma warning disable S3218 // Inner class members should not shadow outer class "static" or type members
                public const string IsSuspended = "SearchAccount.Upline.IsSuspended";
                public const string IsAllowedOutright = "SearchAccount.Upline.IsAllowedOutright";
                public const string IsClosed = "SearchAccount.Upline.IsClosed";
                public const string IsAllowedDoubleComm = "SearchAccount.Upline.IsAllowedDoubleComm";
#pragma warning disable RECS0146 // Member hides static member from outer class
#pragma warning restore S3218 // Inner class members should not shadow outer class "static" or type members
            }
        }

        public static class Actor
        {
            public const string HasUpdateAccountPermission = "Actor.HasUpdateAccountPermission";
            public const string CanEditDailyTransferOption = "Actor.CanEditDailyTransferOption";
        }

        public static class StatusMapping
        {
            public const string DisableAutoMasterPT = "DisableAutoMPT";
            public const string Closed = "Closed";
            public const string Unlock = "Unlock";
            public const string Protected = "IsProtectionMode";
            public const string DoubleComm = "DoubleCommLosing";
            public const string DisableOutright = "DisableOutright";
            public const string Suspended = "Suspended";
            public const string IsAllDayBalTransfer = "IsAllDayBalTransfer";
            public const string AutoTransfer = "AutoTransfer";
        }

        public static class Method
        {
            public const string GET = "GET";
            public const string POST = "POST";
        }

        public static class LockedAccess
        {
            public const byte None = 0;
            public const byte FalseOTPSC = 1;
            public const byte FalsePassword = 2;
        }

        public static class Theme
        {
            public const string IsDarkMode = "Theme.IsDarkMode";
        }
    }
}