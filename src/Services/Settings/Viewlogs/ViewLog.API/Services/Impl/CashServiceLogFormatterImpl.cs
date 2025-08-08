namespace AlphaPlus.Settings.ViewLogs.API.Services.Impl
{
    public class CashServiceLogFormatterImpl : BaseLogFormatterImpl, CashServiceLogFormatter
    {
        public CashServiceLogFormatterImpl(RemotingRefWrapper remotingRef, ViewLogIPService ipService) : base(remotingRef, ipService)  
        {
        }

        public string GetBankName(string code)
        {
            return GetLogResource("BankName_" + code);
        }

        public string FormatBankAccountNumber(string bankAccountNumber)
        {
            return bankAccountNumber.Replace("NoMatchingBankAccount", GetLogResource("NoMatchingBankAccount"), StringComparison.OrdinalIgnoreCase);
        }

        public string GetTransactionTypeName(string transactionType)
        {
            return GetLogResource("Transaction_" + transactionType);
        }

        public string GetPaymentMethodName(int paymentMethod)
        {
            return remotingRef.GetPaymentMethodName(paymentMethod);
        }

        public string GetPaymentMethodStatusName(int paymentMethodStatus)
        {
            return GetLogResource("PaymentMethodStatus_" + paymentMethodStatus);
        }

        public string GetStatus(int bankStatus)
        {
            return bankStatus == 1 ? GetLogResource("enabled") : GetLogResource("disabled");
        }

        public string GetTransactionTypes(string transactionTypes)
        {
            var types = transactionTypes.Select(t => GetLogResource("TransactionType_" + t));
            return string.Join(", ", types);
        }

        public string GetDepositMatchingMode(int depositMatchingMode)
        {
            return GetLogResource("DepositMatchingMode_" + depositMatchingMode);
        }
    }
}
