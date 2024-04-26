namespace AlphaPlus.Settings.MemberInfo.Domain.Models.WinLossLimit
{
    public class WinLossLimitDetail
    {
        private const int Percent = 100;

        public int CustId { get; set; }

        public string? PlayerId { get; set; }

        public decimal Balance { get; set; }

        public decimal CreditBalance { get; set; }

        public decimal DebitBalance { get; set; }

        public string? Username { get; set; }

        public string? Firstname { get; set; }

        public string? Lastname { get; set; }

        public bool Enabled { get; set; }

        public bool Suspended { get; set; }

        public bool Closed { get; set; }

        public byte ClosedReason { get; set; }

        public byte Locked { get; set; }

        public decimal Credit { get; set; }

        public bool IsAutoReset { get; set; }

        public decimal MaxWinLimit { get; set; }

        public decimal? MaxLossLimit { get; set; }

        public decimal Outstanding { get; set; }

        public decimal Transfer3rd { get; set; }

        public decimal? CurrentWinAnmout
        {
            get
            {
                var isUnlimited = MaxWinLimit == 0 && (!MaxLossLimit.HasValue || MaxLossLimit.Value == 0);

                return isUnlimited ? (decimal?)null : CreditBalance - DebitBalance;
            }
        }

        public decimal? WinPercent
        {
            get
            {
                if (MaxWinLimit == 0)
                {
                    return null;
                }

                if (CreditBalance - DebitBalance > 0 && Credit == 0 && MaxWinLimit > 0)
                {
                    return (CreditBalance - DebitBalance) * Percent;
                }

                if (CreditBalance - DebitBalance > 0 && Credit > 0 && MaxWinLimit > 0)
                {
                    return (CreditBalance - DebitBalance) * Percent / (MaxWinLimit * Credit);
                }

                return 0;
            }
        }

        public string Status
        {
            get
            {
                if (!Enabled)
                {
                    return "Disabled";
                }

                if (Closed)
                {
                    return "Closed";
                }

                if (Suspended)
                {
                    return "Suspended";
                }

                return "Open";
            }
        }

        public decimal? RemainingBuyInAmount
            => MaxLossLimit.HasValue ? MaxLossLimit - DebitBalance + CreditBalance - Outstanding : null;

        public decimal? ReaminingTransferInAmount
            => MaxLossLimit.HasValue ? MaxLossLimit - DebitBalance + CreditBalance + Transfer3rd : null;

        public decimal? LossPercent
        {
            get
            {
                return !MaxLossLimit.HasValue || MaxLossLimit.Value == 0
                    ? (decimal?)null :
                    -CurrentWinAnmout * Percent / MaxLossLimit.Value;
            }
        }
    }
}