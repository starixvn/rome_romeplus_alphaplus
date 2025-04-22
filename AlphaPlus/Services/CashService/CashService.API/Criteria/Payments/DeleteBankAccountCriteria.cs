using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class DeleteBankAccountCriteria : NonQueryCommand
    {
        public DeleteBankAccountCriteria(int bankAccountId)
        {
            BankAccountId = bankAccountId;
        }

        [SpParam(SqlParamName = "BankAccountId")]
        public int BankAccountId { get; }

        public override string GetSettingKey() => "Age_ManagementPayments_DelBankAccount";

        public override bool IsValid() => BankAccountId > 0;
    }
} 