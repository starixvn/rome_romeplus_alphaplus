using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class GetBankAccountByIdCriteria(int bankAccountId) : CriteriaBase
    {
        [SpParam(SqlParamName = "AgentBankAccountID")]
        public int BankAccountId { get; } = bankAccountId;

        public override string GetSettingKey() => "Age_CashService_BankAccountAgent_Sel";

        public override bool IsValid() => BankAccountId > 0;
    }
} 