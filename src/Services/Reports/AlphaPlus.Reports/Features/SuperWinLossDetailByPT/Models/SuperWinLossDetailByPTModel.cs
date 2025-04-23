using AlphaPlus.Reports.Features.Shared.Extensions;

namespace AlphaPlus.Reports.Features.SuperWinLossDetailByPT;

public class SuperWinLossDetailByPTModel(
    decimal superPT,
    IEnumerable<SuperWinLossDetailByPTEntity> superWinLossDetails,
    SuperWinLossDetailByPTEntity total)
{
    public decimal SuperPT { get; } = superPT;

    public string SuperPTPercentage { get; } = superPT.FormatPTPercentage();

    public IEnumerable<SuperWinLossDetailByPTEntity> SuperWinLossDetails { get; } = superWinLossDetails;

    public SuperWinLossDetailByPTEntity Total { get; } = total;
}