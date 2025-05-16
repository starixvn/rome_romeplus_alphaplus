using AlphaPlus.CashService.API.Features.Payments.Requests;
using AlphaPlus.CashService.API.Features.Shared.Criteria;
using Fanex.Data;

namespace AlphaPlus.CashService.API.Features.Payments.Criteria
{
    public class UpdateProviderStatusCriteria(UpdateProviderRequest request) 
        : BaseDBCommand(request.ActorId, request.SubAccId, request.IP, request.Language)
    {
        public int ProviderId { get; } = request.ProviderId;

        [SpParam(SqlParamName = "ProviderStatus")]
        public int Status { get; } = request.Status;

        public override string GetSettingKey() => "Age_CashService_PaymentProviderStatus_Upd";

        public override bool IsValid()
            => base.IsValid() &&
                ProviderId > 0;
    }
} 