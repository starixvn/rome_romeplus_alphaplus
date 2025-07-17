using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.Promotion.Criteria
{
    public class UpdatePromotionStatusCriteria : NonQueryCommand
    {
        public int PromotionId { get; set; }
        public int AgentId { get; set; }
        public int SubAccId { get; set; }
        public string IP { get; set; }
        public int SiteDbGroupId { get; set; }
        public bool Enabled { get; set; }
        public string Language { get; set; }
        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Age_CashService_CashPromotionStatusUpd";

        public override bool IsValid() => PromotionId > 0 && AgentId > 0;
    }
}