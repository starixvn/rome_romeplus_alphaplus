using AlphaPlus.CashService.API.Requests.Payments;
using Fanex.Data;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class DeleteBankAccountCriteria(DeleteBankAccountRequest request) 
        : BaseActionCommand(request.ActorId, request.SubAccId, request.IP, request.Language)
    {
        [SpParam(SqlParamName = "AgentBankAccountID")]
        public int BankAccountId { get; } = request.BankAccountId;
        
        public override string GetSettingKey() => "Age_CashService_BankAccountAgent_Del";

        public override bool IsValid()
            => base.IsValid() &&
                BankAccountId > 0;
    }
} 