namespace AlphaPlus.CashService.Site.Helpers;

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
    private const int InternalServerError = 499;
    private const int ServerError = 500;

    private static string GetAccountDisabledMessage() => CashServicesResx.AccountDisabled;

    private static string GetAccountClosedMessage() => CashServicesResx.AccountClosed;

    private static string GetAccountSuspendedMessage() => CashServicesResx.TheAccountWasSuspended;

    private static string GetInvalidParametersMessage() => CashServicesResx.InvalidParameters;

    private static string GetAmountExceedsLimitsMessage() => CashServicesResx.TheAmountExceedsTheAllowedMinimumOrMaximumValue;

    private static string GetTransactionInsertFailedMessage() => CashServicesResx.TheTransactionInsertedFailedPleaseTryAgain;

    private static string GetInvalidMemberMessage(int memberId, int agentId) => string.Format(CashServicesResx.InvalidMemberErrMsg, memberId, agentId);

    private static string GetInvalidTransactionMessage(string transId, int memberId) => string.Format(CashServicesResx.InvalidTransactionErrMsg, transId, memberId);

    private static readonly Dictionary<int, Func<int, int, string>> WithdrawalErrorResolvers =
        new()
        {
            { 400, (_, _) => CashServicesResx.ModelValidationFailed },
            { AccountDisabled, (_, _) => GetAccountDisabledMessage() },
            { AccountClosed, (_, _) => GetAccountClosedMessage() },
            { AccountSuspended, (_, _) => GetAccountSuspendedMessage() },
            { InvalidParameters, (_, _) => GetInvalidParametersMessage() },
            { InsufficientBalance, (_, _) => CashServicesResx.MemberCashBalanceIsInsufficient },
            { AmountExceedsLimits, (_, _) => CashServicesResx.TheAmountMustBeBetween0And1AsPerProviderLimits },
            { DailyLimitExceeded, (_, _) => CashServicesResx.ExceedsTheDailyWithdrawalLimit },
            { TransactionInsertFailed, (_, _) => GetTransactionInsertFailedMessage() },
            { ProviderRejected, (_, _) => CashServicesResx.ProviderRejectErrorMessage },
            { ProviderUnavailable, (_, _) => CashServicesResx.PaymentProviderUnavailable },
            { InvalidMember, GetInvalidMemberMessage },
            { ServerError, (_, _) => CashServicesResx.AnErrorOccurredWhileProcessingWithdrawalRequest },
        };

    public static string MappingAddManualDepositErrorCode(int errCode) =>
        errCode switch
        {
            AccountDisabled => GetAccountDisabledMessage(),
            AccountClosed => GetAccountClosedMessage(),
            403 => CashServicesResx.UnableToProcessDeposits,
            AccountSuspended => GetAccountSuspendedMessage(),
            InvalidParameters => GetInvalidParametersMessage(),
            AmountExceedsLimits => GetAmountExceedsLimitsMessage(),
            TransactionInsertFailed => GetTransactionInsertFailedMessage(),
            _ => GetTransactionInsertFailedMessage()
        };

    public static string MappingAddManualWithdrawalErrCode(int errCode, int memberId, int agentId)
        => WithdrawalErrorResolvers.TryGetValue(errCode, out var resolve)
            ? resolve(memberId, agentId)
            : GetTransactionInsertFailedMessage();

    public static string MappingUpdatePromoteTransactionStatusErrCode(int errCode, string transId, int memberId, int agentId)
        => errCode switch
        {
            400 => CashServicesResx.ModelValidationFailed,
            AccountDisabled => GetAccountDisabledMessage(),
            AccountClosed => GetAccountClosedMessage(),
            AccountSuspended => GetAccountSuspendedMessage(),
            InvalidParameters => GetInvalidParametersMessage(),
            InsufficientBalance => CashServicesResx.MemberCashBalanceIsInsufficient,
            DailyLimitExceeded => CashServicesResx.ExceedsTheDailyWithdrawalLimit,
            TransactionInsertFailed => GetTransactionInsertFailedMessage(),
            InvalidMember => GetInvalidMemberMessage(memberId, agentId),
            InvalidTransaction => GetInvalidTransactionMessage(transId, memberId),
            ServerError => CashServicesResx.AnErrorOccurredWhileProcessingWithdrawalRequest,
            _ => GetTransactionInsertFailedMessage()
        };

    public static string MappingUpdateTransactionStatus(int errCode, string sqlException, string transId, int memberId, int agentId)
        => errCode switch
        {
            InvalidParameters => GetInvalidParametersMessage(),
            AmountExceedsLimits => GetAmountExceedsLimitsMessage(),
            TransactionInsertFailed => GetTransactionInsertFailedMessage(),
            ProviderRejected => CashServicesResx.ProviderRejectErrorMessage,
            ProviderUnavailable => CashServicesResx.PaymentProviderUnavailable,
            InvalidMember => GetInvalidMemberMessage(memberId, agentId),
            InvalidTransaction => GetInvalidTransactionMessage(transId, memberId),
            InternalServerError => $"{CashServicesResx.InternalServerError} {sqlException}",
            _ => sqlException
        };

    public static string MappingRevertTransactionErrCode(int errCode, int transactionTypeId, string transId, int memberId, int agentId)
        => errCode switch
        {
            InsufficientBalance => GetInsufficientBalanceMessage(transactionTypeId),
            AmountExceedsLimits => GetAmountExceedsLimitsMessage(),
            TransactionInsertFailed => GetTransactionInsertFailedMessage(),
            InvalidMember => GetInvalidMemberMessage(memberId, agentId),
            InvalidTransaction => GetInvalidTransactionMessage(transId, memberId),
            _ => GetTransactionInsertFailedMessage()
        };

    private static string GetInsufficientBalanceMessage(int transactionTypeId) =>
        transactionTypeId == 1 ? CashServicesResx.AgentCashBalanceIsInsufficient : CashServicesResx.MemberCashBalanceIsInsufficient;
}