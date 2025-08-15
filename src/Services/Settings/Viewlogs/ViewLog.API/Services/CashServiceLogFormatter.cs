namespace AlphaPlus.Settings.ViewLogs.API.Services
{
    public interface CashServiceLogFormatter : BaseLogFormatter
    {
        string FormatBankAccountNumber(string bankAccountNumber);

        string GetTrackingTransactionTypeName(string transactionType);

        string GetTrackingTransactionName(string transactionType);

        string GetTrackingPaymentMethodName(string paymentMethod);

        string GetTrackingPaymentMethodStatusName(string paymentMethodStatus);

        string GetTrackingStatus(string bankStatus);

        string GetTrackingTransactionTypes(string transactionTypes);

        string GetTrackingDepositMatchingMode(string depositMatchingMode);

        string GetTrackingProviderName(string providerId);

        string GetTrackingTransactionStatus(string transactionStatus);

        string GetTrackingPromotionTypeName(string promotionType);

        string GetTrackingRegisterInfo(string registerInfo);
    }
}