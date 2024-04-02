namespace AlphaPlus.Reports.Site.Features.SuperWinLossDetailByPT;

public record SuperPTProductViewModel(decimal PositionTaking, string ProductNames)
{
    public string Message
    {
        get
        {
            const decimal percentage = 100.0M;
            var positionTakingPercentage = PositionTaking * percentage;

            return $"{positionTakingPercentage.ToString("N1")}% {Resources.PT}: {ProductNames}";
        }
    }
}