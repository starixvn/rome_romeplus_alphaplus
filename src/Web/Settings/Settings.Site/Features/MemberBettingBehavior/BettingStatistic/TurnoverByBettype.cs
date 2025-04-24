namespace AlphaPlus.Settings.Site.Features.MemberBettingBehavior.BettingStatistic
{
    public class TurnoverByBettype
    {
        public string Bettype { get; set; }

        public string ProductID { get; set; }

        public string ProductName { get; set; }

        public decimal LiveTurnover { get; set; }

        public decimal NonLiveTurnover { get; set; }

        public decimal MixTurnover { get; set; }
    }
}