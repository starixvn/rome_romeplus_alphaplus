using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class GetBanksByTransactionTypeCriteria : CriteriaBase
    {
        public GetBanksByTransactionTypeCriteria(int transactionType)
        {
            TransactionType = transactionType == 0 ? null : transactionType;
        }

        [SpParam(Ignored = true)]
        public int? TransactionType { get; }

        public override string GetSettingKey() => "Age_CashService_BankList";

        public override bool IsValid() => true;
    }
} 