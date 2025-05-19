using AlphaPlus.CashService.API.Features.Payments.Requests;
using AlphaPlus.CashService.API.Features.Shared.Criteria;

namespace AlphaPlus.CashService.API.Features.Payments.Criteria
{
    public class UpdateProviderSettingsCriteria(UpdateProviderRequest request, string settings) 
        : BaseDBCommand(request.ActorId, request.SubAccId, request.IP, request.Language)
    {
        public int ProviderId { get; } = request.ProviderId;
        public string PaymentMethods { get; } = settings;

        public override string GetSettingKey() => "Age_CashService_PaymentMethod_Upd";

        public override bool IsValid()
            => base.IsValid() &&
                ProviderId > 0;
    }
} 