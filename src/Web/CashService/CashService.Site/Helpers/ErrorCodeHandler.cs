namespace AlphaPlus.CashService.Site.Helpers;

using Shared.AppResources.CashServices;

public static class ErrorCodeHandler
{
    public static string MappingAddManualDepositErrorCode(int errCode) =>
        errCode switch
        {
            401 => CashServicesResx.AccountDisabled,
            402 => CashServicesResx.AccountClosed,
            403 => CashServicesResx.UnableToProcessDeposits,
            404 => CashServicesResx.TheAccountWasSuspended,
            405 => CashServicesResx.InvalidParameters,
            407 => CashServicesResx.TheAmountExceedsTheAllowedMinimumOrMaximumValue,
            409 => CashServicesResx.TheTransactionInsertedFailedPleaseTryAgain,
            _ => CashServicesResx.TheTransactionInsertedFailedPleaseTryAgain
        };

    public static string MappingAddManualWithdrawalErrCode(int errCode, int memberId, int agentId)
        => errCode switch
        {
            400 => CashServicesResx.ModelValidationFailed,
            401 => CashServicesResx.AccountDisabled,
            402 => CashServicesResx.AccountClosed,
            404 => CashServicesResx.TheAccountWasSuspended,
            405 => CashServicesResx.InvalidParameters,
            406 => CashServicesResx.MemberCashBalanceIsInsufficient,
            407 => CashServicesResx.TheAmountMustBeBetween0And1AsPerProviderLimits,
            408 => CashServicesResx.ExceedsTheDailyWithdrawalLimit,
            409 => CashServicesResx.TheTransactionInsertedFailedPleaseTryAgain,
            410 => CashServicesResx.ProviderRejectErrorMessage,
            411 => CashServicesResx.PaymentProviderUnavailable,
            415 => string.Format(CashServicesResx.InvalidMemberErrMsg, memberId, agentId),
            500 => CashServicesResx.AnErrorOccurredWhileProcessingWithdrawalRequest,
            _ => CashServicesResx.TheTransactionInsertedFailedPleaseTryAgain
        };

    public static string MappingUpdatePromoteTransactionStatusErrCode(int errCode, string transId, int memberId, int agentId)
        => errCode switch
        {
            400 => CashServicesResx.ModelValidationFailed,
            401 => CashServicesResx.AccountDisabled,
            402 => CashServicesResx.AccountClosed,
            404 => CashServicesResx.TheAccountWasSuspended,
            405 => CashServicesResx.InvalidParameters,
            406 => CashServicesResx.MemberCashBalanceIsInsufficient,
            408 => CashServicesResx.ExceedsTheDailyWithdrawalLimit,
            409 => CashServicesResx.TheTransactionInsertedFailedPleaseTryAgain,
            415 => string.Format(CashServicesResx.InvalidMemberErrMsg, memberId, agentId),
            416 => string.Format(CashServicesResx.InvalidTransactionErrMsg, transId, memberId),
            500 => CashServicesResx.AnErrorOccurredWhileProcessingWithdrawalRequest,
            _ => CashServicesResx.TheTransactionInsertedFailedPleaseTryAgain
        };

    public static string MappingUpdateTransactionStatus(int errCode, string sqlException, string transId, int memberId, int agentId)
        => errCode switch
        {
            405 => CashServicesResx.InvalidParameters,
            407 => CashServicesResx.TheAmountExceedsTheAllowedMinimumOrMaximumValue,
            409 => CashServicesResx.TheTransactionInsertedFailedPleaseTryAgain,
            410 => CashServicesResx.ProviderRejectErrorMessage,
            411 => CashServicesResx.PaymentProviderUnavailable,
            415 => string.Format(CashServicesResx.InvalidMemberErrMsg, memberId, agentId),
            416 => string.Format(CashServicesResx.InvalidTransactionErrMsg, transId, memberId),
            499 => $"{CashServicesResx.InternalServerError} {sqlException}",
            _ => sqlException
        };

    public static string MappingRevertTransactionErrCode(int errCode, int transactionTypeId, string transId, int memberId, int agentId)
        => errCode switch
        {
            406 => transactionTypeId == 1 ? CashServicesResx.AgentCashBalanceIsInsufficient : CashServicesResx.MemberCashBalanceIsInsufficient,
            407 => CashServicesResx.TheAmountExceedsTheAllowedMinimumOrMaximumValue,
            409 => CashServicesResx.TheTransactionInsertedFailedPleaseTryAgain,
            415 => string.Format(CashServicesResx.InvalidMemberErrMsg, memberId, agentId),
            416 => string.Format(CashServicesResx.InvalidTransactionErrMsg, transId, memberId),
            _ => CashServicesResx.TheTransactionInsertedFailedPleaseTryAgain
        };
}