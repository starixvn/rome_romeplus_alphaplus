namespace AlphaPlus.Settings.Site.Features.ProductStatus.Params
{
    public class BetTypeStatusParam(int sportType, int betType, bool isDisabled)
    {
        public int SportType { get; } = sportType;

        public int BetType { get; } = betType;

        public bool IsDisabled { get; } = isDisabled;
    }
}