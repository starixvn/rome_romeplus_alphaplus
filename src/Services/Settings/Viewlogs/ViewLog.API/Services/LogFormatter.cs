using AlphaPlus.Settings.ViewLogs.API.Params;

namespace AlphaPlus.Settings.ViewLogs.API.Services
{
    public interface LogFormatter
    {
        void Init(TransformParam transformParam, bool isHideNickname = false);

        string FormatIP(string ip);

        string FormatCountryCode(string ip, string country);

        bool IsVisible(string key);

        string GetLogResource(string key);

        string GetSportName(int sportType);

        string GetBetTypeName(int betType);

        string GetProductName(int productId);

        string TransferConditionFromCode(string dayCodesWithDiff);

        string FormatErrCode(string code);

        string FormatErrMsg(string message);

        string FormatMemberWinLimit(string value);

        string FormatUnlimited(int unlimited);

        string FormatUnlimitedNew(string unlimited);

        string FormatLanguage(string value);

        string FormatLanguageKey(string value);

        bool IsVisibleKeno(string hideKeno);

        string GetDisabledKenoBetTypeName(string disableBetType);

        bool IsRNGCasino(int sportType);

        bool IsGoldDeluxe(int sportType);

        string RenderCustStatus(string name, string value);

        string FormatChatbotSelfDestructionPeriod(string value);

        string GetPlatformOfSearchAndCopyFunction(string value);

        string FormatNickName(string nickname);

        string FormatApiKeyScopes(string scopes);

        string FormatDate(string date);

        string GetSubProductGroupName(int subGroupId);

        string GetProductGroupName(int productGroupId);

        string GetSportbookCommissionName(int sportType, int betType);

        string GetSportbookMemberCommissionName(int sportType, int betType);

        string GetRNGGamesBetSettingName(int sportType, int betType);

        string GetOtherGamesBetSettingName(int sportType, int betType);

        string GetParlayMaxPayoutSportBetTypeName(int sportType, int betType);

        string GetSportsBookBetSettingName(int sportType, int betType);

        string GetDisplayName(int settingLevel, int sportType, int betType, int productId);

        string FormatParlayMaxPayout(decimal value);

        string FormatWinLossLimit(decimal value);

        string FormatAutoTransferSchedule(string schedulesDiff);
    }
}