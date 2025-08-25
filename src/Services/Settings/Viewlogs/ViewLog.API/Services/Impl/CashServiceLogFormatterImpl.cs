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

        public string GetTrackingTransactionTypeName(string transactionType)
        {
            return ReplaceOldNewValue(transactionType, GetTransactionTypeName);
        }

        public string GetTrackingTransactionName(string transactionType)
        {
            return ReplaceOldNewValue(transactionType, GetTransactionName);
        }

        public string GetTrackingPaymentMethodName(string paymentMethod)
        {
            return ReplaceOldNewValue(paymentMethod, GetPaymentMethodName);
        }

        public string GetTrackingPaymentMethodStatusName(string paymentMethodStatus)
        {
            return ReplaceOldNewValue(paymentMethodStatus, GetPaymentMethodStatusName);
        }

        public string GetTrackingStatus(string bankStatus)
        {
            return ReplaceOldNewValue(bankStatus, GetStatus);
        }

        public string GetTrackingTransactionTypes(string transactionTypes)
        {
            return ReplaceOldNewValue(transactionTypes, GetTransactionTypes);
        }

        public string GetTrackingDepositMatchingMode(string depositMatchingMode)
        {
            return ReplaceOldNewValue(depositMatchingMode, GetDepositMatchingMode);
        }

        public string GetTrackingProviderName(string providerId)
        {
            return ReplaceOldNewValue(providerId, GetProviderName);
        }

        public string GetTrackingTransactionStatus(string transactionStatus)
        {
            return ReplaceOldNewValue(transactionStatus, GetTransactionStatus);
        }

        public string GetTrackingPromotionTypeName(string promotionType)
        {
            return ReplaceOldNewValue(promotionType, GetPromotionTypeName);
        }

        public string GetTrackingRegisterInfo(string registerInfo)
        {
            return ReplaceOldNewValue(registerInfo, GetRegisterInfo);
        }

        public string GetTrackingAllowed(string status)
        {
            return ReplaceOldNewValue(status, GetAllowed);
        }

        private string GetTransactionTypeName(string type)
        {
            return GetLogResource("TransactionType_" + type);
        }

        private string GetTransactionName(string type)
        {
            return GetLogResource("Transaction_" + type);
        }

        private string GetPaymentMethodName(string paymentMethod)
        {
            return remotingRef.GetPaymentMethodName(paymentMethod);
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
            return !string.IsNullOrWhiteSpace(providerId) ? remotingRef.GetProviderName(int.Parse(providerId)) : string.Empty;
        }

        private string GetTransactionStatus(string transactionStatus)
        {
            return GetLogResource("TransactionStatus_" + transactionStatus);
        }

        private string GetPromotionTypeName(string promotionType)
        {
            return GetLogResource("PromotionType_" + promotionType);
        }

        private string GetRegisterInfo(string registerInfo)
        {
            return GetLogResource("CashService_RegisterInfo_" + registerInfo);
        }

        private string GetAllowed(string status)
        {
            return status == "1" ? GetLogResource("Yes") : GetLogResource("No");
        }
    }
}
