using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class DeleteBankAccountCriteria(int bankAccountId) : NonQueryCommand
    {
        [SpParam(SqlParamName = "BankAccountId")]
        public int BankAccountId { get; } = bankAccountId;

        public override string GetSettingKey() => "Age_ManagementPayments_DelBankAccount";

        public override bool IsValid() => BankAccountId > 0;
    }
} 