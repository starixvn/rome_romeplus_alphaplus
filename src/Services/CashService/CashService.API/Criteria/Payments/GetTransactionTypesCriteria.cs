using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class GetTransactionTypesCriteria : CriteriaBase
    {
        public GetTransactionTypesCriteria(int bankId)
        {
            BankId = bankId;
        }

        [SpParam(SqlParamName = "TransactionTypeID")]
        public int BankId { get; set; }

        public override string GetSettingKey() => "Age_CashService_TransTypeControl";

        public override bool IsValid() => true;
    }
} 