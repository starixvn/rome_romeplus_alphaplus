using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class GetBankAccountsCriteria : CriteriaBase
    {
        public GetBankAccountsCriteria(
            int? bankId = null,
            int? transactionType = null,
            int? status = null,
            string? searchText = null)
        {
            BankId = bankId;
            TransactionType = transactionType;
            Status = status;
            SearchText = searchText;
        }

        [SpParam(SqlParamName = "BankId")]
        public int? BankId { get; }

        [SpParam(SqlParamName = "TransactionType")]
        public int? TransactionType { get; }

        [SpParam(SqlParamName = "Status")]
        public int? Status { get; }

        [SpParam(SqlParamName = "SearchText")]
        public string? SearchText { get; }

        public override string GetSettingKey() => "Age_ManagementPayments_SelBankAccounts";

        public override bool IsValid() => true; // All parameters are optional
    }
} 