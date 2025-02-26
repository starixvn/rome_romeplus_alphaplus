namespace AlphaPlus.Settings.ViewLogs.API.Models
{
    public class PagingLogs<LogType> where LogType : Log
    {
        public PagingLogs()
        {
            Logs = Enumerable.Empty<LogType>();
        }

        public IEnumerable<LogType> Logs { get; set; }

        public int TotalRecords { get; set; }
    }
}
