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

    public static string MappingAddManualWithdrawalErrCode(int errCode)
        => errCode switch
        {
            400 => CashServicesResx.ModelValidationFailed,
            401 => CashServicesResx.AccountDisabled,
            402 => CashServicesResx.AccountClosed,
            404 => CashServicesResx.TheAccountWasSuspended,
            405 => CashServicesResx.InvalidParameters,
            406 => CashServicesResx.MemberCashBalanceIsInsufficient,
            407 => CashServicesResx.TheAmountExceedsTheAllowedMinimumOrMaximumValue,
            408 => CashServicesResx.ExceedsTheDailyWithdrawalLimit,
            409 => CashServicesResx.TheTransactionInsertedFailedPleaseTryAgain,
            410 => CashServicesResx.ProviderRejectErrorMessage,
            411 => CashServicesResx.PaymentProviderUnavailable,
            500 => CashServicesResx.AnErrorOccurredWhileProcessingWithdrawalRequest,
            _ => CashServicesResx.TheTransactionInsertedFailedPleaseTryAgain
        };

    public static string MappingUpdateTransactionStatus(int errCode, string sqlException)
        => errCode switch
        {
            405 => CashServicesResx.InvalidParameters,
            407 => CashServicesResx.TheAmountExceedsTheAllowedMinimumOrMaximumValue,
            409 => CashServicesResx.TheTransactionInsertedFailedPleaseTryAgain,
            410 => CashServicesResx.ProviderRejectErrorMessage,
            411 => CashServicesResx.PaymentProviderUnavailable,
            499 => $"{CashServicesResx.InternalServerError} {sqlException}",
            _ => sqlException
        };

    public static string MappingRevertTransactionErrCode(int errCode)
        => errCode switch
        {
            405 => CashServicesResx.InvalidParameters,
            406 => CashServicesResx.MemberCashBalanceIsInsufficient,
            407 => CashServicesResx.TheAmountExceedsTheAllowedMinimumOrMaximumValue,
            409 => CashServicesResx.TheTransactionInsertedFailedPleaseTryAgain,
            _ => CashServicesResx.TheTransactionInsertedFailedPleaseTryAgain
        };
}