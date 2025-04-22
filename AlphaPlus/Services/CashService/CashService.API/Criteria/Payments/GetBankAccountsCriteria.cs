using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class GetBankAccountsCriteria : CriteriaBase
    {
        public GetBankAccountsCriteria(
            int? bankId = null,
            string? accountNumber = null,
            string? accountHolderName = null,
            int? transactionType = null,
            int? status = null)
        {
            BankId = bankId;
            AccountNumber = accountNumber;
            AccountHolderName = accountHolderName;
            TransactionType = transactionType;
            Status = status;
        }

        [SpParam(SqlParamName = "BankId")]
        public int? BankId { get; }

        [SpParam(SqlParamName = "AccountNumber")]
        public string? AccountNumber { get; }

        [SpParam(SqlParamName = "AccountHolderName")]
        public string? AccountHolderName { get; }

        [SpParam(SqlParamName = "TransactionType")]
        public int? TransactionType { get; }

        [SpParam(SqlParamName = "Status")]
        public int? Status { get; }

        public override string GetSettingKey() => "Age_ManagementPayments_SelBankAccounts";

        public override bool IsValid() => true; // All parameters are optional
    }
} 