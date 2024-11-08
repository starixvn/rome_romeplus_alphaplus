using System.Text.RegularExpressions;
using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.Shared.Constants;
using AlphaPlus.Shared.AppResources.Report;
using Fanex.Caching;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.BetCreditTracking.BetCreditHistory;

public interface IBetCreditHistoryService
{
    Task<BetCreditHistoryModel> GetBetCreditHistory(BetCreditHistoryParam param);
}

public class BetCreditHistoryService(
    IDynamicRepository repository,
    IPartialUMService partialUMService,
    ICacheService cacheService) : IBetCreditHistoryService
{
    private const int LogActionCacheInMinutes = 60;
    private const string LogActionCacheKey = "AlphaPlus.Reports.BetCreditTracking.BetCreditHistory.LogAction";

    private const string TransLogSettled = "TransLog settled,";
    private const string TransLog = "TransLog";
    private const string LC = "LC";
    private const string RNG = "RNG";
    private const string Bet = "Bet";
    private const string BetUpdated = "Bet updated,";
    private const string BetSettled = "Bet settled,";
    private const string BetRejected = "Bet rejected";
    private const string VOID = "VOID";
    private const string Move = "Move";
    private const string voided = "voided";
    private const string to = "to";
    private const string to0 = "to 0";
    private const string to1 = "to 1";

    private readonly IDynamicRepository repository = repository;
    private readonly IPartialUMService partialUMService = partialUMService;
    private readonly ICacheService cacheService = cacheService;

    public async Task<BetCreditHistoryModel> GetBetCreditHistory(BetCreditHistoryParam param)
    {
        var criteria = new BetCreditHistoryCriteria(
            param.Member,
            param.FromDate,
            param.ToDate,
            param.RefNo,
            param.ViewerId,
            param.PageIndex,
            param.PageSize);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var rawdata = await repository.FetchAsync<BetCreditHistoryEntity>(criteria);
        foreach (var row in rawdata)
        {
            row.Action = row.IsUserLogActionId
                ? await GetLogActionName(row.Action, row.ActionId, param.Language)
                : ConvertActionName(row.Action, row.ActionId);
        }

        return new BetCreditHistoryModel(criteria.ErrorCode, criteria.ErrorMess, rawdata, criteria.TotalRows);
    }

    private async Task<string> GetLogActionName(string action, int actionId, string language)
    {
        var logAction = await GetLogAction(actionId);
        if (logAction == null)
        {
            return action;
        }

        return language switch
        {
            LanguageCode.ChineseTraditional => logAction.ChineseTraditionalName,
            LanguageCode.ChineseSimplified => logAction.ChineseSimplifiedName,
            LanguageCode.Japanese => logAction.JapaneseName,
            LanguageCode.Thai => logAction.ThaiName,
            LanguageCode.Korean => logAction.KoreanName,
            LanguageCode.Vietnamese => logAction.VietnameseName,
            _ => logAction.EnglishName
        };
    }

    private async Task<LogAction> GetLogAction(int actionId)
    {
        var logActionDictionary = await cacheService.GetOrSetAsync(
            LogActionCacheKey,
            async () =>
            {
                var logActions = await repository.FetchAsync<LogAction>(new GetLogActionsCriteria());
                return logActions.ToDictionary(logAction => logAction.Id);
            },
            new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(LogActionCacheInMinutes))
        );

        return logActionDictionary.TryGetValue(actionId, out var logAction)
            ? logAction
            : null;
    }

    private static string ConvertActionName(string action, int actionId)
    {
        switch (actionId)
        {
            case -1:
                return BetCreditHistoryResources.OpeningCredit;

            case 0:
                return TransformActionIdEqualsTo0(action);

            case 2:
                return GetActionName2(action);

            case 10:
            case 11:
            case 12:
                return GetActionName10(action);

            case 13:
                return GetActionName13(action);

            case 20:
                return TransformActionIdEqualsTo20(action);

            case 29:
                return GetActionStatusToStatus(BetCreditHistoryResources.CashOut, action);

            case 30:
                return TransformActionIdEqualsTo30(action);

            case 31:
                return TransformActionIdEqualsTo31(action);

            case 32:
                return TransformActionIdEqualsTo32(action);

            case 34:
                return GetActionStatusToStatus(BetCreditHistoryResources.CancelCashOut, action);

            case 50:
                return TransformActionIdEqualsTo50(action);

            case 55:
                return TransformActionIdEqualsTo55(action);

            case 110:
                return GetTransferSubmitted(action);

            default:
                return action;
        }
    }

    private static string TransformActionIdEqualsTo0(string action)
    {
        if (IsMatchPrefixPattern(action, TransLogSettled) || IsMatchPrefixPattern(action, BetUpdated))
        {
            return GetActionStatusToStatus(BetCreditHistoryResources.BetUpdated, action);
        }

        if (IsMatchPrefixPattern(action, LC) || IsMatchPrefixPattern(action, RNG))
        {
            return IsHaveStatusToStatus(action)
                ? GetActionStatusToStatus(BetCreditHistoryResources.BetUpdated, action)
                : BetCreditHistoryResources.BetUpdated;
        }

        return action;
    }

    private static string TransformActionIdEqualsTo20(string action)
    {
        if (action.Trim().Equals(BetRejected, StringComparison.OrdinalIgnoreCase))
        {
            return BetCreditHistoryResources.BetRejected;
        }

        return action;
    }

    private static string TransformActionIdEqualsTo30(string action)
    {
        if (IsMatchPrefixPattern(action, BetUpdated))
        {
            return GetActionStatusToStatus(BetCreditHistoryResources.BetUpdated, action);
        }

        if (IsMatchPrefixPattern(action, TransLogSettled)
            || IsMatchPrefixPattern(action, LC)
            || IsMatchPrefixPattern(action, RNG)
            || IsMatchPrefixPattern(action, BetSettled))
        {
            return GetActionStatusToStatus(BetCreditHistoryResources.BetSettled, action);
        }

        return action;
    }

    private static string TransformActionIdEqualsTo31(string action)
    {
        if (IsMatchPrefixPattern(action, TransLog) || IsMatchPrefixPattern(action, BetUpdated))
        {
            return GetActionStatusToStatus(BetCreditHistoryResources.BetUpdated, action);
        }

        if (IsMatchPrefixPattern(action, LC) || IsMatchPrefixPattern(action, RNG))
        {
            return IsHaveStatusToStatus(action)
                ? GetActionStatusToStatus(BetCreditHistoryResources.BetUpdated, action)
                : BetCreditHistoryResources.BetUpdated;
        }

        return action;
    }

    private static string TransformActionIdEqualsTo32(string action)
    {
        if (IsMatchPrefixPattern(action, TransLog)
            || IsMatchPrefixPattern(action, BetUpdated)
            || IsMatchPrefixPattern(action, Move))
        {
            return GetActionStatusToStatus(BetCreditHistoryResources.BetUpdated, action);
        }

        if (IsMatchPrefixPattern(action, LC) || IsMatchPrefixPattern(action, RNG))
        {
            return IsHaveStatusToStatus(action)
                ? GetActionStatusToStatus(BetCreditHistoryResources.BetUpdated, action)
                : BetCreditHistoryResources.BetUpdated;
        }

        return action;
    }

    private static string TransformActionIdEqualsTo50(string action)
    {
        if (IsMatchPrefixAndSuffixPattern(action, Bet, VOID)
            || IsMatchPrefixAndSuffixPattern(action, TransLog, VOID)
            || IsMatchPrefixAndSuffixPattern(action, LC, VOID)
            || IsMatchPrefixAndSuffixPattern(action, RNG, VOID))
        {
            return GetActionStatusToStatus(BetCreditHistoryResources.BetUpdated, action);
        }

        if (IsMatchPrefixAndSuffixPattern(action, Bet, voided)
            || IsMatchPrefixAndSuffixPattern(action, TransLog, voided)
            || IsMatchPrefixAndSuffixPattern(action, LC, voided)
            || IsMatchPrefixAndSuffixPattern(action, RNG, voided))
        {
            return GetBetUpdatedRunningToVoid();
        }

        return action;
    }

    private static string TransformActionIdEqualsTo55(string action)
    {
        if (IsMatchPrefixPattern(action, TransLog))
        {
            return GetActionStatusToStatus(BetCreditHistoryResources.BetUpdated, action);
        }

        if (IsMatchPrefixPattern(action, LC) || IsMatchPrefixPattern(action, RNG))
        {
            return GetBetUpdatedVoidToStatus(action);
        }

        return action;
    }

    private static bool IsHaveStatusToStatus(string action)
        => action.IndexOf(to, StringComparison.OrdinalIgnoreCase) != -1 &&
            action.IndexOf(to0, StringComparison.OrdinalIgnoreCase) == -1 &&
            action.IndexOf(to1, StringComparison.OrdinalIgnoreCase) == -1;

    private static bool IsMatchPrefixAndSuffixPattern(string str, string prefix, string suffix)
    {
        const string prefixAndSuffixPattern = "^({0})(.*)({1})$";

        return Regex.IsMatch(str, string.Format(prefixAndSuffixPattern, prefix, suffix), RegexOptions.IgnoreCase);
    }

    private static bool IsMatchPrefixPattern(string str, string prefix)
    {
        const string prefixPattern = "^({0})";

        return Regex.IsMatch(str, string.Format(prefixPattern, prefix), RegexOptions.IgnoreCase);
    }

    private static string GetActionStatusToStatus(string name, string action)
    {
        var statusToStatus = action.Split(',')[1];

        var splitStatus = statusToStatus.Split(new[] { "to" }, StringSplitOptions.None);
        var fromStatus = splitStatus[0].Trim().ToLowerInvariant();
        var toStatus = splitStatus[1].Trim().ToLowerInvariant();

        return new string[]
        {
            name,
            ", ",
            BetCreditHistoryResources.ResourceManager.GetString(fromStatus),
            " ",
            BetCreditHistoryResources.to,
            " ",
            BetCreditHistoryResources.ResourceManager.GetString(toStatus)
        }.JoinWithComma();
    }

    private static string GetBetUpdatedVoidToStatus(string action)
    {
        var status = action.Split(new[] { "to" }, StringSplitOptions.None)[1].Trim().ToLowerInvariant();

        return new string[]
        {
            BetCreditHistoryResources.BetUpdated,
            ", ",
            BetCreditHistoryResources._void,
            " ",
            BetCreditHistoryResources.to,
            " ",
            BetCreditHistoryResources.ResourceManager.GetString(status)
        }.JoinWithComma("");
    }

    private static string GetBetUpdatedRunningToVoid()
        => new string[]
        {
            BetCreditHistoryResources.BetUpdated,
            ", ",
            BetCreditHistoryResources.running,
            " ",
            BetCreditHistoryResources.to,
            " ",
            BetCreditHistoryResources._void
        }.JoinWithComma("");

    private static string GetTransferSubmitted(string action)
    {
        var refNo = action.Split(new[] { "inserted" }, StringSplitOptions.None)[1].Trim();
        return $"{BetCreditHistoryResources.TransferSubmitted} {refNo}";
    }

    private static string GetActionName2(string action)
    {
        var strTo = " to ";
        var toPos = action.ToLowerInvariant().LastIndexOf(strTo);

        var prefixLength = 26;
        var d1 = action.Substring(prefixLength, toPos - prefixLength);
        var d2 = action.Substring(toPos + strTo.Length, action.Length - toPos - strTo.Length);

        return string.Format(BetCreditHistoryResources.Action2, d1, d2);
    }

    private static string GetActionName13(string action)
    {
        var strTo = " to ";
        var toPos = action.ToLowerInvariant().LastIndexOf(strTo);

        var prefixLength = 13;
        var s1 = action.Substring(prefixLength, toPos - prefixLength).ToLowerInvariant();
        var s2 = action.Substring(toPos + strTo.Length, action.Length - toPos - strTo.Length).ToLowerInvariant();

        return new string[]
        {
            BetCreditHistoryResources.BetSettled,
            ", ",
            BetCreditHistoryResources.ResourceManager.GetString(s1),
            " ",
            BetCreditHistoryResources.to,
            " ",
            BetCreditHistoryResources.ResourceManager.GetString(s2)
        }.JoinWithComma("");
    }

    private static string GetActionName10(string action)
    {
        var a = action.ToLowerInvariant();
        if (a == "waiting" || a == "running")
        {
            return BetCreditHistoryResources.BetPlaced;
        }

        var afterAs = action.Split(new[] { "as" }, StringSplitOptions.None)[1].Trim();
        var status = afterAs.Split(' ')[0].Trim().ToLowerInvariant();
        return $"{BetCreditHistoryResources.BetPlacedAs} {BetCreditHistoryResources.ResourceManager.GetString(status)}";
    }
}