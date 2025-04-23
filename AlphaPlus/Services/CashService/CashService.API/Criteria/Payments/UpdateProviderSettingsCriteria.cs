using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class UpdateProviderSettingsCriteria : NonQueryCommand
    {
        public UpdateProviderSettingsCriteria(int custId, int providerId, string settings)
        {
            CustId = custId;
            ProviderId = providerId;
            PaymentMethods = settings;
        }

        public int CustId { get; }
        public int ProviderId { get; }
        public string PaymentMethods { get; }

        public int ErrCode { get; set; }
        public string ErrMsg { get; set; } = string.Empty;

        public override string GetSettingKey() => "Age_CashService_PaymentProvider_Upd";

        public override bool IsValid() => ProviderId > 0;
    }
} 