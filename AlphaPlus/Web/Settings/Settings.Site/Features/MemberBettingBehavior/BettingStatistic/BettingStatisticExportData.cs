namespace AlphaPlus.Settings.Site.Features.MemberBettingBehavior.BettingStatistic
{
    public class BettingStatisticExportData
    {
        public BettingStatisticExportData(
            IEnumerable<BettingStatisticExportRow> bettingStatisticExportRows,
            BettingStatisticExportRow total)
        {
            BettingStatisticExportRows = bettingStatisticExportRows;
            Total = total;
        }

        public IEnumerable<BettingStatisticExportRow> BettingStatisticExportRows { get; set; }

        public BettingStatisticExportRow Total { get; set; }
    }
}