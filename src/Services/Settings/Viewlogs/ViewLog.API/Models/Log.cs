namespace AlphaPlus.Settings.ViewLogs.API.Models
{
    public class Log
    {
        public string Browser { get; set; }

        public string Domain { get; set; }

        public string Country { get; set; }

        public int ActionId { get; set; }

        public string ActionName { get; set; }

        public int ActionTypeId { get; set; }

        public int CustId { get; set; }

        public int RoleId { get; set; }

        public string CustName { get; set; }

        public bool IsInternal { get; set; }

        public DateTime LogDate { get; set; }

        public long LogId { get; set; }

        public string StoreProcedureName { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }

        public string TrackingValue { get; set; }

        public string TrackingValueOld { get; set; }

        public string IP { get; set; }

        public string Details { get; set; }

        public bool IsHideNickname { get; set; }
    }
}
