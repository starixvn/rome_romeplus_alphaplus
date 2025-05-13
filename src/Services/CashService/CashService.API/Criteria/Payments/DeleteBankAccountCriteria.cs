using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class DeleteBankAccountCriteria(int bankAccountId) : NonQueryCommand
    {
        [SpParam(SqlParamName = "AgentBankAccountID")]
        public int BankAccountId { get; } = bankAccountId;

        public int ErrCode { get; set; }
        public string ErrMsg { get; set; } = string.Empty;
        
        public override string GetSettingKey() => "Age_CashService_BankAccountAgent_Del";

        public override bool IsValid() => BankAccountId > 0;
    }
} 