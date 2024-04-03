namespace AlphaPlus.Reports.Features.Consolidation;

public class ConsolidationByDate : ConsolidationEntity
{
    public string BreakDown { get; }

    public DateTime Date => Convert.ToDateTime(BreakDown);
}