using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.Payments.Criteria
{
    public class GetBanksByTransactionTypeCriteria(int custId, int transactionType) : CriteriaBase
    {
        public int CustId { get; } = custId;

        [SpParam(Ignored = true)]
        public int? TransactionType { get; } = transactionType == 0 ? null : transactionType;

        public override string GetSettingKey() => "Age_CashService_BankList";

        public override bool IsValid() => true;
    }
} 