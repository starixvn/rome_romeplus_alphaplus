namespace AlphaPlus.Settings.MemberInfo.Domain.Models.WinLossLimit
{
    public class PotentialWinLimitDetail
    {
        private const int Percent = 100;

        public int CustId { get; set; }

        public string Username { get; set; }

        public DateTime? LastLogged { get; set; }

        public string Status { get; set; }

        public bool IsProblemAccount { get; set; }

        public decimal Credit { get; set; }

        public bool IsAutoReset { get; set; }

        public decimal PotentialWinLimit { get; set; }

        public int SportType { get; set; }

        public int BetType { get; set; }

        public decimal AccumToWin { get; set; }
        
        public decimal AccumWinLoss { get; set; }

        public decimal PotentialWin
        {
            get
            {
                return AccumToWin + AccumWinLoss;
            }
        }

        public decimal? PotentialWinPercent
        {
            get
            {
                if (PotentialWinLimit == 0)
                {
                    return null;
                }

                if (PotentialWin > 0)
                {
                    return PotentialWin * Percent / PotentialWinLimit;
                }

                return 0;
            }
        }
    }
} 