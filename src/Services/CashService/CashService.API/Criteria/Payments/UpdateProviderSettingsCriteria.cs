using AlphaPlus.CashService.API.Requests.Payments;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class UpdateProviderSettingsCriteria(UpdateProviderRequest request, string settings) 
        : BaseActionCommand(request.ActorId, request.SubAccId, request.IP, request.Language)
    {
        public int ProviderId { get; } = request.ProviderId;
        public string PaymentMethods { get; } = settings;

        public override string GetSettingKey() => "Age_CashService_PaymentProvider_Upd";

        public override bool IsValid()
            => base.IsValid() &&
                ProviderId > 0;
    }
} 