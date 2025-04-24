using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class GetBankAccountByIdCriteria : CriteriaBase
    {
        public GetBankAccountByIdCriteria(int bankAccountId)
        {
            BankAccountId = bankAccountId;
        }

        [SpParam(SqlParamName = "BankAccountAgentID")]
        public int BankAccountId { get; }

        public override string GetSettingKey() => "Age_CashService_BankAccountAgent_Sel";

        public override bool IsValid() => BankAccountId > 0;
    }
} 