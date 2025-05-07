using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class UpdateProviderSettingsCriteria(int custId, int providerId, string settings) : NonQueryCommand
    {
        public int CustId { get; } = custId;
        public int ProviderId { get; } = providerId;
        public string PaymentMethods { get; } = settings;

        public int ErrCode { get; set; }
        public string ErrMsg { get; set; } = string.Empty;

        public override string GetSettingKey() => "Age_CashService_PaymentProvider_Upd";

        public override bool IsValid() => ProviderId > 0 && CustId > 0;
    }
} 