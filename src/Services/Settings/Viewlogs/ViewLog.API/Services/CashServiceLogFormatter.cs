namespace AlphaPlus.Settings.ViewLogs.API.Services
{
    public interface CashServiceLogFormatter : BaseLogFormatter
    {
        string FormatBankAccountNumber(string bankAccountNumber);

        string GetTransactionTypeName(string transactionType);

        string GetPaymentMethodName(int paymentMethod);

        string GetPaymentMethodStatusName(int paymentMethodStatus);

        string GetStatus(int bankStatus);

        string GetTransactionTypes(string transactionTypes);

        string GetDepositMatchingMode(int depositMatchingMode);

        string GetProviderName(int providerId);
    }
}