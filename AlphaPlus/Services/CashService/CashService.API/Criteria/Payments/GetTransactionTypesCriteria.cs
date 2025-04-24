using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class GetTransactionTypesCriteria : CriteriaBase
    {
        public GetTransactionTypesCriteria(int bankId)
        {
            BankId = bankId;
        }

        public int BankId { get; set; }

        public override string GetSettingKey() => "Age_ManagementPayments_SelTransactionTypes";

        public override bool IsValid() => true;
    }
} 