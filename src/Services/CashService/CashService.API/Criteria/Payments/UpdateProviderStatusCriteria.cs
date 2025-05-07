using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class UpdateProviderStatusCriteria(int custId, int providerId, int status) : NonQueryCommand
    {
        public int CustId { get; } = custId;
        public int ProviderId { get; } = providerId;

        [SpParam(SqlParamName = "ProviderStatus")]
        public int Status { get; } = status;

        public int ErrCode { get; set; }
        public string ErrMsg { get; set; } = string.Empty;

        public override string GetSettingKey() => "Age_CashService_PaymentProviderStatus_Upd";

        public override bool IsValid() => ProviderId > 0 && CustId > 0;
    }
} 