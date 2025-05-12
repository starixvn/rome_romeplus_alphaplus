namespace AlphaPlus.CashService.API.Models.Promotion
{
    public class PromotionModel
    {
        public int PromotionId { get; set; }

        public string PromotionTitle { get;set; }

        public string PromotionType { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public decimal SportsPercentage { get; set; }

        public decimal VirtualSportsPercentage { get; set; }

        public decimal GamesPercentage { get; set; }

        public decimal LiveCasinoPercentage { get; set; }

        public decimal SportsMaxPayout { get; set; }

        public decimal VirtualSportsMaxPayout { get; set; }

        public decimal GamesMaxPayout { get; set; }

        public decimal LiveCasinoMaxPayout { get; set; }

        public string Period { get; set; }

        public string Provider { get; set; }

        public string Status { get; set; }  

        public string LastUpdated { get; set; }
    }
}
