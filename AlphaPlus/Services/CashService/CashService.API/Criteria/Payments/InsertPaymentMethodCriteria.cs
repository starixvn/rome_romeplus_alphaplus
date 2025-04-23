using AlphaPlus.CashService.API.Models.Payments;
using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class InsertPaymentMethodCriteria : NonQueryCommand
    {
        public InsertPaymentMethodCriteria(PaymentMethodModel model)
        {
            CustId = model.CustId;
            ProviderId = model.ProviderId;
            TransactionType = model.TransactionType;
            Method = model.Method;

            APIMethodKey = model.APIMethodKey ?? string.Empty;
            MerchantID = model.MerchantID ?? string.Empty;
            AppSecret = model.AppSecret ?? string.Empty;
        }

        public int CustId { get; }
        public int ProviderId { get; }
        public int TransactionType { get; }

        [SpParam(SqlParamName = "PaymentMethodID")]
        public int Method { get; }

        public string APIMethodKey { get; }
        public string MerchantID { get; }
        public string AppSecret { get; }

        public int ErrCode { get; set; }
        public string ErrMsg { get; set; } = "";

        public override string GetSettingKey() => "Age_CashService_PaymentProvider_Ins";

        public override bool IsValid() => CustId > 0 && ProviderId > 0;
    }
} 