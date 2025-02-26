using AlphaPlus.Reports.Features.Shared.Extensions;

namespace AlphaPlus.Reports.Site.Features.SuperWinLossDetailByPT;

public record SuperPTProductViewModel(decimal PT, IEnumerable<string> ProductNames)
{
    public string Message => $"{PT.FormatPTPercentage()}: {ProductNames.JoinWithComma(", ")}";
}