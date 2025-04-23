namespace AlphaPlus.Settings.Site.Features.ViewLogs.ViewModels
{
    public class LogItemViewModel
    {
        public int ActionId { get; set; }

        public string ActionName { get; set; }

        public int ActionTypeId { get; set; }

        public int CustId { get; set; }

        public string CustName { get; set; }

        public byte IsInternal { get; set; }

        public string LogDate { get; set; }

        public long LogId { get; set; }

        public string SPName { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }

        public string TrackingValue { get; set; }

        public string TrackingValueOld { get; set; }

        public string Details { get; set; }

        public string IP { get; set; }
    }
}