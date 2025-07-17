using AlphaPlus.CashService.API.Features.Payments.Requests;
using AlphaPlus.CashService.API.Features.Shared.Criteria;
using Fanex.Data;

namespace AlphaPlus.CashService.API.Features.Payments.Criteria
{
    public class InsertPaymentMethodCriteria(InsertPaymentMethodRequest request, bool validationResult) 
        : BaseDBCommand(request.ActorId, request.SubAccId, request.IP, request.Language)
    {
        public int ProviderId { get; } = request.ProviderId;
        public int TransactionType { get; } = request.TransactionType;

        [SpParam(SqlParamName = "PaymentMethodID")]
        public int Method { get; } = request.Method;

        public bool ValidationResult { get; } = validationResult;
        
        public string APIConfig { get; } = request.GetAPIConfigJson();

        public override string GetSettingKey() => "Age_CashService_PaymentMethod_Config";

        public override bool IsValid()
            => base.IsValid() &&
                ProviderId > 0;
    }
} 