using AlphaPlus.CashService.API.Features.Payments.Constants;
using AlphaPlus.CashService.API.Features.Payments.Requests;
using AlphaPlus.CashService.API.Features.Shared.Criteria;
using Fanex.Data;

namespace AlphaPlus.CashService.API.Features.Payments.Criteria
{
    public class SetDailyWithdrawalCountCriteria(SetDailyWithdrawalCountRequest request) 
        : BaseDBCommand(request.ActorId, request.SubAccId, request.IP, request.Language)
    {
        public int ModeAction { get; } = ModeViewConstants.PaymentGateway; 

        [SpParam(SqlParamName = "DrawLimit")]
        public int DailyWithdrawalCount { get; } = request.Count;

        public decimal MinPerDeposit { get; }
        public decimal MaxPerDeposit { get; }
        public decimal MinPerWithdrawal { get; }
        public decimal MaxPerWithdrawal { get; }

        public override string GetSettingKey() => "Age_CashService_AgentBankAccountLimit_Upd";

        public override bool IsValid()
            => base.IsValid() 
                && DailyWithdrawalCount > 0 
                && DailyWithdrawalCount <= 32767;
    }
} 