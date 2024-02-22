using AlphaPlus.Main.Site.AppResources;
using AlphaPlus.Main.Site.Helpers;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Main.Site.Features.Dashboard.Statistics
{
    public class StatisticsViewModel
    {
        public StatisticsViewModel(Statistics statistics, UserLevel userLevel, bool canViewReports)
        {
            Statistics = statistics;
            UserLevel = userLevel;
            CanViewReports = canViewReports;
            OutstandingLabel = GetOutStandingLabelByLevel(userLevel);
        }

        public Statistics Statistics { get; }

        public UserLevel UserLevel { get; }

        public bool CanViewReports { get; }

        public string OutstandingLabel { get; }

        public string MemberOutstandingReportPath
        {
            get
            {
                var reportSiteAppPath = UrlHelper.MapReportsPath("~/");

                return $@"{reportSiteAppPath}outstanding/{GetOutstandingDefaultReportLevel(UserLevel)}";
            }
        }

        public string SessionID => AlphaContext.Current.Session.Id;

        internal string GetOutstandingDefaultReportLevel(UserLevel userLevel)
        {
            if (userLevel.IsSuperLevel())
            {
                return "masternew";
            }

            if (userLevel.IsMasterLevel())
            {
                return "agentnew";
            }

            return "membernew";
        }

        private string GetOutStandingLabelByLevel(UserLevel userLevel)
        {
            if (userLevel.IsSuperLevel())
            {
                return Resources.superOutstanding;
            }

            if (userLevel.IsMasterLevel())
            {
                return Resources.masterOutstanding;
            }

            return Resources.agentOutstanding;
        }
    }
}
