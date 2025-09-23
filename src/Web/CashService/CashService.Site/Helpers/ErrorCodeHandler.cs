namespace AlphaPlus.CashService.Site.Helpers;

using Features.Shared.Models;
using Shared.AppResources.CashServices;

public static class ErrorCodeHandler
{
    private const int AccountDisabled = 401;
    private const int AccountClosed = 402;
    private const int AccountSuspended = 404;
    private const int InvalidParameters = 405;
    private const int InsufficientBalance = 406;
    private const int AmountExceedsLimits = 407;
    private const int DailyLimitExceeded = 408;
    private const int TransactionInsertFailed = 409;
    private const int ProviderRejected = 410;
    private const int ProviderUnavailable = 411;
    private const int InvalidMember = 415;
    private const int InvalidTransaction = 416;
    private const int ExceedAgentAvailableCredit = 417;
    private const int OutRangeMinMaxAmount = 419;
    private const int InternalServerError = 499;
    private const int ServerError = 500;

    private static string GetAccountDisabledMessage() => CashServicesResx.AccountDisabled;

    private static string GetAccountClosedMessage() => CashServicesResx.AccountClosed;

    private static string GetAccountSuspendedMessage() => CashServicesResx.TheAccountWasSuspended;

    private static string GetInvalidParametersMessage() => CashServicesResx.InvalidParameters;

    private static string GetAmountExceedsLimitsMessage() => CashServicesResx.TheAmountExceedsTheAllowedMinimumOrMaximumValue;

    private static string GetTransactionInsertFailedMessage() => CashServicesResx.TheTransactionInsertedFailedPleaseTryAgain;

    private static string GetInvalidMemberMessage(int memberId, int agentId) => string.Format(CashServicesResx.InvalidMemberErrMsg, memberId, agentId);

    private static string GetInvalidTransactionMessage(string transId, int memberId)
        => string.Format(CashServicesResx.InvalidTransactionErrMsg, transId, memberId);

    private static readonly Dictionary<int, Func<int, int, ResultModel, string>> WithdrawalErrorResolvers =
        new()
        {
            { 400, (_, _, _) => CashServicesResx.ModelValidationFailed },
            { AccountDisabled, (_, _, _) => GetAccountDisabledMessage() },
            { AccountClosed, (_, _, _) => GetAccountClosedMessage() },
            { AccountSuspended, (_, _, _) => GetAccountSuspendedMessage() },
            { InvalidParameters, (_, _, _) => GetInvalidParametersMessage() },
            { InsufficientBalance, (_, _, result) => string.Format(CashServicesResx.TheAmountMustNotExceedMemberCashBalance,result.GetDataValue("MemberCashBalance")) },
            { AmountExceedsLimits, (_, _, _) => CashServicesResx.TheAmountMustBeBetween0And1AsPerProviderLimits },
            { DailyLimitExceeded, (_, _, _) => CashServicesResx.ExceedsTheDailyWithdrawalLimit },
            { TransactionInsertFailed, (_, _, _) => GetTransactionInsertFailedMessage() },
            { ProviderRejected, (_, _, _) => CashServicesResx.ProviderRejectErrorMessage },
            { ProviderUnavailable, (_, _, _) => CashServicesResx.PaymentProviderUnavailable },
            { InvalidMember, (memberId, agentId, _) => GetInvalidMemberMessage(memberId, agentId) },
            { OutRangeMinMaxAmount, (_, _, result) => string.Format(
                CashServicesResx.TheAmountMustBeBetween0And1AsPerProviderLimits,
                result.GetDataValue("MinAmount"),
                result.GetDataValue("MaxAmount") )
            },
            { ServerError, (_, _, _) => CashServicesResx.AnErrorOccurredWhileProcessingWithdrawalRequest },
        };

    private static readonly Dictionary<int, Func<string, int, int, string>> PromotionStatusErrorResolvers =
        new()
        {
            { 400, (_, _, _) => CashServicesResx.ModelValidationFailed },
            { AccountDisabled, (_, _, _) => GetAccountDisabledMessage() },
            { AccountClosed, (_, _, _) => GetAccountClosedMessage() },
            { AccountSuspended, (_, _, _) => GetAccountSuspendedMessage() },
            { InvalidParameters, (_, _, _) => GetInvalidParametersMessage() },
            { InsufficientBalance, (_, _, _) => CashServicesResx.MemberCashBalanceIsInsufficient },
            { DailyLimitExceeded, (_, _, _) => CashServicesResx.ExceedsTheDailyWithdrawalLimit },
            { TransactionInsertFailed, (_, _, _) => GetTransactionInsertFailedMessage() },
            { InvalidMember, (_, memberId, agentId) => GetInvalidMemberMessage(memberId, agentId) },
            { InvalidTransaction, (transId, memberId, _) => GetInvalidTransactionMessage(transId, memberId) },
            { ServerError, (_, _, _) => CashServicesResx.AnErrorOccurredWhileProcessingWithdrawalRequest },
        };

    private static readonly Dictionary<int, Func<ResultModel, string, int, int, string>> UpdateTransactionStatusResolvers =
        new()
        {
            { InvalidParameters, (_, _, _, _) => GetInvalidParametersMessage() },
            { AmountExceedsLimits, (_, _, _, _) => GetAmountExceedsLimitsMessage() },
            { TransactionInsertFailed, (_, _, _, _) => GetTransactionInsertFailedMessage() },
            { ProviderRejected, (_, _, _, _) => CashServicesResx.ProviderRejectErrorMessage },
            { ProviderUnavailable, (_, _, _, _) => CashServicesResx.PaymentProviderUnavailable },
            { InvalidMember, (_, _, memberId, agentId) => GetInvalidMemberMessage(memberId, agentId) },
            { InsufficientBalance, (result, _, _, _) => string.Format(CashServicesResx.TheAmountMustNotExceedMemberCashBalance, result.GetDataValue("MemberCashBalance")) },
            { ExceedAgentAvailableCredit, (result, _, _, _) => string.Format(CashServicesResx.TheAmountMustNotExceedAgentAvailableCredit, result.GetDataValue("AgentAvailableCredit"))},
            { OutRangeMinMaxAmount, (result, _, _, _) => string.Format(
                            CashServicesResx.TheAmountMustBeBetween0And1AsPerProviderLimits,
                            result.GetDataValue("MinAmount"),
                            result.GetDataValue("MaxAmount"))
            },
            { InvalidTransaction, (_, transId, memberId, _) => GetInvalidTransactionMessage(transId, memberId) },
            { InternalServerError, (result, _, _, _) => $"{CashServicesResx.InternalServerError} {result.Message}" },
        };

    public static string MappingAddManualDepositErrorCode(ResultModel result) =>
        result.Code switch
        {
            AccountDisabled => GetAccountDisabledMessage(),
            AccountClosed => GetAccountClosedMessage(),
            403 => CashServicesResx.UnableToProcessDeposits,
            AccountSuspended => GetAccountSuspendedMessage(),
            InvalidParameters => GetInvalidParametersMessage(),
            AmountExceedsLimits => GetAmountExceedsLimitsMessage(),
            ExceedAgentAvailableCredit => string.Format(CashServicesResx.TheAmountMustNotExceedAgentAvailableCredit, result.GetDataValue("AgentAvailableCredit")),
            TransactionInsertFailed => GetTransactionInsertFailedMessage(),
            _ => GetTransactionInsertFailedMessage()
        };

    public static string MappingAddManualWithdrawalErrCode(ResultModel result, int memberId, int agentId)
        => WithdrawalErrorResolvers.TryGetValue(result.Code, out var resolve)
            ? resolve(memberId, agentId, result)
            : GetTransactionInsertFailedMessage();

    public static string MappingUpdatePromotionTransactionStatusErrCode(int errCode, string transId, int memberId, int agentId)
        => PromotionStatusErrorResolvers.TryGetValue(errCode, out var resolve)
            ? resolve(transId, memberId, agentId)
            : GetTransactionInsertFailedMessage();

    public static string MappingUpdateTransactionStatus(ResultModel result, string transId, int memberId, int agentId)
        => UpdateTransactionStatusResolvers.TryGetValue(result.Code, out var resolve)
            ? resolve(result, transId, memberId, agentId)
            : result.Message;

    public static string MappingRevertTransactionErrCode(ResultModel result, string transId, int memberId, int agentId)
        => result.Code switch
        {
            InsufficientBalance => string.Format(CashServicesResx.TheAmountMustNotExceedMemberCashBalance, result.GetDataValue("MemberCashBalance")),
            AmountExceedsLimits => GetAmountExceedsLimitsMessage(),
            TransactionInsertFailed => GetTransactionInsertFailedMessage(),
            InvalidMember => GetInvalidMemberMessage(memberId, agentId),
            InvalidTransaction => GetInvalidTransactionMessage(transId, memberId),
            ExceedAgentAvailableCredit => string.Format(CashServicesResx.TheAmountMustNotExceedAgentAvailableCredit, result.GetDataValue("AgentAvailableCredit")),

            _ => GetTransactionInsertFailedMessage()
        };
}