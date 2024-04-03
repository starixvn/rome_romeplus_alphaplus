namespace AlphaPlus.Settings.Site.ViewLogs.Requests
{
    using System;
    using System.Collections.Generic;
    using AlphaPlus.Core.Helpers;
    using AlphaPlus.Core.Models;
    using AlphaPlus.Settings.ViewLogs.API.Helpers;
    using AlphaPlus.Settings.ViewLogs.API.Helpers.Impl;
    using Configurations;
    using Constants;
    using Fanex.Common;

    public class LogQuery
    {
        public const string CookieShowOldValue = "Alpha.ViewLogs.ShowOldValuesCookie";

        private readonly DateTime maxReportDate;

        private readonly DateTime minReportDate;

        private readonly DateTime limitedDateToShowInfo;

        private readonly Dictionary<string, bool> custStatuses;

        private readonly Dictionary<string, int> resourcesByBetTypeCacheRef;

        private readonly Dictionary<string, int> resourceByProductCacheRef;

        private readonly Dictionary<string, int> resourcesBySportTypeCacheRef;

        public LogQuery() : this(null, null, null)
        {
        }

#pragma warning disable S107 // Methods should not have too many parameters
        public LogQuery(
            IUserSession currentUserSession,
            IDateTime dateTimeWrap,
            bool? isShowOldValue,
            Dictionary<string, bool> custStatuses = null,
            Dictionary<string, int> resourcesByBetTypeCacheRef = null,
            bool? isShowMyanmarOdds = null,
            Dictionary<string, int> resourceByProductCacheRef = null,
            Dictionary<string, int> resourcesBySportTypeCacheRef = null)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            currentUserSession = currentUserSession ?? UserSession.Current;
            dateTimeWrap = dateTimeWrap ?? new DateTimeWrap();

            IsShowOldValue = isShowOldValue ?? GetShowOldValueFromCookie();
            this.custStatuses = custStatuses;
            this.resourcesByBetTypeCacheRef = resourcesByBetTypeCacheRef;
            this.resourceByProductCacheRef = resourceByProductCacheRef;
            this.resourcesBySportTypeCacheRef = resourcesBySportTypeCacheRef;

            // Limit time range for view log in 2 years (start from beginning of last year)
            maxReportDate = dateTimeWrap.Now;
            minReportDate = new DateTime(dateTimeWrap.Now.AddYears(-1).Year, 1, 1);

            limitedDateToShowInfo = currentUserSession.GetProperty<DateTime>(SessionKey.CustomerDateLimit);

            RoleId = currentUserSession.RoleId;
            ActorId = currentUserSession.CustId;
            // TODO IsShowMyanmarOdds = isShowMyanmarOdds ?? UserPermission.IsShowMyanmarOdds;
            HasViewLogPermission = currentUserSession.SubPageAccess != null && currentUserSession.SubPageAccess.ViewLog;
            Language = currentUserSession.Language;
            IsInternal = false;
            UplineId = currentUserSession.CustId;
            PreviousUsername = Username;
        }

        public string PreviousUsername { get; set; }

        public string Username { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public string ActionTypeId { get; set; }

        public int UplineId { get; set; }

        public byte RoleId { get; set; }

        public int ActorId { get; set; }

        public int SubAccId { get; set; }

        public bool HasViewLogPermission { get; set; }

        public bool IsInternal { get; set; }

        public bool IsShowOldValue { get; set; }

        public string Language { get; set; }

        public string PageIndex { get; set; }

        public string PageSize { get; set; }

        public Dictionary<string, bool> SportBetTypeVisibilities
        {
            get
            {
                return custStatuses ?? GetCustStatuses();
            }
        }

        public Dictionary<string, int> ResourcesByProductCacheRef
        {
            get
            {
                return resourceByProductCacheRef ?? AppSettings.StatusMappings;
            }
        }

        public Dictionary<string, int> ResourcesByBetTypeCacheRef
        {
            get
            {
                return resourcesByBetTypeCacheRef ?? AppSettings.ResourcesByBetTypeCacheRef;
            }
        }

        public Dictionary<string, int> ResourcesBySportTypeCacheRef
        {
            get
            {
                return resourcesBySportTypeCacheRef ?? AppSettings.ResourcesBySportTypeCacheRef;
            }
        }

        public DateTime MaxReportDate
        {
            get
            {
                return maxReportDate;
            }
        }

        public DateTime MinReportDate
        {
            get
            {
                return minReportDate;
            }
        }

        public bool IsShowMyanmarOdds { get; }

        public DateTime LimitedDateForShowInfo => limitedDateToShowInfo;

        private Dictionary<string, bool> GetCustStatuses()
        {
            var custStatusDict = new Dictionary<string, bool>
            {
                { "disablevirtualsports", UserPermission.IsEnableProduct(Settings.ViewLogs.API.Constants.ProductIdConstants.VirtualSports) },
                { "44", true /*TODO UserPermission.IsShowSportsbook2*/ }
            };

            foreach (var statusMapping in AppSettings.StatusMappings)
            {
                //// Key: Status Name, Value: Product Id
                custStatusDict.Add(statusMapping.Key, UserPermission.IsEnableProduct(statusMapping.Value));
            }

            return custStatusDict;
        }

        private bool GetShowOldValueFromCookie()
        {
            var cookie = AlphaContext.Current.Request.Cookies[CookieShowOldValue];

            bool showOldValue = cookie == null || Converter.ToInt(cookie) == 1;

            return showOldValue;
        }
    }
}