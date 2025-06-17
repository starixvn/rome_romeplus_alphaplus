namespace AlphaPlus.CashService.Site.Helpers;

using Shared.AppResources.CashServices;

public static class ErrorCodeHandler
{
    public static string MappingDepositErrorCode(int errCode) =>
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
            500 => CashServicesResx.AnErrorOccurredWhileProcessingWithdrawalRequest,
            _ => CashServicesResx.TheTransactionInsertedFailedPleaseTryAgain
        };
}