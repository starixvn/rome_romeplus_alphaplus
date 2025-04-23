namespace AlphaPlus.Settings.Site.Features.MemberBettingBehavior.BettingStatistic
{
    public class MemberBettingStatistic
    {
        public MemberBettingStatistic(
             SummaryStatistic summaryStatistic,
             IEnumerable<TurnOverBySport> turnOverBySports,
             IEnumerable<TurnoverByBettype> turnoverByBettypes,
             IEnumerable<TurnoverWinLossByDate> turnoverWinLossByDates
            )
        {
            SummaryStatistic = summaryStatistic;
            TurnOverBySports = turnOverBySports;
            TurnoverByBettypes = turnoverByBettypes;
            TurnoverWinLossByDates = turnoverWinLossByDates;
        }

        public SummaryStatistic SummaryStatistic { get; }

        public IEnumerable<TurnOverBySport> TurnOverBySports { get; }

        public IEnumerable<TurnoverByBettype> TurnoverByBettypes { get; }

        public IEnumerable<TurnoverWinLossByDate> TurnoverWinLossByDates { get; }
    }
}