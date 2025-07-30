using System.Resources;
using System.Text;
using System.Xml.Linq;
using AlphaPlus.Settings.ViewLogs.API.Extensions;
using AlphaPlus.Core.Configurations;
using AlphaPlus.Settings.ViewLogs.API.Constants;
using AlphaPlus.Settings.ViewLogs.API.Helpers;
using AlphaPlus.Settings.ViewLogs.API.Params;
using AlphaPlus.Shared.AppResources.Settings.ViewLog;

namespace AlphaPlus.Settings.ViewLogs.API.Services.Impl
{
    public class LogFormatterImpl : LogFormatter
    {
        private readonly ViewLogIPService ipService;
        private readonly RemotingRefWrapper remotingRef;

        private Dictionary<string, bool> custStatuses;
        private Dictionary<string, int> resourcesByProductCacheRef;
        private Dictionary<string, int> resourcesBySportTypeCacheRef;
        private Dictionary<string, int> resourcesByBetTypeCacheRef;
        private bool isHideNickname;

        private const int SevenDaysIndex = 8;

        public LogFormatterImpl(
            ViewLogIPService ipService,
            RemotingRefWrapper remotingRef)
        {
            this.ipService = ipService;
            this.remotingRef = remotingRef;
        }

        public ResourceManager ResourceManager { get; set; } = TrackingValueResources.ResourceManager;

        /// <summary>
        /// Need be reviewed code here, AssignParams should not in formatter class
        /// </summary>
        /// <param name="transformParam"></param>
        public void Init(TransformParam transformParam, bool isHideNickname = false)
        {
            if (transformParam == null)
            {
                return;
            }

            custStatuses = transformParam.CustStatuses;
            resourcesByProductCacheRef = transformParam.ResourcesProductCacheRef;
            resourcesByBetTypeCacheRef = transformParam.ResourcesByBetTypeCacheRef;
            resourcesBySportTypeCacheRef = transformParam.ResourcesBySportTypeCacheRef;
            this.isHideNickname = isHideNickname;
        }

        public string FormatIP(string ip)
        {
            return ipService.FormatIP(ip);
        }

        public string FormatCountryCode(string ip, string country)
        {
            return ipService.FormatCountryCode(ip, country);
        }

        public bool IsVisible(string key)
        {
            return custStatuses.ContainsKey(key) && custStatuses[key];
        }

        public string GetLogResource(string key)
        {
            ResourceManager.IgnoreCase = true;
            var value = ResourceManager.GetString(key) ?? string.Empty;
            if (AppSettings.IsSiteLabel777 && key.Contains("Saba"))
            {
                value = ResourceManager.GetString(key.Replace("Saba", "Virtual")) ?? value;
            }

            return value;
        }

        public string GetSportName(int sportType)
        {
            return remotingRef.GetSportName(sportType);
        }

        public string GetBetTypeName(int betType)
        {
            if (betType == BetTypeId.AllBetNormalGames)
            {
                return TrackingValueResources.AllbetNormalGames;
            }

            return remotingRef.GetBetType(betType);
        }

        public string GetProductName(int productId)
        {
            return remotingRef.GetProductName(productId);
        }

        public string TransferConditionFromCode(string dayCodesWithDiff)
        {
            if (string.IsNullOrWhiteSpace(dayCodesWithDiff))
            {
                return string.Empty;
            }

            var codes = XElement.Parse("<root>" + dayCodesWithDiff + "</root>").Elements().ToList();
            var nodeCount = codes.Count;

            var newValue = string.Empty;
            var oldValue = string.Empty;

            const int nodeCountIncaseOfHavingNewOldDivs = 2;

            if (nodeCount >= nodeCountIncaseOfHavingNewOldDivs)
            {
                newValue = codes.First().Value;

                // remove char ( at the beginning of oldValue
                // 7 days => 7 characters
                oldValue = codes.Last().Value.Substring(1, SevenDaysIndex);

                dayCodesWithDiff = dayCodesWithDiff.Replace(newValue, CovertCodeCondition(newValue))
                                                   .Replace(oldValue, CovertCodeCondition(oldValue));
                return dayCodesWithDiff;
            }

            const int nodeCountIncaseOfUnchangeValuesOrNotShowOldValues = 1;

            if (nodeCount == nodeCountIncaseOfUnchangeValuesOrNotShowOldValues)
            {
                newValue = codes.First().Value;

                // title: "{oldValue} --> {newValue}" or empty
                // old value = Split()[0]
                oldValue = codes.First().Attribute("title").Value.Split()[0];

                dayCodesWithDiff = dayCodesWithDiff.Replace(newValue, CovertCodeCondition(newValue))
                                                   .Replace(oldValue, CovertCodeCondition(oldValue));
                return dayCodesWithDiff;
            }

            return CovertCodeCondition(dayCodesWithDiff);
        }

        public string FormatCodeSite(string code)
        {
            var condition = string.Empty;

            if (string.IsNullOrWhiteSpace(code))
            {
                return condition;
            }

            condition = TrackingValueResources.ResourceManager.GetString("Site_" + code) ?? string.Empty;

            return condition;
        }

        public string FormatErrCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return "0";
            }

            return code;
        }

        public string FormatErrMsg(string message)
        {
            if (string.Equals(message, "no information is available", StringComparison.OrdinalIgnoreCase))
            {
                message = string.Empty;
            }

            return message;
        }

        public string FormatMemberWinLimit(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Replace("x", "X").Replace("unlimited", Resources.Unlimited);
        }

        public string FormatUnlimited(int unlimited)
        {
            return unlimited == -1 ? Resources.Unlimited : unlimited.ToString();
        }

        public string FormatUnlimitedNew(string unlimited)
        {
            return unlimited.Replace("unlimited", Resources.Unlimited);
        }

        public bool IsVisibleKeno(string hideKeno)
        {
            return hideKeno == "0";
        }

        public string GetDisabledKenoBetTypeName(string disableBetType)
        {
            return string.Format("{0} {1}", Resources.Disabled, remotingRef.GetBetTypeName(disableBetType));
        }

        public bool IsRNGCasino(int sportType)
        {
            return sportType == SportId.RNGGame ||
                sportType == SportId.RNGMiniGame ||
                sportType == SportId.RNGMobile;
        }

        public bool IsGoldDeluxe(int sportType)
        {
            return sportType == SportId.GoldDeluxe;
        }

        public string RenderCustStatus(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return string.Empty;
            }

            const string statusHtmlFormat = "<tr><td colspan=\"3\" class=\"colW1\">{0}</td><td colspan=\"3\" class=\"colW2\">{1}</td></tr>";
            var lowerCaseResourceName = name.ToLower();

            var resourceValue = string.Empty;
            if (custStatuses.ContainsKey(lowerCaseResourceName)
                && custStatuses[lowerCaseResourceName]
                && resourcesByProductCacheRef.ContainsKey(lowerCaseResourceName))
            {
                resourceValue = $"{TrackingValueResources.disabled} {GetProductName(resourcesByProductCacheRef[lowerCaseResourceName])}";
            }
            else if (resourcesBySportTypeCacheRef != null &&
                resourcesBySportTypeCacheRef.ContainsKey(lowerCaseResourceName))
            {
                resourceValue = $"{TrackingValueResources.disabled} {GetSportName(resourcesBySportTypeCacheRef[lowerCaseResourceName])}";
            }
            else if (resourcesByBetTypeCacheRef != null &&
                resourcesByBetTypeCacheRef.ContainsKey(lowerCaseResourceName))
            {
                resourceValue = $"{TrackingValueResources.disabled} {GetBetTypeName(resourcesByBetTypeCacheRef[lowerCaseResourceName])}";
            }
            else if (custStatuses.ContainsKey(lowerCaseResourceName)
                && custStatuses[lowerCaseResourceName])
            {
                resourceValue = GetLogResource(name);
            }

            return string.IsNullOrEmpty(resourceValue) ? string.Empty : string.Format(statusHtmlFormat, resourceValue, value);
        }

        private string CovertCodeCondition(string dayCodes)
        {
            char[] codes = dayCodes.Trim().ToCharArray();
            string condition = string.Empty;

            const int indicatorOfWeekly = 7;

            if (!codes.Any()
                || codes.Length < indicatorOfWeekly)
            {
                return condition;
            }

            if (codes[indicatorOfWeekly].ToString() == "0")
            {
                return Resources.Daily;
            }

            var selectedDays = new List<string>();
            var daysInWeek = DateTimeHelper.DaysInWeek();
            var numberOfDaysInWeek = daysInWeek.Count();

            for (var i = 0; i < numberOfDaysInWeek; i++)
            {
                if (codes[i] == '1')
                {
                    selectedDays.Add(daysInWeek[i]);
                }
            }

            return Resources.Weekly + " (" + string.Join(" - ", selectedDays) + ")";
        }

        public string FormatLanguage(string value)
            => value.Replace(LanguageCode.English.ToLowerInvariant(), GetLogResource(LanguageCode.English))
                    .Replace(LanguageCode.ChineseSimplified.ToLowerInvariant(), GetLogResource(LanguageCode.ChineseSimplified))
                    .Replace(LanguageCode.ChineseSimplifiedNew.ToLowerInvariant(), GetLogResource(LanguageCode.ChineseSimplified))
                    .Replace(LanguageCode.ChineseTraditional.ToLowerInvariant(), GetLogResource(LanguageCode.ChineseTraditional))
                    .Replace(LanguageCode.ChineseTraditionalNew.ToLowerInvariant(), GetLogResource(LanguageCode.ChineseTraditional))
                    .Replace(LanguageCode.Japanese.ToLowerInvariant(), GetLogResource(LanguageCode.Japanese))
                    .Replace(LanguageCode.Korean.ToLowerInvariant(), GetLogResource(LanguageCode.Korean))
                    .Replace(LanguageCode.Thai.ToLowerInvariant(), GetLogResource(LanguageCode.Thai))
                    .Replace(LanguageCode.Vietnamese.ToLowerInvariant(), GetLogResource(LanguageCode.Vietnamese));

        public string FormatLanguageKey(string value)
            => value.Replace($">{DBLanguageKey.English.ToLowerInvariant()}<", $">{GetLogResource(LanguageCode.English)}<")
                    .Replace($"({DBLanguageKey.English.ToLowerInvariant()})", $"({GetLogResource(LanguageCode.English)})")
                    .Replace($">{DBLanguageKey.ChineseSimplified.ToLowerInvariant()}<", $">{GetLogResource(LanguageCode.ChineseSimplified)}<")
                    .Replace($"({DBLanguageKey.ChineseSimplified.ToLowerInvariant()})", $"({GetLogResource(LanguageCode.ChineseSimplified)})")
                    .Replace($">{DBLanguageKey.ChineseTraditional.ToLowerInvariant()}<", $">{GetLogResource(LanguageCode.ChineseTraditional)}<")
                    .Replace($"({DBLanguageKey.ChineseTraditional.ToLowerInvariant()})", $"({GetLogResource(LanguageCode.ChineseTraditional)})")
                    .Replace($">{DBLanguageKey.Japanese.ToLowerInvariant()}<", $">{GetLogResource(LanguageCode.Japanese)}<")
                    .Replace($"({DBLanguageKey.Japanese.ToLowerInvariant()})", $"({GetLogResource(LanguageCode.Japanese)})")
                    .Replace($">{DBLanguageKey.Thai.ToLowerInvariant()}<", $">{GetLogResource(LanguageCode.Thai)}<")
                    .Replace($"({DBLanguageKey.Thai.ToLowerInvariant()})", $"({GetLogResource(LanguageCode.Thai)})")
                    .Replace($">{DBLanguageKey.Korean.ToLowerInvariant()}<", $">{GetLogResource(LanguageCode.Korean)}<")
                    .Replace($"({DBLanguageKey.Korean.ToLowerInvariant()})", $"({GetLogResource(LanguageCode.Korean)})")
                    .Replace($">{DBLanguageKey.Vietnamese.ToLowerInvariant()}<", $">{GetLogResource(LanguageCode.Vietnamese)}<")
                    .Replace($"({DBLanguageKey.Vietnamese.ToLowerInvariant()})", $"({GetLogResource(LanguageCode.Vietnamese)})");

        public string FormatChatbotSelfDestructionPeriod(string value)
            => value.Replace(SelfdestructionPeriod.FifteenMinutes.ToLowerInvariant(), GetLogResource(nameof(SelfdestructionPeriod.FifteenMinutes)))
                    .Replace(SelfdestructionPeriod.ThirtyMinutes.ToLowerInvariant(), GetLogResource(nameof(SelfdestructionPeriod.ThirtyMinutes)))
                    .Replace(SelfdestructionPeriod.OneHour.ToLowerInvariant(), GetLogResource(nameof(SelfdestructionPeriod.OneHour)))
                    .Replace(SelfdestructionPeriod.ThreeHours.ToLowerInvariant(), GetLogResource(nameof(SelfdestructionPeriod.ThreeHours)))
                    .Replace(SelfdestructionPeriod.SixHours.ToLowerInvariant(), GetLogResource(nameof(SelfdestructionPeriod.SixHours)))
                    .Replace(SelfdestructionPeriod.TwelveHours.ToLowerInvariant(), GetLogResource(nameof(SelfdestructionPeriod.TwelveHours)))
                    .Replace(SelfdestructionPeriod.OneDay.ToLowerInvariant(), GetLogResource(nameof(SelfdestructionPeriod.OneDay)))
                    .Replace(SelfdestructionPeriod.ThreeDays.ToLowerInvariant(), GetLogResource(nameof(SelfdestructionPeriod.ThreeDays)))
                    .Replace(SelfdestructionPeriod.SevenDays.ToLowerInvariant(), GetLogResource(nameof(SelfdestructionPeriod.SevenDays)))
                    .Replace(SelfdestructionPeriod.FifteenDays.ToLowerInvariant(), GetLogResource(nameof(SelfdestructionPeriod.FifteenDays)))
                    .Replace(SelfdestructionPeriod.ThirtyDays.ToLowerInvariant(), GetLogResource(nameof(SelfdestructionPeriod.ThirtyDays)));

        public string GetPlatformOfSearchAndCopyFunction(string value)
            => string.IsNullOrWhiteSpace(value)
            ? string.Empty
            : GetLogResource($"SystemId_{value}");

        public string FormatNickName(string nickname)
            => isHideNickname ? string.Empty : nickname;

        public string FormatApiKeyScopes(string scopes)
        {
            //<span class="valuechanged">2</span><span class="valueold">(1;2)</span>
            var node = XElement.Parse("<root>" + scopes + "</root>").Elements().ToList();
            var nodeCount = node.Count;
            const int nodeCountInCaseOfHavingOldValue = 2;
            if (nodeCount == nodeCountInCaseOfHavingOldValue)
            {
                var newValue = node.First().Value;
                var oldValue = node.Last().Value;
                node.First().Value = ConvertApiKeyScopes(newValue);
                node.Last().Value = $"({ConvertApiKeyScopes(oldValue.Substring(1, oldValue.Length - 2))})";

                return $"{node.First()}{node.Last()}";
            }

            return ConvertApiKeyScopes(scopes);
        }

        private static string ConvertApiKeyScopes(string scopes)
        {
            var scopeArr = scopes.Split(';');
            var result = new StringBuilder();
            foreach (var scope in scopeArr)
            {
                result.Append(TrackingValueResources.ResourceManager.GetString("APIKeyScope_" + scope)).Append(", ");
            }

            return result.ToString().Remove(result.Length - 2, 2);
        }

        public string FormatDate(string date)
        {
            //<span class=\"valuechanged\">feb  9 2023 12:00am</span><span class=\"valueold\">(null)</span>
            var node = XElement.Parse("<root>" + date + "</root>").Elements().ToList();
            var nodeCount = node.Count;
            const int nodeCountInCaseOfHavingOldValue = 2;
            if (nodeCount == nodeCountInCaseOfHavingOldValue)
            {
                var newValue = node.First().Value;
                var oldValue = node.Last().Value;
                node.First().Value = ConvertApiKeyDate(newValue);
                node.Last().Value = $"({ConvertApiKeyDate(oldValue.Substring(1, oldValue.Length - 2))})";

                return $"{node.First()}{node.Last()}";
            }

            return ConvertApiKeyDate(date);
        }

        private static string ConvertApiKeyDate(string date)
        {
            if (date.Equals("NULL", StringComparison.OrdinalIgnoreCase))
            {
                return TrackingValueResources.ResourceManager.GetString("Never");
            }

            return DateTimeHelper.ToLongDateWithSlash(DateTime.Parse(date));
        }

        public string GetSubProductGroupName(int subGroupId)
        {
            return remotingRef.GetSubProductGroupName(subGroupId);
        }

        public string GetProductGroupName(int productGroupId)
        {
            return remotingRef.GetProductGroupName(productGroupId);
        }

        public string GetSportbookCommissionName(int sportType, int betType)
        {
            return remotingRef.GetSportbookCommissionName(sportType, betType);
        }

        public string GetSportbookMemberCommissionName(int sportType, int betType)
        {
            return remotingRef.GetSportbookMemberCommissionName(sportType, betType);
        }

        public string GetRNGGamesBetSettingName(int sportType, int betType)
        {
            return remotingRef.GetRNGGamesBetSettingName(sportType, betType);
        }

        public string GetParlayMaxPayoutSportBetTypeName(int sportType, int betType)
        {
            return remotingRef.GetParlayMaxPayoutSportBetTypeName(sportType, betType);
        }

        public string GetSportsBookBetSettingName(int sportType, int betType)
        {
            return remotingRef.GetSportsBookBetSettingName(sportType, betType);
        }

        public string GetSportsBookPositionTaking(int sportType, int betType)
        {
            return remotingRef.GetSportsBookPositionTaking(sportType, betType);
        }

        public string GetDisplayName(int settingLevel, int sportType, int betType, int productId)
        {
            switch (settingLevel)
            {
                case SettingLevels.Product:
                    return GetProductName(productId);

                case SettingLevels.SportType:
                    return GetSportName(sportType);

                case SettingLevels.BetType:
                    return GetBetTypeName(betType);

                default:
                    return $"{GetSportName(sportType)} - {GetBetTypeName(betType)}";
            }
        }

        public string FormatParlayMaxPayout(decimal value)
        {
            return value == 0 ? Resources.Unlimited : value.ToNumericString();
        }

        public string FormatWinLossLimit(decimal value)
        {
            return value == 0 ? Resources.Unlimited : value.ToNumericString();
        }

        public string GetOtherGamesBetSettingName(int sportType, int betType)
        {
            return remotingRef.GetOtherGamesBetSettingName(sportType, betType);
        }

        public string FormatAutoTransferSchedule(string schedulesDiff)
        {
            return FormatCustomValue(schedulesDiff, CovertSchedule);
        }

        private static string FormatCustomValue(string valuesDiff, Func<string, string> formatDiffFunc)
        {
            if (string.IsNullOrWhiteSpace(valuesDiff))
            {
                return string.Empty;
            }

            var codes = XElement.Parse("<root>" + valuesDiff + "</root>").Elements().ToList();
            var nodeCount = codes.Count;

            var newValue = string.Empty;
            var oldValue = string.Empty;

            const int nodeCountIncaseOfHavingNewOldDivs = 2;
            var result = valuesDiff;

            if (nodeCount >= nodeCountIncaseOfHavingNewOldDivs)
            {
                newValue = codes.First().Value;

                // remove char ( and ) of oldValue
                var l = codes.Last().Value;
                oldValue = l.Substring(1, l.Length - 2);
                return ReplaceValues(oldValue, newValue, valuesDiff, formatDiffFunc);
            }

            const int nodeCountIncaseOfUnchangeValuesOrNotShowOldValues = 1;

            if (nodeCount == nodeCountIncaseOfUnchangeValuesOrNotShowOldValues)
            {
                newValue = codes.First().Value;

                // title: "{oldValue} --> {newValue}" or empty
                // old value = Split()[0]
                oldValue = codes.First().Attribute("title").Value.Split()[0];
                return ReplaceValues(oldValue, newValue, valuesDiff, formatDiffFunc);
            }

            return formatDiffFunc(result);
        }

        public static string CovertSchedule(string schedule)
        {
            var dates = schedule.Trim().ToCharArray();
            if (schedule == "_" || !dates.Any())
            {
                return "-";
            }

            if (dates.Length == 7)
            {
                return Resources.Daily;
            }

            string[] dateNames = {
                Resources.Sun,
                Resources.Mon,
                Resources.Tue,
                Resources.Wed,
                Resources.Thurs,
                Resources.Fri,
                Resources.Sat
            };

            return string.Join("-", dates.Select(d => dateNames[int.Parse(d.ToString()) - 1]));
        }

        public string GetBankName(string code)
        {
            return TrackingValueResources.ResourceManager.GetString("BankName_" + code) ?? string.Empty;
        }

        private static string ReplaceValues(
            string oldValue,
            string newValue,
            string diff,
            Func<string, string> formatDiffFunc)
        {
            var result = diff;

            if (string.IsNullOrWhiteSpace(newValue))
            {
                result = result.Replace("<span class=\"valuechanged\"></span>", "<span class=\"valuechanged\">-</span>");
            }

            if (oldValue.Contains(newValue))
            {
                result = result.Replace($"({oldValue})", $"({formatDiffFunc(oldValue)})");

                if (!string.IsNullOrWhiteSpace(newValue))
                {
                    result = result.Replace(newValue, formatDiffFunc(newValue));
                }

                return result;
            }

            if (!string.IsNullOrWhiteSpace(newValue))
            {
                result = result.Replace(newValue, formatDiffFunc(newValue));
            }

            result = result.Replace($"({oldValue})", $"({formatDiffFunc(oldValue)})");
            return result;
        }
    }
}
