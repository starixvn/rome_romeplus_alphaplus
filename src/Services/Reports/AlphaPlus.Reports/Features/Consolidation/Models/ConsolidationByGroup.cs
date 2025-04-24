
namespace AlphaPlus.Reports.Features.Consolidation;

public class ConsolidationByGroup(string breakdown, IEnumerable<ConsolidationEntity> winloss)
{
    public string BreakDown { get; } = breakdown;

    public IEnumerable<ConsolidationEntity> WinLoss { get; } = winloss;
}