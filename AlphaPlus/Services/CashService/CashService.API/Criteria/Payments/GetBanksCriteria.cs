using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class GetBanksCriteria : CriteriaBase
    {
        public GetBanksCriteria(int transactionType)
        {
            TransactionType = transactionType;
        }

        [SpParam(Ignored = true)]
        public int TransactionType { get; }

        public override string GetSettingKey() => "Age_CashService_BankList";

        public override bool IsValid() => true;
    }
} 