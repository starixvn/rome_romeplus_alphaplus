using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.Payments.Criteria
{
    public class GetBankAccountsCriteria(
        int custId,
        int? bankId = null,
        int? transactionType = null,
        int? status = null,
        string searchText = null)
        : CriteriaBase
    {
        public int CustId { get; } = custId;

        public int? BankId { get; } = bankId;

        [SpParam(SqlParamName = "TransTypeList")]
        public string TransactionType { get; } = transactionType.HasValue && transactionType.Value != 0 ? transactionType.ToString() : null;

        [SpParam(SqlParamName = "BankAccountStatus")]
        public int? Status { get; } = status == 2 ? null : status;

        [SpParam(SqlParamName = "SearchAccount")]
        public string SearchText { get; } = searchText;

        public override string GetSettingKey() => "Age_CashService_BankAccountAgent_List";

        public override bool IsValid() => true; 
    }
} 