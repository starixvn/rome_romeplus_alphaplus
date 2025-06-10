namespace AlphaPlus.Settings.MemberInfo.Domain.Models.WinLossLimit
{
    public class PotentialWinLimitPage
    {
        public PotentialWinLimitPage()
        {
            NumberOfRows = 0;
            PotentialWinLimitDetail = Enumerable.Empty<PotentialWinLimitDetail>();
        }

        public PotentialWinLimitPage(int numberOfRows, IEnumerable<PotentialWinLimitDetail> PotentialwinLimitDetail)
        {
            NumberOfRows = numberOfRows;
            PotentialWinLimitDetail = PotentialwinLimitDetail;
        }

        public int NumberOfRows { get; set; }

        public new IEnumerable<PotentialWinLimitDetail> PotentialWinLimitDetail { get; set; }

        public static PotentialWinLimitPage operator +(PotentialWinLimitPage o1, PotentialWinLimitPage o2)
        {
            var result = new PotentialWinLimitPage
            {
                NumberOfRows = o1.NumberOfRows + o2.NumberOfRows,
                PotentialWinLimitDetail = o1.PotentialWinLimitDetail.Concat(o2.PotentialWinLimitDetail)
            };

            return result;
        }
    }
} 