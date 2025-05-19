using AlphaPlus.CashService.API.Features.Payments.Constants;
using AlphaPlus.CashService.API.Features.Payments.Requests;
using AlphaPlus.CashService.API.Features.Shared.Criteria;
using Fanex.Data;

namespace AlphaPlus.CashService.API.Features.Payments.Criteria
{
    public class SetTransactionLimitCriteria(SetTransactionLimitRequest request) 
        : BaseDBCommand(request.ActorId, request.SubAccId, request.IP, request.Language)
    {
        public int ModeAction { get; } = ModeViewConstants.BankAccount; 

        [SpParam(SqlParamName = "DrawLimit")]
        public int DailyWithdrawalCount { get; } = request.DailyWithdrawalCount;    

        public decimal MinPerDeposit { get; } = request.MinPerDepositTransaction ;  
        public decimal MaxPerDeposit { get; } = request.MaxPerDepositTransaction ;
        public decimal MinPerWithdrawal { get; } = request.MinPerWithdrawalTransaction ;
        public decimal MaxPerWithdrawal { get; } = request.MaxPerWithdrawalTransaction ;

        public override string GetSettingKey() => "Age_CashService_AgentBankAccountLimit_Upd";

        public override bool IsValid()
            => base.IsValid()
                && DailyWithdrawalCount >= 0 
                && DailyWithdrawalCount <= 32767
                && MinPerDeposit >= 0 
                && MaxPerDeposit >= 0
                && MinPerWithdrawal >= 0 
                && MaxPerWithdrawal >= 0;
    }
} 