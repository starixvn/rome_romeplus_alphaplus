using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Promotion
{
    public class DeletePromotionCriteria : NonQueryCommand
    {
        public DeletePromotionCriteria(int promotionId, int custId)
        {
            PromotionId = promotionId;
            CustId = custId;
        }

        [SpParam(SqlParamName = "PromotionId")]
        public int PromotionId { get; }

        [SpParam(SqlParamName = "CustId")]
        public int CustId { get; }

        public int ErrCode { get; set; }
        
        public string ErrMsg { get; set; } = string.Empty;

        public override string GetSettingKey() => "Age_CashService_Promotion_Delete";

        public override bool IsValid() => PromotionId > 0 && CustId > 0;
    }
} 