namespace AlphaPlus.Settings.Site.Features.MemberBettingBehavior.BettingStatistic
{
    public class BettingStatisticExportRow
    {
        public string Sporttype { get; set; }

        public string Bettype { get; set; }

        public decimal WinningWinLoss { get; set; }

        public decimal LosingWinLoss { get; set; }

        public decimal TotalWinLoss { get; set; }

        public decimal Margin { get; set; }

        public decimal LiveTurnover { get; set; }

        public decimal NonLiveTurnover { get; set; }

        public decimal MixTurnover { get; set; }

        public decimal Turnover { get; set; }

        public decimal NetTurnOver { get; set; }
    }
}