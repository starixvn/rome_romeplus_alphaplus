namespace AlphaPlus.Reports.Features.Consolidation;

public class ConsolidationByUsername(ConsolidationEntity total, IEnumerable<ConsolidationByPT> consolidationByPTs)
{
    public ConsolidationEntity Total { get; } = total;

    public IEnumerable<ConsolidationByPT> ConsolidationByPTs { get; } = consolidationByPTs;
}
