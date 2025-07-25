using System.Text.RegularExpressions;
using AlphaPlus.Core.Models;
using AlphaPlus.Settings.ViewLogs.API.Constants;
using AlphaPlus.Settings.ViewLogs.API.Criteria;
using AlphaPlus.Settings.ViewLogs.API.DataAccesses;
using AlphaPlus.Settings.ViewLogs.API.Models;
using AlphaPlus.Settings.ViewLogs.API.Params;
using Fanex.Data.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace AlphaPlus.Settings.ViewLogs.API.Services.Impl.Logs
{
    public abstract class BaseService
    {
        protected const int OneHour = 1;
        private const string LogActionCacheKey = "AlphaPlus.Settings.ViewLogs.API.Setting.LogAction";
        private const string LogActionTypesCacheKey = "AlphaPlus.Settings.ViewLogs.API.Setting.LogActionTypes";

        protected readonly IMemoryCache cacheService;
        protected readonly IDynamicRepository dynamicRepository;
        private readonly CustomerService customerService;
        private readonly IUserSession session;

        protected BaseService(IDynamicRepository dynamicRepository, CustomerService customerService, LogDataAccess logDataAccess, IUserSession session, IMemoryCache cacheService)
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

        public async Task<string> GetLogActionName(int actionId, string language)
        {
            var logAction = await GetLogAction(actionId);
            return language switch
            {
                LanguageCode.ChineseTraditional => logAction.ChineseTraditionalName,
                LanguageCode.ChineseSimplified => logAction.ChineseSimplifiedName,
                LanguageCode.Japanese => logAction.JapaneseName,
                LanguageCode.Thai => logAction.ThaiName,
                LanguageCode.Korean => logAction.KoreanName,
                LanguageCode.Vietnamese => logAction.VietnameseName,
                _ => logAction.EnglishName,
            };
        }

        public virtual async Task<IEnumerable<LogActionType>> GetLogActionTypes()
        {
            var logActionTypes = cacheService.Get<IEnumerable<LogActionType>>(LogActionTypesCacheKey);

            if (logActionTypes != null
                && logActionTypes.Any())
            {
                return logActionTypes;
            }

            logActionTypes = await dynamicRepository.FetchAsync<LogActionType>(new GetAllLogActionTypeCriteria());

            cacheService.Set(LogActionTypesCacheKey, logActionTypes, new MemoryCacheEntryOptions { SlidingExpiration = TimeSpan.FromHours(OneHour) });

            return logActionTypes;
        }

        public async Task<LogAction> GetLogAction(int actionId)
        {
            var logActionDictionary = cacheService.Get<Dictionary<int, LogAction>>(LogActionCacheKey);

            if (logActionDictionary == null
                || logActionDictionary.Count == 0)
            {
                logActionDictionary = (await dynamicRepository
                    .FetchAsync<LogAction>(new GetLogActionsCriteria()))
                    .ToDictionary(logAction => logAction.Id);

                cacheService.Set(LogActionCacheKey, logActionDictionary, DefaultVariables.CacheItemOptions);
            }

            return logActionDictionary.TryGetValue(actionId, out LogAction value) ? value :
                new LogAction
                {
                    Id = actionId
                };
        }

        public virtual async Task<PagingLogs<Log>> GetPagingLogs(LogParam logParam)
        {
            var pagingLogs = new PagingLogs<Log>();
            if (logParam == null)
            {
                return pagingLogs;
            }

            var downlineInfo = await customerService.GetDownlineInfoByUsername(logParam.Username ?? string.Empty, logParam.UplineId);

            // If search username is not belong to searcher, then return
            if (downlineInfo.CustId == 0)
            {
                return pagingLogs;
            }

            logParam.CustId = downlineInfo.CustId;

            pagingLogs.Logs = await GetLogs(logParam);
            pagingLogs.TotalRecords = await GetTotalLogs(logParam);

            foreach (var log in pagingLogs.Logs)
            {
                var isHideNickname = IsHideNickName(IsSubAccFromLog(log), downlineInfo.RoleId, downlineInfo.CustId, SubAccNameFromLog(log));
                log.IsHideNickname = isHideNickname;
            }

            return pagingLogs;
        }

        protected DateTime GetGreaterDate(DateTime fromDate, DateTime limitedDate)
            => fromDate < limitedDate ? limitedDate : fromDate;

        protected virtual Task<IEnumerable<Log>> GetLogs(LogParam logParam)
        {
            var criteria = CreateGetLogsCriteria(logParam);
            return dynamicRepository.FetchAsync<Log>(criteria);
        }

        protected Task<int> GetTotalLogs(LogParam logParam)
        {
            var criteria = CreateGetTotalLogsCriteria(logParam);

            return LogDataAccess.GetTotalRecordsOfLogs(criteria);
        }

        protected bool IsHideNickName(bool isSubAccount, int roleId, int custId, string subUsername)
            => IsIndirect(roleId)
                || IsSubAccViewMainAcc(isSubAccount, custId)
                || IsDirectUplineViewSubAcc(isSubAccount, roleId)
                || IsSubAccViewOtherSubAcc(isSubAccount, subUsername);

        private bool IsIndirect(int roleId)
            => session.RoleId - roleId != 1 && session.RoleId != roleId;

        private bool IsDirectUplineViewSubAcc(bool isSubAccount, int roleId)
            => session.RoleId - roleId == 1 && isSubAccount;

        private bool IsSubAccViewMainAcc(bool isSubAccount, int custId)
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
