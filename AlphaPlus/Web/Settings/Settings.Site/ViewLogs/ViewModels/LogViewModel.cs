namespace AlphaPlus.Settings.Site.ViewLogs.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using AlphaPlus.Core.Models;
    using AlphaPlus.Settings.ViewLogs.API.Params;
    using Helpers;
    using Requests;

    public class LogViewModel
    {
        private readonly MicroSites microSites;
        private static readonly List<int> ActionIdByAdmin = new List<int> { 1221 };

        public LogViewModel(
            LogParam logParam,
            int totalRecords,
            MicroSites microSites = null)
        {
            this.microSites = microSites ?? new MicroSitesImpl();

            if (logParam == null)
            {
                return;
            }

            var dateInRange = logParam.FromDate < logParam.MinReportDate
                ? logParam.MinReportDate
                : logParam.FromDate;

            var fromDate = dateInRange.Day.ToString();

            dateInRange = logParam.ToDate < logParam.MaxReportDate
                ? logParam.ToDate
                : logParam.MaxReportDate;

            var toDate = dateInRange.Day.ToString();

            HasViewLogPermission = logParam.HasViewLogPermission;
            IsSubAccount = logParam.SubAccId > 0;
            RoleId = logParam.RoleId;

            ShowOldValue = logParam.IsShowOldValue;
            TotalRecords = totalRecords;
            PageSize = logParam.PageSize;
            PageIndex = logParam.PageIndex;

            FromDate = fromDate;
            ToDate = toDate;

            DayInMonth = logParam.MinReportDate < logParam.FromDate ?
                logParam.FromDate.ToShortDateString() :
                logParam.MinReportDate.ToShortDateString();

            MaxReportDate = logParam.MaxReportDate.ToShortDateString();
            MinReportDate = logParam.MinReportDate.ToShortDateString();

            Username = logParam.Username;

            ShowOldValuesCookieName = LogQuery.CookieShowOldValue;
        }

        public bool HasViewLogPermission { get; set; }

        public bool IsSubAccount { get; set; }

        public byte RoleId { get; set; }

        public string UserLevel
        {
            get
            {
                return Enumeration.FromValue<UserLevel>(RoleId).DisplayName.ToLower();
            }
        }

        public string ShowOldValuesCookieName { get; set; }

        public string UsernameAutoSuggestionPath
        {
            get
            {
                return microSites.OldMainPath("~/UserNameSuggestion");
            }
        }

        public string ViewIPInfoPath
        {
            get
            {
                return "main/IPInfo";
            }
        }

        public string ActionTypeId { get; set; }

        public bool HasLogs
        {
            get
            {
                return LogItems != null && LogItems.Any();
            }
        }

        public static bool ShowEditedBy(int actionId)
            => !ActionIdByAdmin.Contains(actionId);

        public bool ShowOldValue { get; set; }

        public int TotalRecords { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public List<LogItemViewModel> LogItems { get; set; }

        public string DayInMonth { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public string MinReportDate { get; set; }

        public string MaxReportDate { get; set; }

        public string Username { get; set; }

        public int FirstRowNumber => PageIndex * PageSize;
    }
}