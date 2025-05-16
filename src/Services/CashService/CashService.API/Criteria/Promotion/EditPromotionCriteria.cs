using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Promotion
{
    public class EditPromotionCriteria : NonQueryCommand
    {
        public string PromotionType { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string Provider { get; set; }

        public decimal SportsPercentage { get; set; }

        public decimal SportsMinTurnover { get; set; }

        public decimal SportsMaxPayout { get; set; }

        public decimal VirtualSportsPercentage { get; set; }

        public decimal VirtualSportsMinTurnover { get; set; }

        public decimal VirtualSportsMaxPayout { get; set; }

        public decimal GamesPercentage { get; set; }

        public decimal GamesMinTurnover { get; set; }

        public decimal GamesMaxPayout { get; set; }

        public decimal LiveCasinoPercentage { get; set; }

        public decimal LiveCasinoMinTurnover { get; set; }

        public decimal LiveCasinoMaxPayout { get; set; }

        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "";

        public override bool IsValid() => true;
    }
}
