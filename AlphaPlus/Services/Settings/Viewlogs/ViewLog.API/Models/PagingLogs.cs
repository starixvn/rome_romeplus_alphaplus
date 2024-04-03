namespace AlphaPlus.Settings.ViewLogs.API.Models
{
    using System.Collections.Generic;
    using System.Linq;

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
