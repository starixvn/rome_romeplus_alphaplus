namespace AlphaPlus.Reports.Features.Consolidation;

public class ConsolidationByUsername : ConsolidationEntity
{
    public int BreakDown { get; init; }

    public string ProductName { get; init; }

    public decimal PositionTaking { get; init; }
}