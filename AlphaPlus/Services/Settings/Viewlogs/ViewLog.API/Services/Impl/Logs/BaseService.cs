namespace AlphaPlus.Settings.ViewLogs.API.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Constants;
    using Criteria;
    using DataAccesses;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models;
    using Params;
    using AlphaPlus.Core.Models;

    public abstract class BaseService
    {
        private const string LogActionCacheKey = "AlphaPlus.Settings.ViewLogs.API.Setting.LogAction";
        private const string LogActionTypesCacheKey = "AlphaPlus.Settings.ViewLogs.API.Setting.LogActionTypes";
        private const int OneHour = 1;

        private readonly IDynamicRepository dynamicRepository;
        private readonly CustomerService customerService;
        private readonly IUserSession session;
        private readonly ICacheService cacheService;

        protected BaseService(IDynamicRepository dynamicRepository, CustomerService customerService, LogDataAccess logDataAccess, IUserSession session, ICacheService cacheService)
        {
            this.dynamicRepository = dynamicRepository;
            this.customerService = customerService;
            LogDataAccess = logDataAccess;
            this.session = session;
            this.cacheService = cacheService;
        }

        protected LogDataAccess LogDataAccess { get; }

        internal abstract ICriteria CreateGetLogsCriteria(LogParam logParam);

        internal abstract ICriteria CreateGetTotalLogsCriteria(LogParam logParam);

        public string? GetLogActionName(int actionId, string language)
        {
            var logAction = GetLogAction(actionId);
            switch (language)
            {
                case LanguageCode.ChineseTraditional:
                    return logAction.ChineseTraditionalName;

                case LanguageCode.ChineseSimplified:
                    return logAction.ChineseSimplifiedName;

                case LanguageCode.Japanese:
                    return logAction.JapaneseName;

                case LanguageCode.Thai:
                    return logAction.ThaiName;

                case LanguageCode.Korean:
                    return logAction.KoreanName;

                case LanguageCode.Vietnamese:
                    return logAction.VietnameseName;

                default:
                    return logAction.EnglishName;
            }
        }

        public IEnumerable<LogActionType> GetLogActionTypes()
        {
            var logActionTypes = cacheService.Get<IEnumerable<LogActionType>>(LogActionTypesCacheKey);

            if (logActionTypes != null
                && logActionTypes.Any())
            {
                return logActionTypes;
            }

            logActionTypes = dynamicRepository.Fetch<LogActionType>(new GetAllLogActionTypeCriteria());

            cacheService.Set(LogActionTypesCacheKey, logActionTypes, new CacheItemOptions { SlidingExpiration = TimeSpan.FromHours(OneHour) });

            return logActionTypes;
        }

        public LogAction GetLogAction(int actionId)
        {
            var logActionDictionary = cacheService.Get<Dictionary<int, LogAction>>(LogActionCacheKey);

            if (logActionDictionary == null
                || !logActionDictionary.Any())
            {
                logActionDictionary = dynamicRepository
                    .Fetch<LogAction>(new GetLogActionsCriteria())
                    .ToDictionary(logAction => logAction.Id);

                cacheService.Set(LogActionCacheKey, logActionDictionary, DefaultVariables.CacheItemOptions);
            }

            return logActionDictionary.ContainsKey(actionId) ?
                logActionDictionary[actionId] :
                new LogAction
                {
                    Id = actionId
                };
        }

        public virtual PagingLogs<Log> GetPagingLogs(LogParam logParam)
        {
            var pagingLogs = new PagingLogs<Log>();
            if (logParam == null)
            {
                return pagingLogs;
            }

            var downlineInfo = customerService.GetDownlineInfoByUsername(logParam.Username ?? string.Empty, logParam.UplineId);

            // If search username is not belong to searcher, then return
            if (downlineInfo.CustId == 0)
            {
                return pagingLogs;
            }

            logParam.CustId = downlineInfo.CustId;

            pagingLogs.Logs = GetLogs(logParam);
            pagingLogs.TotalRecords = GetTotalLogs(logParam);

            foreach (var log in pagingLogs.Logs)
            {
                var isHideNickname = IsHideNickName(IsSubAccFromLog(log), downlineInfo.RoleId, downlineInfo.CustId, SubAccNameFromLog(log));
                log.IsHideNickname = isHideNickname;
            }
            
            return pagingLogs;
        }

        protected DateTime GetGreaterDate(DateTime fromDate, DateTime limitedDate)
            => fromDate < limitedDate ? limitedDate : fromDate;

        protected virtual IEnumerable<Log> GetLogs(LogParam logParam)
        {
            var criteria = CreateGetLogsCriteria(logParam);
            return dynamicRepository.Fetch<Log>(criteria);
        }

        protected int GetTotalLogs(LogParam logParam)
        {
            var criteria = CreateGetTotalLogsCriteria(logParam);

            return LogDataAccess.GetTotalRecordsOfLogs(criteria);
        }

        protected bool IsHideNickName(bool isSubAccount, int roleId, int custId, string subUsername)
            => IsIndirect(roleId) 
                || IsSubAccViewMainAcc(isSubAccount, custId) 
                || IsDirectUplineViewSubAcc(isSubAccount, roleId)
                || IsSubAccViewOtherSubAcc(isSubAccount, subUsername);

        private bool IsIndirect (int roleId)
            => session.RoleId - roleId != 1 && session.RoleId != roleId;

        private bool IsDirectUplineViewSubAcc(bool isSubAccount, int roleId)
            => session.RoleId - roleId == 1 && isSubAccount;

        private bool IsSubAccViewMainAcc (bool isSubAccount, int custId)
            => session.IsSubAcc && session.CustId == custId && !isSubAccount;

        private bool IsSubAccViewOtherSubAcc(bool isSubAccount, string subUsername)
            => session.IsSubAcc && isSubAccount && !session.SubUserName.Equals(subUsername, StringComparison.OrdinalIgnoreCase);

        protected static bool IsSubAccFromLog(Log log)
            => Enum.GetValues(typeof(NicknameActions)).Cast<int>().Any(actionId => actionId == log.ActionId) 
                && (new Regex($"(?:SubAccName=)(?<paramValue>[^&|^\\s]+)").IsMatch(log.TrackingValue ?? string.Empty)
                    || new Regex($"(?:SubAccount=)(?<paramValue>[^&|^\\s]+)").IsMatch(log.TrackingValue ?? string.Empty));

        protected static string SubAccNameFromLog(Log log)
        {
            var subAccName = new Regex("(?:SubAccName=)(?<paramValue>[^&|^\\s]+)").Match(log.TrackingValue ?? string.Empty).Groups[1].Value;

            subAccName = !string.IsNullOrWhiteSpace(subAccName) ? subAccName
                : new Regex("(?:SubAccount=)(?<paramValue>[^&|^\\s]+)").Match(log.TrackingValue ?? string.Empty).Groups[1].Value;

            subAccName = !string.IsNullOrWhiteSpace(subAccName) ? subAccName : string.Empty;

            return subAccName.Replace("\"", string.Empty);
        }

        private enum NicknameActions
        {
            CreateMainAccNickname = 621,
            CreateSubAccNickname = 688,
        }
    }
}
