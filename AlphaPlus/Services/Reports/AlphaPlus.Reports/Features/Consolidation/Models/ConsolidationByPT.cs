namespace AlphaPlus.Reports.Features.Consolidation;

public class ConsolidationByPT(ConsolidationEntity total, IEnumerable<ConsolidationEntity> winloss)
{
    public ConsolidationEntity Total { get; } = total;

    public IEnumerable<ConsolidationEntity> WinLoss { get; } = winloss;
}