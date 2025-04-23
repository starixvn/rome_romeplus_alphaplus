using AlphaPlus.CashService.API.Models.Payments;
using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class UpdateProviderSettingsCriteria : NonQueryCommand
    {
        public UpdateProviderSettingsCriteria(int custId, int providerId, ProviderSettingsModel model)
        {
            CustId = custId;
            ProviderId = providerId;
            Method = model.Method;

            Status = model.Status;
            TransactionType = model.TransactionType;
            MinAmount = model.MinAmount;
            MaxAmount = model.MaxAmount;
            IsEnabled = model.IsEnabled;
        }

        public int CustId { get; }
        public int ProviderId { get; }

        [SpParam(SqlParamName = "PaymentMethods")]
        public int Method { get; }

        [SpParam(Ignored = true)]
        public int Status { get; }

        [SpParam(Ignored = true)]
        public int TransactionType { get; }

        [SpParam(Ignored = true)]
        public decimal MinAmount { get; }

        [SpParam(Ignored = true)]
        public decimal MaxAmount { get; }

        [SpParam(Ignored = true)]
        public bool IsEnabled { get; }

        public int ErrCode { get; set; }
        public string ErrMsg { get; set; } = string.Empty;

        public override string GetSettingKey() => "Age_CashService_PaymentProvider_Upd";

        public override bool IsValid() => ProviderId > 0 && MinAmount >= 0 && MaxAmount >= MinAmount;
    }
} 