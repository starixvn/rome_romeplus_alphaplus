namespace AlphaPlus.Settings.ViewLogs.API.Services.Impl
{
    public class CashServiceLogFormatterImpl : BaseLogFormatterImpl, CashServiceLogFormatter
    {
        public CashServiceLogFormatterImpl(RemotingRefWrapper remotingRef, ViewLogIPService ipService) : base(remotingRef, ipService)  
        {
        }

        public string FormatBankAccountNumber(string bankAccountNumber)
        {
            return bankAccountNumber.Replace("NoMatchingBankAccount", GetLogResource("NoMatchingBankAccount"), StringComparison.OrdinalIgnoreCase);
        }

        public string GetTransactionTypeName_OldNewValue(string transactionType)
        {
            return ReplaceOldNewValue(transactionType, GetTransactionTypeName);
        }
 
        public string GetPaymentMethodName_OldNewValue(string paymentMethod)
        {
            return ReplaceOldNewValue(paymentMethod, GetPaymentMethodName);
        }

        public string GetPaymentMethodStatusName_OldNewValue(string paymentMethodStatus)
        {
            return ReplaceOldNewValue(paymentMethodStatus, GetPaymentMethodStatusName);
        }

        public string GetStatus_OldNewValue(string bankStatus)
        {
            return ReplaceOldNewValue(bankStatus, GetStatus);
        }

        public string GetTransactionTypes_OldNewValue(string transactionTypes)
        {
            return ReplaceOldNewValue(transactionTypes, GetTransactionTypes);
        }

        public string GetDepositMatchingMode_OldNewValue(string depositMatchingMode)
        {
            return ReplaceOldNewValue(depositMatchingMode, GetDepositMatchingMode);
        }

        public string GetProviderName_OldNewValue(string providerId)
        {
            return ReplaceOldNewValue(providerId, GetProviderName);
        }

        private string GetTransactionTypeName(string type)
        {
            return GetLogResource("Transaction_" + type);
        }

        private string GetPaymentMethodName(string paymentMethod)
        {
            return remotingRef.GetPaymentMethodName(int.Parse(paymentMethod));
        }
    
        private string GetPaymentMethodStatusName(string paymentMethodStatus)
        {
            return GetLogResource("PaymentMethodStatus_" + paymentMethodStatus);
        }

        private string GetStatus(string status)
        {
            return status == "1" ? GetLogResource("enabled") : GetLogResource("disabled");
        }

        private string GetTransactionTypes(string transactionTypes)
        {
            var types = transactionTypes.Select(t => GetLogResource("TransactionType_" + t));
            return string.Join(", ", types);
        }

        private string GetDepositMatchingMode(string depositMatchingMode)
        {
            return GetLogResource("DepositMatchingMode_" + depositMatchingMode);
        }

        private string GetProviderName(string providerId)
        {
            return remotingRef.GetProviderName(int.Parse(providerId));
        }
    }
}
