using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class GetBankAccountsCriteria : CriteriaBase
    {
        public GetBankAccountsCriteria(
            int custId,
            int? bankId = null,
            int? transactionType = null,
            int? status = null,
            string searchText = null)
        {
            CustId = custId;
            BankId = bankId;
            TransactionType = transactionType.HasValue && transactionType.Value != 0 ? transactionType.ToString() : null;
            Status = status;
            SearchText = searchText;
        }
        
        public int CustId { get; }

        public int? BankId { get; }

        [SpParam(SqlParamName = "TransTypeList")]
        public string TransactionType { get; }

        [SpParam(SqlParamName = "BankAccountStatus")]
        public int? Status { get; }

        [SpParam(SqlParamName = "SearchAccount")]
        public string SearchText { get; }

        public override string GetSettingKey() => "Age_CashService_BankAccountAgent_List";

        public override bool IsValid() => true; 
    }
} 