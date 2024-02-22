namespace AlphaPlus.Main.Site.Features.Dashboard.BalanceInfo
{
    using System;

    public class BalanceUpDayInfo
    {
        public BalanceUpDayInfo(DateTime balanceUpDay, DateTime moveBetsDay)
        {
            BalanceUpDay = balanceUpDay;
            MoveBetsDay = moveBetsDay;
        }

        public DateTime BalanceUpDay { get; }

        public DateTime MoveBetsDay { get; }
    }
}