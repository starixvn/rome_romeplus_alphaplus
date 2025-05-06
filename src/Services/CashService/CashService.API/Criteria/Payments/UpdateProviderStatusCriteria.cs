using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class UpdateProviderStatusCriteria : NonQueryCommand
    {
        public UpdateProviderStatusCriteria(int custId, int providerId, int status)
        {
            CustId = custId;
            ProviderId = providerId;
            Status = status;
        }

        public int CustId { get; }
        public int ProviderId { get; }

        [SpParam(SqlParamName = "ProviderStatus")]
        public int Status { get; }

        public int ErrCode { get; set; }
        public string ErrMsg { get; set; } = string.Empty;

        public override string GetSettingKey() => "Age_CashService_PaymentProviderStatus_Upd";

        public override bool IsValid() => ProviderId > 0 && CustId > 0;
    }
} 