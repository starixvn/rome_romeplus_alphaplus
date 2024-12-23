namespace AlphaPlus.Main.Site.Features.Dashboard.BalanceInfo
{
    public class BalanceInfo
    {
        public string UserName { get; init; }

        public decimal CashBalance { get; init; }

        public decimal YesterdayCashBalance { get; init; }

        public decimal TotalBalance { get; init; }

        public decimal YesterdayTotalBalance { get; init; }

        public decimal TodayWinLoss { get; init; }

        public decimal YesterdayWinLoss { get; init; }

        public decimal GivenCredit { get; init; }

        public decimal MemberCredit { get; init; }

        public decimal PendingTransfer3rd { get; init; }

        public decimal? RewardPayable { get; set; }

        public decimal? RewardReceivable { get; set; }

        public string RewardCurrency { get; set; }
    }
}