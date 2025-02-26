namespace AlphaPlus.Main.Site.Features.NetworkSpeed
{
    public class NetworkSpeedViewModel
    {
        public NetworkSpeedViewModel(bool requiredCalculation)
        {
            RequiredCalculation = requiredCalculation;
        }

        public bool RequiredCalculation { get; }
    }
}