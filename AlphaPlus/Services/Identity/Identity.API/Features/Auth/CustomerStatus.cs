namespace AlphaPlus.Identity.API.Features.Auth
{
    public class CustomerStatus
    {
        public bool DoubleCommLosing { get; init; }

        public bool DisableAutoMPT { get; init; }

        public int SRecommend { get; init; }

        public int MRecommend { get; init; }

        public int ARecommend { get; init; }

        public bool IsAdminCanViewVNIP { get; init; }

        public bool IsMyanmarOdds { get; init; }
    }
}