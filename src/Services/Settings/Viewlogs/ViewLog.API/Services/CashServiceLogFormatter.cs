namespace AlphaPlus.Settings.ViewLogs.API.Services
{
    public interface CashServiceLogFormatter : BaseLogFormatter
    {
        string FormatBankAccountNumber(string bankAccountNumber);

        string GetTransactionTypeName_OldNewValue(string transactionType);

        string GetPaymentMethodName_OldNewValue(string paymentMethod);

        string GetPaymentMethodStatusName_OldNewValue(string paymentMethodStatus);

        string GetStatus_OldNewValue(string bankStatus);

        string GetTransactionTypes_OldNewValue(string transactionTypes);

        string GetDepositMatchingMode_OldNewValue(string depositMatchingMode);

        string GetProviderName_OldNewValue(string providerId);

        string GetTransactionStatus_OldNewValue(string transactionStatus);
    }
}