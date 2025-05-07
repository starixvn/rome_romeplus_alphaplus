using AlphaPlus.CashService.API.Models.Payments;
using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class InsertPaymentMethodCriteria(PaymentMethodModel model) : NonQueryCommand
    {
        public int CustId { get; } = model.CustId;
        public int ProviderId { get; } = model.ProviderId;
        public int TransactionType { get; } = model.TransactionType;

        [SpParam(SqlParamName = "PaymentMethodID")]
        public int Method { get; } = model.Method;

        public string APIMethodKey { get; set; }
        public string MerchantID { get; set; }
        public string AppSecret { get; set; }

        public int ErrCode { get; set; }
        public string ErrMsg { get; set; } = "";

        public override string GetSettingKey() => "Age_CashService_PaymentProvider_Ins";

        public override bool IsValid() => CustId > 0 && ProviderId > 0;
    }
} 