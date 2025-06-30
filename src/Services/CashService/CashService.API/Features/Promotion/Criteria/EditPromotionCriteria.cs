using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.Promotion.Criteria
{
    public class EditPromotionCriteria : NonQueryCommand
    {
        public int PromotionId { get; set; }
        public int PromotionType { get; set; }
        public int StatusId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public byte[] Banner { get; set; }
        public string ProductSettings { get; set; }
        public int CreatedBy { get; set; }
        public string IP { get; set; }
        public int SiteDbGroupId { get; set; }

        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "";

        public override bool IsValid() => PromotionId > 0;
    }
}