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
        }

        public int CustId { get; }
        public int ProviderId { get; }
        public int TransactionType { get; }

        [SpParam(SqlParamName = "PaymentMethodID")]
        public int Method { get; }

        public string APIMethodKey { get; set; }
        public string MerchantID { get; set; }
        public string AppSecret { get; set; }

        public int ErrCode { get; set; }
        public string ErrMsg { get; set; } = "";

        public override string GetSettingKey() => "Age_CashService_PaymentProvider_Ins";

        public override bool IsValid() => CustId > 0 && ProviderId > 0;
    }
} 