namespace AlphaPlus.Settings.MemberInfo.Domain.Models.WinLossLimit
{
    using System.Collections.Generic;
    using System.Linq;

    public class WinLossLimitPage
    {
        public WinLossLimitPage()
        {
            NumberOfRows = 0;
            WinLossLimitDetail = Enumerable.Empty<WinLossLimitDetail>();
        }

        public WinLossLimitPage(int numberOfRows, IEnumerable<WinLossLimitDetail> winLossLimitDetail)
        {
            NumberOfRows = numberOfRows;
            WinLossLimitDetail = winLossLimitDetail;
        }

        public int NumberOfRows { get; set; }

        public IEnumerable<WinLossLimitDetail> WinLossLimitDetail { get; set; }

        public static WinLossLimitPage operator +(WinLossLimitPage o1, WinLossLimitPage o2)
        {
            var result = new WinLossLimitPage
            {
                NumberOfRows = o1.NumberOfRows + o2.NumberOfRows,
                WinLossLimitDetail = o1.WinLossLimitDetail.Concat(o2.WinLossLimitDetail)
            };

            return result;
        }
    }
}
