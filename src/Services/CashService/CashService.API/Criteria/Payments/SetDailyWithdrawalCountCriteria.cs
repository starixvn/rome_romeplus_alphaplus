using AlphaPlus.CashService.API.Constants;
using AlphaPlus.CashService.API.Requests.Payments;
using Fanex.Data;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class SetDailyWithdrawalCountCriteria(SetDailyWithdrawalCountRequest request) 
        : BaseActionCommand(request.ActorId, request.SubAccId, request.IP, request.Language)
    {
        public int ModeAction { get; } = ModeViewConstants.PaymentGateway; 

        [SpParam(SqlParamName = "DrawLimit")]
        public int DailyWithdrawalCount { get; } = request.Count;

        public decimal MinPerDeposit { get; } = 0;
        public decimal MaxPerDeposit { get; } = 0;
        public decimal MinPerWithdrawal { get; } = 0;
        public decimal MaxPerWithdrawal { get; } = 0;

        public override string GetSettingKey() => "Age_CashService_AgentBankAccountLimit_Upd";

        public override bool IsValid()
            => base.IsValid() &&
                DailyWithdrawalCount >= 0;
    }
} 