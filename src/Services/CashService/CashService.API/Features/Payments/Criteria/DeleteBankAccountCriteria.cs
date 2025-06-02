using AlphaPlus.CashService.API.Features.Payments.Requests;
using AlphaPlus.CashService.API.Features.Shared.Criteria;
using Fanex.Data;

namespace AlphaPlus.CashService.API.Features.Payments.Criteria
{
    public class DeleteBankAccountCriteria(DeleteBankAccountRequest request) 
        : BaseDBCommand(request.ActorId, request.SubAccId, request.IP, request.Language)
    {
        [SpParam(SqlParamName = "AgentBankAccountID")]
        public int BankAccountId { get; } = request.BankAccountId;
        
        public override string GetSettingKey() => "Age_CashService_BankAccountAgent_Del";

        public override bool IsValid()
            => base.IsValid() &&
                BankAccountId > 0;
    }
} 